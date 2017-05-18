using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Hardware;
using GHI.Pins;
using System.IO;
using System.IO.Ports;
using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

using PR.BR2;
using PR.Vision;
using PR.Membres;


namespace PR
{
    #region structures

  
    struct EtatRobot
    {
        public Couleur couleurEquipe;
        public int disposition;
        public int coefPos;
        public int coefAngle;
        public bool stop;
        public sens s;
    }

    #endregion


    partial class PetitRobot
    {
        #region Attributs

        // convention : contrairement aux autres attributs, les membres ne sont précédés par m_

        ConfigurationPorts m_ports;
        Thread m_threadRun;
        EtatRobot m_etatRobot;

        GestionnaireStrategie GestionStrat;
        IHM m_ihm;
        Jack m_jack;
        GroupeInfrarouge m_IR;
        DateTime InstantDebut;

   //     CCapteurUltrason m_ultrason;

        CBaseRoulante m_baseRoulante;

        ControleurAX12 m_controleurAX12;
        CPetitBras petitBras;
        CPince pince;
        CPoussoir poussoir;
        public static bool obstacle = false;
        
       
        #endregion
        
        #region Constructeur

        public PetitRobot(ConfigurationPorts ports, Couleur equipe, int disposition, IHM ihm)
        {
            m_ports = ports;

            m_baseRoulante = new CBaseRoulante(m_ports.idBaseRoulante);
            m_baseRoulante.setCouleur(equipe);
            m_etatRobot.couleurEquipe = equipe;
            m_etatRobot.disposition = disposition;
            m_ihm = ihm;
                       
            GestionStrat = new GestionnaireStrategie();
            InitialisationStrategie();

            m_controleurAX12 = new ControleurAX12(m_ports.idContAX12);

            Debug.Print("Cont AX12 opérationnel");
            //NB: pince = 1 AX12, petitBras = 2 AX12 et 1 CapteurCouleur, poussoir = 1 AX12
            pince = new CPince(equipe, m_controleurAX12, m_ports.pince);
            poussoir = new CPoussoir(equipe, m_controleurAX12, m_ports.poussoir);
            petitBras = new CPetitBras(equipe, m_controleurAX12, m_ports.petitBras);
            Debug.Print("Members opérationnels");

            
            // idIO = idPortDeLaSpider, idJack = idPinSurLExtendeur
            m_jack = new Jack(m_ports.idIO, m_ports.idJack);
            m_IR = new GroupeInfrarouge(m_ports.idIO, m_ports.idInfrarougeAVD, m_ports.idInfrarougeAVG, m_ports.idInfrarougeARD, m_ports.idInfrarougeARG);

           // m_ultrason = new CCapteurUltrason(m_ports.idCapteurUltrason);
            Debug.Print("Détection opérationnels");

            // et c'est parti pour la boucle !
            // est-ce vraiment utile ?
            /*
            m_threadRun = new Thread(new ThreadStart(Demarrer));    //Création d'un thread
            m_threadRun.Start();*/
            Debug.Print("Thread opérationnels");
             

            
        }

        #endregion

        #region Initialisation

        #endregion

        #region Méthodes de debut/fin et executions des actions

        /// <summary>
        /// Attends que le jack soit retiré
        /// </summary>
        public void AttendreJack()
        {
            //m_ihm.Afficher("Attends que le Jack soit debranche...");
            while (!m_jack.Etat) Thread.Sleep(1);
            /*
            m_threadRun = new Thread(new ThreadStart(Demarrer));    //Création d'un thread
            m_threadRun.Start();*/
        }

        public Couleur robotGetCouleur()
        {
            return m_etatRobot.couleurEquipe;
        }

        public int robotGetDisposition()
        {
            return m_etatRobot.disposition;
        }  

        etatBR robotGoToXY(ushort x,ushort y, sens s, bool boolDetection = false,int speed=10, int speedAngle=300)
        {
            etatBR retour;
            if (boolDetection)
            {
                // on passe le sens "dir" au timer via la variable "state"
                // analogue au timeout-callback pour les amoureux du js
                //Timer t = new Timer(new TimerCallback(Detecter), s, 0, 1000);
                obstacle = false; // paramètre pour savoir si il y'a bien un obstacle
                var thDetection = new Thread(() =>
                {
                    while (true)
                    {
                        Detecter(s);
                        //Thread.Sleep(20);
                    }
                });
                thDetection.Start();
                retour = m_baseRoulante.allerDect(y, x, s,speed);// x,y,s
                thDetection.Suspend();
                obstacle = false;
                //t.Dispose();
            }
            else
            {
                retour = m_baseRoulante.allerEn(y, x, s, speed, speedAngle);
            }
            return retour;
        }

