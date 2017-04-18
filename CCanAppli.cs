using System;
using Microsoft.SPOT;
using System.Collections;
using System.Threading;
using GHI.IO;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using Gadgeteer.Modules.GHIElectronics;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Microsoft.SPOT.Hardware;
using System.IO.Ports;


namespace PR
{
    enum idErreur : int
    {
        notDefined, OK = 1, STOPPED = 2, BLOCKED = 3, ACKSTOP = 5, TOCANCEL,
        // Code erreur
        FatalErreurCAN = -1, ErrorReceived = -2, timoutACK = -3, timoutDONE = -4,
    };

    enum IDcommande : uint
    {
        // Id WIDAR

        WStop = 0x100, WStopACK, WStopDONE,
        WSetColor = 0x103, WSetColorACK, WSetColorDONE,
        WSetSpeed = 0x106, WSetSpeedACK, WSetSpeedDONE,
        WGoTo = 0x109, WGoToACK, WGoToDONE,
        WGoTo2 = 0x10C, WGoTo2ACK, WGoTo2DONE,
        WRotate = 0x10F, WRotateACK, WRotateDONE,
        WGetPosition = 0x112, WGetPositionACK, WGetPositionDONE,
        WGetCodeur = 0x115, WGetCodeurACK, WGetCodeurDONE,
        WBeBlocked = 280
    };

    class CCanAppli
    {
        //private GTM.GHIElectronics.CANDW  m_busCan;
        private ControllerAreaNetwork m_busCan;
        private int returnWaitAny;
        static private object mutex;
        uint m_id;

        Bitmap m_LCD;
        Font m_font;



        // Déclaration du tableau de data reçu.
        ControllerAreaNetwork.Message[] m_msgRecu;
        //  CAN.Message[] m_msgRecu = new CAN.Message[1];

        // Déclaration des Evenements
        static WaitHandle[] m_tabWaitDONE;
        static WaitHandle[] m_waitACK;
        idErreur m_erreur;

        public CCanAppli(Font font) //J'ai modifié le code en supprimant le Bitmap LCD du paramètre
        {
            //m_busCan=new GTM.GHIElectronics.CANDW(6);
            m_busCan = new ControllerAreaNetwork(ControllerAreaNetwork.Channel.One, ControllerAreaNetwork.Speed.Kbps250);
            m_font = font;
            //    m_LCD = LCD;
            //
            m_tabWaitDONE = new WaitHandle[]
            {
                new AutoResetEvent(false),  //DONE
                new AutoResetEvent(false),  //STOPPED
                new AutoResetEvent(false)   //BLOCKED
            };

            m_waitACK = new WaitHandle[]
            {
                new AutoResetEvent(false),
                new AutoResetEvent(false),
            };


            m_msgRecu = new ControllerAreaNetwork.Message[10];
            for (int i = 0; i < 10; i++)
                m_msgRecu[i] = new ControllerAreaNetwork.Message();
            //  m_busCan.Initialize(GHI.IO.ControllerAreaNetwork.Speed.Kbps125, GHI.IO.ControllerAreaNetwork.Channel.One);
            //m_busCan.ErrorReceived += m_busCan_ErrorReceived;
            //    m_busCan.MessagesReceived += m_busCan_MessagesReceived;
            m_busCan.ErrorReceived += m_busCan_ErrorReceived;
            m_busCan.MessageAvailable += m_busCan_MessageAvailable;
            m_busCan.Enabled = true;

            // mutex = new Object();
        }

        void m_busCan_MessageAvailable(ControllerAreaNetwork sender, ControllerAreaNetwork.MessageAvailableEventArgs e)
        {
            m_msgRecu[0] = sender.ReadMessage();
            Debug.Print("CanId reçu" + m_msgRecu[0].ArbitrationId.ToString());

            AutoResetEvent evtDONE = (AutoResetEvent)m_tabWaitDONE[0];
            AutoResetEvent evtSTOP = (AutoResetEvent)m_tabWaitDONE[1];
            AutoResetEvent evtBLOCKED = (AutoResetEvent)m_tabWaitDONE[2];
            AutoResetEvent evtACK = (AutoResetEvent)m_waitACK[0];
            AutoResetEvent evtACKSTOP = (AutoResetEvent)m_waitACK[1];

            // Etude de la commande reçue.

            if ((m_msgRecu[0].ArbitrationId != (uint)IDcommande.WStopACK) && (m_msgRecu[0].ArbitrationId != (uint)IDcommande.WStopDONE))
                if (m_msgRecu[0].ArbitrationId == m_id + 1)
                    evtACK.Set();
                else if (m_msgRecu[0].ArbitrationId == m_id + 2)
                    evtDONE.Set();

            switch (m_msgRecu[0].ArbitrationId)
            {
                case (uint)IDcommande.WStopACK:
                    evtACKSTOP.Set();
                    break;
                case (uint)IDcommande.WStopDONE:
                    evtSTOP.Set();
                    break;
                case (uint)IDcommande.WBeBlocked:
                    evtBLOCKED.Set();
                    break;
            }

        }

