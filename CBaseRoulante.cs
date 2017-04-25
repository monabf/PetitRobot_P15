using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using GHI.Pins;
using System.IO.Ports;
using Microsoft.SPOT.Hardware;


using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;


namespace PR.BR3
{
   // enum Couleur { none = 0, Bleu, Jaune };

    struct etatRobot
    {
        public positionBaseRoulante posBR;
        public Couleur couleurEquipe;
        public int coefPos;
        public int coefAngle;
        public bool stop;
        public sens sens;
    }
    
    enum sens : byte { avancer = (byte)'A', reculer = (byte)'R', none = 0 };
    
    enum signeAngle : byte { plus = (byte)'+', moins = (byte)'-' };

    struct positionBaseRoulante
    {
        public ushort posx;
        public ushort posy;
        public short alpha;
    };

    class CBaseRoulante
    {
        private CCanAppli m_gestionCan;
        private int m_timeOut;
        const double m_vitesse = 0.3;        //0,3m/s

        public CBaseRoulante(CCanAppli gestionCan)// récupération en paramètre de la configuration du port can
        {
            m_gestionCan = gestionCan;
        }

        private void calculerTimeOut(positionBaseRoulante posDestination) // définition du timeout de la connexion can
        {

            /*     double distance = 0;
                positionBaseRoulante posActuelle= new positionBaseRoulante();
            
               if(getPosition(ref posActuelle)==idErreur.OK)
                {
                    double X=MathEX.Pow(posDestination.posx-posActuelle.posx,2);
                    double Y=MathEX.Pow(posDestination.posy-posActuelle.posy,2);
                    distance = MathEX.Sqrt(X + Y);
                    m_timeOut = (int)(distance/m_vitesse);  
                }
                */
            m_timeOut = 20000;
        }

        public idErreur stop() // arrêter toute tache en cours
        {
            idErreur retour = m_gestionCan.envoyer((uint)IDcommande.WStop);
            if (retour == idErreur.ACKSTOP)
            {
                retour = m_gestionCan.isDone(2000);
            }
            return retour;

        }

        public idErreur goToXY(ushort positionX, ushort positionY, short alpha, signeAngle signe, sens dir) // aller jusqu'a la position et se positionner dans un angle précis
        {
            byte[] Data = new byte[8];
            idErreur retour = 0;

            Data[0] = (byte)positionX;
            Data[1] = (byte)(positionX >> 8);
            Data[2] = (byte)positionY;
            Data[3] = (byte)(positionY >> 8);
            Data[4] = (byte)dir;
            Data[5] = (byte)signe;
            Data[6] = (byte)alpha;
            Data[7] = (byte)(alpha >> 8); ;

            positionBaseRoulante posDestination = new positionBaseRoulante();
            posDestination.posx = positionX;
            posDestination.posy = positionY;
            calculerTimeOut(posDestination);

            retour = m_gestionCan.envoyer((uint)IDcommande.WGoTo, Data, 8); //check de l'ack
            if (retour == idErreur.OK)
                retour = m_gestionCan.isDone(m_timeOut);
            return retour;
        }

        public idErreur goToXY(ushort positionX, ushort positionY, sens dir)
        {
            byte[] Data = new byte[8];
            idErreur retour = 0;

            Data[0] = (byte)positionX;
            Data[1] = (byte)(positionX >> 8);
            Data[2] = (byte)positionY;
            Data[3] = (byte)(positionY >> 8);
            Data[4] = (byte)dir;
            positionBaseRoulante posDestination = new positionBaseRoulante();
            posDestination.posx = positionX;
            posDestination.posy = positionY;
            calculerTimeOut(posDestination);

            retour = m_gestionCan.envoyer((uint)IDcommande.WGoTo2, Data, 5);
            if (retour == idErreur.OK)
                retour = m_gestionCan.isDone(m_timeOut);
            return retour;
        }

        // Fixe la vitesse de déplacement du robot, setSpeed = 0x208
        public idErreur setSpeed(byte speed) // configurer la vitesse des moteurs de la base roulante
        {
            idErreur retour = idErreur.timoutDONE;
            byte[] Data = new byte[8];
            Data[0] = speed;

            retour = m_gestionCan.envoyer((uint)IDcommande.WSetSpeed, Data, 1);
            if (retour == idErreur.OK)
                retour = m_gestionCan.isDone(2000);

            return retour;
        }

        // Méthode permettant de faire tourner le robot d'un angle alpha à une vitesse donnée, rotate = 0x212
        public idErreur rotate(byte speed, signeAngle signe, short alpha)
        {
            idErreur retour;
            byte[] Data = new byte[8];

            //Data[0] = speed;
            Data[0] = (byte)signe;
            Data[1] = (byte)alpha;
            Data[2] = (byte)(alpha >> 8);

            retour = m_gestionCan.envoyer((uint)IDcommande.WRotate, Data, 3);
            if (retour == idErreur.OK)
            {
                retour = m_gestionCan.isDone(10000);
            }
            return retour;
        }


        //Méthode pour définir la position de départ, startPosition = 0x206, byte color {'R' = 0x52 = Rouge, 'B' = 0x42 = Bleu}
        public idErreur setColor(byte color)
        {
            idErreur retour = idErreur.timoutDONE;
            byte[] Data = new byte[8];

            //Création du message
            Data[0] = color;

            retour = m_gestionCan.envoyer((uint)IDcommande.WSetColor, Data, 1);
            if (retour == idErreur.OK)
                retour = m_gestionCan.isDone(2000);

            return retour;
        }

        public idErreur getPosition(ref positionBaseRoulante posBR)// récupère la position théorique retenue par la carte widdar
        {
            idErreur retour;
            int nbDataRecu = 0;
            byte[] dataRecu = new byte[8];

            retour = m_gestionCan.envoyer((uint)IDcommande.WGetPosition);
            if (retour == idErreur.OK)
            {
                retour = m_gestionCan.isDone(2000, dataRecu, nbDataRecu);
                if (retour == idErreur.OK)
                {
                    posBR.posx = (ushort)((dataRecu[1] << 8) + dataRecu[0]);
                    posBR.posy = (ushort)((dataRecu[3] << 8) + dataRecu[2]);
                    if (dataRecu[4] == '-')
                        posBR.alpha = (short)(6280 - ((dataRecu[6] << 8) + dataRecu[5]));
                    else
                        posBR.alpha = (short)((dataRecu[6] << 8) + dataRecu[5]);
                }
            }
            return retour;
        }

        public idErreur getCodeur(ref int codeur1, ref int codeur2)//récupère la position dans l'unité des codeurs
        {

            idErreur retour;
            int nbDataRecu = 0;
            byte[] dataRecu = new byte[8];

            retour = m_gestionCan.envoyer((uint)IDcommande.WGetCodeur);
            if (retour == idErreur.OK)
            {
                retour = m_gestionCan.isDone(2000, dataRecu, nbDataRecu);
                if (retour == idErreur.OK)
                {
                    codeur1 = (int)((dataRecu[1] << 8) + dataRecu[0]);
                    codeur2= (int)((dataRecu[4] << 8) + dataRecu[3]);
                }
            }
            return retour;
        }


    }
}
