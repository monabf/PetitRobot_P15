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


namespace PetitRobot_V1
{
    #region structures

  
    struct ConfigPorts
    {
        public int numIO;
        public int pinJack;
        public int pinAVG;
        public int pinAVD;
        public int pinARG;
        public int pinARD;
        public int numSocketUltrason;
        public int numSerialPortBaseRoulante;
        public int numSerialPortMembres;
        public configChasseNeige confChasseNeige;
        public configBras confBras;
    }

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
        
        ConfigPorts m_ports;
        Thread m_threadRun;
        EtatRobot m_etatRobot;

        GestionnaireStrategie GestionStrat;
        IHM m_ihm;
        InputPort m_jack;
        InputPort m_AVD, m_AVG, m_ARD, m_ARG;

        DistanceUS3 m_ultrason;

        CBaseRoulante m_baseRoulante;
        positionBaseRoulante m_positionRobot = new positionBaseRoulante();

        ControleurAX12 m_controleurAX12;
        CBras m_bras;
        CChasseNeige m_chasseNeige;
        
        CTableJeu m_tableJeu;
        
        #endregion
        
        #region Constructeur

        /// <summary>
        /// Constructeur : Initialise les différents paramétres
        /// </summary>
        /// <param name="RobotPorts">Ports du robot</param>
        public PetitRobot(ConfigPorts RobotPorts, CTableJeu tableJeu)
        {
            m_ports = RobotPorts;
        
            m_baseRoulante = new CBaseRoulante(RobotPorts.numSerialPortBaseRoulante);

            this.m_tableJeu = tableJeu;     //Relation d'agregation avec la classe CTableJeu
                       
            m_ihm = new IHM();
            GestionStrat = new GestionnaireStrategie();
            m_controleurAX12= new ControleurAX12(RobotPorts.numSerialPortMembres);
            m_chasseNeige = new CChasseNeige(m_controleurAX12, RobotPorts.confChasseNeige);
            m_bras = new CBras(m_controleurAX12, RobotPorts.confBras);
            
            m_jack = new InputPort(GT.Socket.GetSocket(RobotPorts.numIO, true, null, null).CpuPins[RobotPorts.pinJack], false, Port.ResistorMode.PullUp);
            m_AVG = new InputPort(GT.Socket.GetSocket(RobotPorts.numIO, true, null, null).CpuPins[RobotPorts.pinAVG], false, Port.ResistorMode.PullUp);
            m_AVD = new InputPort(GT.Socket.GetSocket(RobotPorts.numIO, true, null, null).CpuPins[RobotPorts.pinAVD], false, Port.ResistorMode.PullUp);
            m_ARG = new InputPort(GT.Socket.GetSocket(RobotPorts.numIO, true, null, null).CpuPins[RobotPorts.pinARG], false, Port.ResistorMode.PullUp);
            m_ARD = new InputPort(GT.Socket.GetSocket(RobotPorts.numIO, true, null, null).CpuPins[RobotPorts.pinARD], false, Port.ResistorMode.PullUp);

            m_ultrason = new DistanceUS3(RobotPorts.numSocketUltrason);

            m_threadRun = new Thread(new ThreadStart(robotStart));    //Création d'un thread
            
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Méthode qui va initialiser les paramétres du robot en fonction de la couleur et de la disposition choisie
        /// </summary>
        public void Initialisation()
        {
            m_ihm.Selection(ref m_etatRobot.couleurEquipe, ref m_etatRobot.disposition);        //Retourne la couleur de l'equipe et la disposition du terrain choisi sur l'IHM
            m_ihm.Afficher("Selection : OK");

            m_tableJeu = new CTableJeu(m_etatRobot.couleurEquipe, m_etatRobot.disposition);     //Initialisation de la position de chaque objet sur la table de jeu en fonction de la couleur de l'equipe et de la disposition choisi sur l'IHM            
            m_ihm.Afficher("Configuration de la table : OK");

            m_baseRoulante.setCouleur((m_etatRobot.couleurEquipe == Couleur.Violet ? Couleur.Violet : Couleur.Vert));       //Envoi la couleur sélectionné pour définir à la base roulante sa position de départ
            
            /*InitialisationStrategie();              //Initialise la stratégie du robot
            m_ihm.Afficher("Initialisation de la strategie : OK");*/
        }

        #endregion

        #region Méthodes de debut/fin et executions des actions

        /// <summary>
        /// Attends que le jack soit retiré
        /// </summary>
        public void AttendreJack()
        {
            m_ihm.Afficher("Attends que le Jack soit debranche...");
            while (!m_jack.Read()) ;  //Execution de la boucle tant que le jack n'est pas débranché
            m_ihm.Afficher("Jack debranche : OK");
        }

        /// <summary>
        /// Démarre le thread pour la méthode robotStart
        /// </summary>
        public void Start()
        {
            double distance=0;
            //m_threadRun.Start();
           /* do
            {
                distance = m_ultrason.GetDistance(5);
            } while (distance > 30 || distance==-1);*/
           hisserLesPavillons();
           decalerChateau();
           /// allerEn(678, 2573, sens.avancer, false);
            //allerEn(678, 73, sens.reculer, false);
          //  m_baseRoulante.tourner(-90);
           // m_chasseNeige.deployer();
            //m_chasseNeige.ranger();
            //m_bras.sortir();
            //m_bras.rentrer();
           ramasserCoquillages();
        }

        public etatBR allerEn(int x, int y, sens s, bool detection)
        {
            etatBR retour;
            if (detection)
            {
                // on passe le sens "dir" au timer via la variable "state"
                Timer t = new Timer(new TimerCallback(detecter), s, 0, 1000);
                retour = m_baseRoulante.allerEn(x, y, s);
                t.Dispose();
            }
            else
            {
                retour = m_baseRoulante.allerEn(x, y, s);
            }
            return retour;
        }

        public void detecter(object o)
        {
            sens dir = (sens)o;
            // si on avance
            if (dir == sens.avancer)
            {
                // on teste les capteurs IR avants puis le capteur laser en appui
                double distance;
                bool obstacle = false;

                distance = m_ultrason.GetDistance(5);
                if (distance < 30 && distance != -1)
                    obstacle = true;
                if ((!m_AVG.Read() || !m_AVD.Read()) && obstacle )
                {                    
                     m_baseRoulante.stop();
                }
            }
            // si on recule
            else
            {
                // on teste les capteurs IR arrières
                if (!m_ARG.Read() || !m_ARD.Read())
                {
                    m_baseRoulante.stop();
                }
            }
        }
        
        /// <summary>
        /// Execution des différentes tâches
        /// </summary>
        public void robotStart()
        {
            m_ihm.Afficher("Debut de la strategie");

            while (GestionStrat.ExecutionPossible == true)     //Execution de la boucle tant qu'il y a toujours une action à réaliser
            {
                m_ihm.Afficher("Execution de l'action suivante");
                GestionStrat.ExecuterSuivante();
            }

            m_ihm.Afficher("Fin de la strategie");
            
        }

        /// <summary>
        /// Stop tout mouvement du robot. Est appelé au bout de 90s.
        /// </summary>
        public void Stop()
        {
            m_threadRun.Abort();
            m_baseRoulante.stop();
            m_ihm.Afficher("Fin");
            //m_ihm.Fermer();
        }

        #endregion

        #region Action

        public etatBR hisserLesPavillons()
        {
            etatBR retour = 0;
            

            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(m_positionRobot.x,m_etatRobot.couleurEquipe == Couleur.Violet ? m_tableJeu.Cabines[0].Position.Y : m_tableJeu.Cabines[3].Position.Y , sens.avancer, false);     //Avance devant la première cabine
           // m_baseRoulante.tourner(-90);
            
            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(70, m_etatRobot.couleurEquipe == Couleur.Violet ? m_tableJeu.Cabines[0].Position.Y : m_tableJeu.Cabines[3].Position.Y, sens.reculer, false);     //Ferme la première cabine
            
            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(m_positionRobot.x + 300, m_positionRobot.y, sens.avancer, false);       //Avance de 30cm
           // m_baseRoulante.tourner(+90);
            
            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(m_positionRobot.x, m_etatRobot.couleurEquipe == Couleur.Violet ? m_tableJeu.Cabines[1].Position.Y : m_tableJeu.Cabines[2].Position.Y, sens.avancer, false);       //Se place en face de la deuxième cabine
           // m_baseRoulante.tourner(-90);

            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(70, m_etatRobot.couleurEquipe == Couleur.Violet ? m_tableJeu.Cabines[1].Position.Y : m_tableJeu.Cabines[2].Position.Y, sens.reculer, false);       //Ferme la deuxième cabine

            return retour;
        }

        public etatBR decalerChateau()
        {
            etatBR retour = 0;

            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(m_etatRobot.couleurEquipe == Couleur.Violet ? (m_positionRobot.x + 200) : (m_positionRobot.x - 200), m_positionRobot.y, sens.avancer, false);
            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(100, m_etatRobot.couleurEquipe == Couleur.Violet ? (m_positionRobot.y - 200) : (m_positionRobot.y + 200), sens.reculer, false);
            m_bras.sortir();
            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(100, m_etatRobot.couleurEquipe == Couleur.Violet ? m_tableJeu.Dunes[1].Position.Y -120 : m_tableJeu.Dunes[3].Position.Y+120, sens.avancer, false);
            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(m_positionRobot.x, m_etatRobot.couleurEquipe == Couleur.Violet ? (m_positionRobot.y - 200) : (m_positionRobot.y + 200), sens.reculer, false);
            m_bras.rentrer();

            return retour;
        }

        public etatBR pousserChateau()
        {
            etatBR retour = 0;

            allerEn(900, m_etatRobot.couleurEquipe == Couleur.Violet ? (m_tableJeu.Dunes[0].Position.Y - 350) : (m_tableJeu.Dunes[4].Position.Y + 350), sens.avancer, false);

            m_chasseNeige.deployer();

            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(m_positionRobot.x, m_etatRobot.couleurEquipe == Couleur.Violet ? (m_positionRobot.y + 650) : (m_positionRobot.y - 650), sens.avancer, false);

            m_chasseNeige.ranger();

            m_baseRoulante.getPosition(ref m_positionRobot);
            allerEn(m_positionRobot.x, m_etatRobot.couleurEquipe == Couleur.Violet ? (m_positionRobot.y - 350) : (m_positionRobot.y + 350), sens.reculer, false);

            return retour;
        }

        public etatBR ramasserCoquillages()
        {
            etatBR retour = 0;

            m_chasseNeige.deployer();

            if (m_etatRobot.disposition == 1 || m_etatRobot.disposition == 2)
            {
                //allerEn(1250, m_etatRobot.couleurEquipe == Couleur.Violet ? 600 : 2400, sens.avancer, true);
            }
            else
            {
                allerEn(1250, m_etatRobot.couleurEquipe == Couleur.Violet ? 700 : 2400, sens.avancer, true);
            }
            

            allerEn(1550, m_etatRobot.couleurEquipe == Couleur.Violet ? m_tableJeu.Coquillages[1].Position.Y : m_tableJeu.Coquillages[12].Position.Y, sens.avancer, true);

            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            allerEn(1250, m_etatRobot.couleurEquipe == Couleur.Violet ? m_tableJeu.Coquillages[0].Position.Y : m_tableJeu.Coquillages[11].Position.Y, sens.avancer, true);

            allerEn(m_tableJeu.Serviette.Position.X, m_tableJeu.Serviette.Position.Y, sens.avancer, true);

            m_chasseNeige.ranger();

            return retour;
        }

        /*public etatBR ramasserCoquillages()
        {
            etatBR retour = 0;

            m_chasseNeige.deployer();

            allerEn(1250, m_etatRobot.couleurEquipe == Couleur.Violet ? 600 : 2400, sens.avancer, true);

            allerEn(1550, m_etatRobot.couleurEquipe == Couleur.Violet ? m_tableJeu.Coquillages[1].Position.Y : m_tableJeu.Coquillages[12].Position.Y, sens.avancer, true);

            allerEn(1250, m_etatRobot.couleurEquipe == Couleur.Violet ? m_tableJeu.Coquillages[0].Position.Y : m_tableJeu.Coquillages[11].Position.Y, sens.avancer, true);

            allerEn(m_tableJeu.Serviette.Position.X, m_tableJeu.Serviette.Position.Y, sens.avancer, true);

            m_chasseNeige.ranger();

            switch (m_etatRobot.disposition)
            { 
                case 1 :

                    break;

                case 2 :

                    break;

                case 3 :

                    break;

                case 4 :

                    break;

                case 5 :

                    break;
            }

            return retour;
        }*/

        #endregion
    }
}
