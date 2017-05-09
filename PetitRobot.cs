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
        public positionBaseRoulante posBR;
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
        //IHMSelection m_ihm;
        Jack m_jack;
        GroupeInfrarouge m_IR;


        CCapteurUltrason m_ultrason;

        CBaseRoulante m_baseRoulante;
        positionBaseRoulante m_positionRobot = new positionBaseRoulante();

        ControleurAX12 m_controleurAX12;
        CPetitBras petitBras;
        CPince pince;
        CPoussoir poussoir;
        public static bool obstacle = false;
        
       
        #endregion
        
        #region Constructeur

        public PetitRobot(ConfigurationPorts ports, Couleur equipe)
        {
            m_ports = ports;

            m_baseRoulante = new CBaseRoulante(m_ports.idBaseRoulante);
            m_baseRoulante.setCouleur(Couleur.Bleu);
                       
           // m_ihm = new IHMSelection();

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

            m_ultrason = new CCapteurUltrason(m_ports.idCapteurUltrason);
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

        /// <summary>
        /// Méthode qui va initialiser les paramètres du robot en fonction de la couleur et de la disposition choisie
        /// </summary>
        /**
        public void Initialisation()
        {
            m_ihm.Selection(ref m_etatRobot.couleurEquipe, ref m_etatRobot.disposition);        //Retourne la couleur de l'equipe et la disposition du terrain choisi sur l'IHM
            m_ihm.Afficher("Selection : OK");
            m_ihm.Afficher("Configuration de la table : OK");

            m_baseRoulante.setCouleur((m_etatRobot.couleurEquipe == Couleur.Bleu ? Couleur.Bleu : Couleur.Jaune));       //Envoi la couleur sélectionné pour définir à la base roulante sa position de départ
            
            InitialisationStrategie();              //Initialise la stratégie du robot
           // m_ihm.Afficher("Initialisation de la strategie : OK");
        }
        **/
    

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

                

        etatBR robotGoToXY(ushort x,ushort y, sens s, bool boolDetection = false,int speed=10)
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
                retour = m_baseRoulante.allerEn(y, x, s,speed);
            }
            return retour;
        }

        void recalageX(int angle, int x,sens s, int speed)
        {
            m_baseRoulante.recalagePosX(angle, x,speed,s);
        }
        /*void recalageX(int angle, int x)
        {
            m_baseRoulante.recalagePosX(angle, x);
        }*/
        void recalageY(int angle, int y,int speed, sens s)
        {
            m_baseRoulante.recalagePosY(angle, y,speed,s);
        }

        void getPosition(ref positionBaseRoulante pos)
        {
            m_baseRoulante.getPosition(ref pos);
        }

        void changerXYA(int x, int y, int angle)
        {
            m_baseRoulante.changerXYA(angle, x, y);
        }

        etatBR robotRotate(int alpha)
        {
            etatBR retour;
            retour = m_baseRoulante.tourner(alpha);
            return retour;
        }

        

        public void Detecter(object o)
        {
            sens dir = (sens)o;
            // si on avance, les ultrasons sont utiles

            if (dir == sens.avancer)
            {
                // on teste les capteurs IR avants puis le capteur laser en appui
                double distance = 0d;
                bool obstacleUS = false;
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
        public void Demarrer()
        {
            //m_ihm.Afficher("Debut de la strategie");
            Debug.Print("Demmarage ok");
            Debug.Print(""+GestionStrat.NombreAction);
            while (GestionStrat.ExecutionPossible == true)     //Execution de la boucle tant qu'il y a toujours une action à réaliser 
            {
               // m_ihm.Afficher("Execution de l'action suivante");
                GestionStrat.ExecuterSuivante();
                Debug.Print("Action suivante");
            }

            //m_ihm.Afficher("Fin de la strategie");
            
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