        void recalageX(int angle, int x, sens s, int speed, int temps)
        {
            m_baseRoulante.recalagePosX(angle, x, speed, s, temps);
        }

        void recalageY(int angle, int y, sens s, int speed, int temps)
        {
            m_baseRoulante.recalagePosY(angle, y, speed, s, temps);
        }

        void getPosition(ref positionBaseRoulante pos)
        {
            m_baseRoulante.getPosition(ref pos);
        }

        void changerXYA(int x, int y, int angle)
        {
            m_baseRoulante.changerXYA(angle, x, y);
        }

        etatBR robotRotate(int alpha, int speedAngle=300)
        {
            etatBR retour;
            retour = m_baseRoulante.tourner(alpha, speedAngle);
            return retour;
        }

        

        public void Detecter(object o)
        {
            sens dir = (sens)o;
            // si on avance, les ultrasons sont utiles

            if (dir == sens.avancer)
            {
                // on teste les capteurs IR avants puis le capteur laser en appui
                /*
                double distance = 0d;
                bool obstacleUS = false;*/
                // mesure une distance moyenne avec 5 mesures rapides
                //Ultrason désactivé pour l'instant, ils prennent beaucoup trop de temps pour acquérir l'information.
                /*
                m_ultrason.getDistance(1, ref distance);
                if (distance < 30 && distance != -1)
                    obstacleUS = true;
                */

                if ((!m_IR.AVG.Read() || !m_IR.AVD.Read()))// infrarouge OK.. et c'est une condition et && obstacleUS
                {
                    //m_baseRoulante.stop();
                    obstacle = true;

                    
                }
                else obstacle = false;
                
                }
            // si on recule, les ultrasons ne sont plus utiles
            else
            {
                // on teste les capteurs IR arrières
                if (!m_IR.ARG.Read() || !m_IR.ARD.Read())
                {
                    //Debug.Print("Détection obstacle après");
                    //m_baseRoulante.stop();
                    obstacle = true;


                }
                else obstacle = false;
                
            }
            
        }
        
        /// <summary>
        /// Execution des différentes tâches
        /// </summary>
        public void Demarrer(double tempsImparti)
        {


            Timer timeout;
            DateTime fin = new DateTime();
            var thDecompte = new Thread(() =>
            {
                while (GestionStrat.ExecutionPossible && DateTime.Now < fin)
                {
                    //Trac.Ecrire("Temps restant: " + (fin - DateTime.Now).ToString().Substring(3, 5) + ".");
                    Thread.Sleep(10000);
                }
            });
            var thStrat = new Thread(() => EffectuerStrategie());

            InstantDebut = DateTime.Now;
            fin = InstantDebut.AddSeconds(tempsImparti);

            //InitialisationStrategie();

            thDecompte.Start();
            thStrat.Start();

            timeout = new Timer(state =>
            {
                //m_ihm.Ecrire("Fin du temps imparti.");
                if (thStrat.IsAlive) thStrat.Abort();
                m_baseRoulante.stop();
                petitBras.arretUrgenceRoulette();

            }, null, (int)(tempsImparti * 1000), -1);
        }

        private void EffectuerStrategie()
        {
            //m_ihm.Ecrire("Debut de l'execution de la strategie.");

            while (GestionStrat.ExecutionPossible)
            {
                //m_ihm.Ecrire("Execution de l'action suivante.");
                GestionStrat.ExecuterSuivante();
            }

            //m_ihm.Ecrire("Fin de l'execution de la strategie.");
           
        }

        

        /// <summary>
        /// Stop tout mouvement du robot. Est appelé au bout de 90s.
        /// </summary>
        public void Stop()
        {
            m_threadRun.Abort();
            m_baseRoulante.stop();
            //m_ihm.Afficher("Fin");
            //m_ihm.Fermer();
        }

        #endregion

    
    }

    
}