        void m_busCan_ErrorReceived(ControllerAreaNetwork sender, ControllerAreaNetwork.ErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public idErreur envoyer(uint id, byte[] data = null, int nbData = 0)
        {
            idErreur retour = idErreur.FatalErreurCAN;

            m_id = id;
            // Tableau d'envoie de commande.
            ControllerAreaNetwork.Message[] commande;
            commande = new ControllerAreaNetwork.Message[1];
            commande[0] = new ControllerAreaNetwork.Message();

            //construction de la trame 
            commande[0].ArbitrationId = id;
            commande[0].Length = nbData;
            commande[0].IsExtendedId = false;
            commande[0].IsRemoteTransmissionRequest = false;
            if (nbData != 0)
                for (int i = 0; i < nbData; i++)
                    commande[0].Data[i] = data[i];

            // --- lock de la commande à envoyer
            //System.Threading.Monitor.Enter(mutex);
            m_busCan.SendMessages(commande);
            Debug.Print("CanId Envoye" + commande[0].ArbitrationId.ToString());


            // ### Commande envoyé ###
            returnWaitAny = WaitHandle.WaitAny(m_waitACK, 500, false);
            switch (returnWaitAny)
            {
                case 0:
                    retour = idErreur.OK;
                    break;
                case 1:
                    retour = idErreur.ACKSTOP;
                    break;
                default:
                    retour = idErreur.timoutACK;
                    break;
            }

            // --- unlock de la commande
            //System.Threading.Monitor.Exit(mutex);

            return retour;
        }

        void m_busCan_ErrorReceived(CANDW sender, CANDW.ErrorReceivedEventArgs args)
        {
            //throw new NotImplementedException();
            Debug.Print(">>> can_ErrorReceivedEvent <<<");
            switch (args.Error)
            {
                /*  case CAN.Error.Overrun:
                      Debug.Print("Overrun error. Message lost");
                      break;*/

                case ControllerAreaNetwork.Error.RXOver:
                    Debug.Print("RXOver error. Internal buffer is full. Message lost");
                    break;

                case ControllerAreaNetwork.Error.BusOff:
                    Debug.Print("BusOff error. Reset CAN controller.");
                    sender.Reset();
                    break;
                case ControllerAreaNetwork.Error.ErrorPassive:
                    Debug.Print("Error Passive.");
                    break;

            }
        }

        /*    void m_busCan_MessagesReceived(CANDW sender, CANDW.MessagesReceivedEventArgs e) // bus Can va receptionner le message
            {
                int count = e.MessageCount;

                for(int i=0;i<count;i++)
                    m_msgRecu[i] = e.Messages[i];
      
                Debug.Print("CanId reçu" + m_msgRecu[0].ArbitrationId.ToString());

                AutoResetEvent evtDONE = (AutoResetEvent)m_tabWaitDONE[0];
                AutoResetEvent evtSTOP = (AutoResetEvent)m_tabWaitDONE[1];
                AutoResetEvent evtBLOCKED = (AutoResetEvent)m_tabWaitDONE[2];
                AutoResetEvent evtACK = (AutoResetEvent)m_waitACK[0];
                AutoResetEvent evtACKSTOP = (AutoResetEvent)m_waitACK[1];

                // Etude de la commande reçus.
                for (int i = 0; i < count; i++)
                {
                    if ((m_msgRecu[i].ArbitrationId != (uint)IDcommande.WStopACK) && (m_msgRecu[i].ArbitrationId != (uint)IDcommande.WStopDONE))
                        if (m_msgRecu[i].ArbitrationId == m_id + 1)
                            evtACK.Set();
                        else if (m_msgRecu[i].ArbitrationId == m_id + 2)
                            evtDONE.Set();

                    switch (m_msgRecu[i].ArbitrationId)
                    {
                        case (uint)IDcommande.WStopACK:
                            evtACKSTOP.Set();
                            break;
                        case (uint)IDcommande.WStopDONE:
                            evtSTOP.Set();
                            break;
                        case (uint)IDcommande.WBeBlocked:
                            evtBLOCKED.Set();
                            break;
                    }
                }

            }*/


        public idErreur isDone(int timeOutDONE, byte[] data = null, int nbData = 0)
        {
            idErreur retour = idErreur.OK;
            // Attente DONE - DONE = 0; Stop = 1; Timer
            returnWaitAny = WaitHandle.WaitAny(m_tabWaitDONE, timeOutDONE, false);
            switch (returnWaitAny)
            {
                case 0:
                    retour = idErreur.OK;
                    if (m_msgRecu[0].Length != 0)
                    {
                        for (int i = 0; i < m_msgRecu[0].Length; i++)
                            data[i] = m_msgRecu[0].Data[i];
                        nbData = m_msgRecu[0].Length;
                    }
                    break;
                case 1:
                    retour = idErreur.STOPPED;
                    break;
                case 2:
                    retour = idErreur.BLOCKED;
                    break;
                default:
                    retour = idErreur.timoutDONE;
                    break;
            }

            return retour;
        }

    }
}
