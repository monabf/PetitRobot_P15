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
        IHMSelection m_ihm;
        Jack m_jack;
        GroupeInfrarouge m_IR;


        CCapteurUltrason m_ultrason;

        CBaseRoulante m_baseRoulante;
        positionBaseRoulante m_positionRobot = new positionBaseRoulante();

        ControleurAX12 m_controleurAX12;
        CPetitBras petitBras;
        CPince pince;
        CPoussoir poussoir;
        
       
        #endregion
        
        #region Constructeur

        /// <summary>
        /// Constructeur : Initialise les différents paramétres
        /// </summary>
        /// <param name="RobotPorts">Ports du robot</param>
        public PetitRobot(ConfigurationPorts ports, Couleur equipe)
        {
            m_ports = ports;

            //Font font; // WARNING: font is not defined yet
            m_baseRoulante = new CBaseRoulante(m_ports.idBaseRoulante);
                       
            m_ihm = new IHMSelection();
            GestionStrat = new GestionnaireStrategie();
            m_controleurAX12 = new ControleurAX12(m_ports.idContAX12);

            //NB: pince = 1 AX12, petitBras = 2 AX12 et 1 CapteurCouleur, poussoir = 1 AX12
            pince = new CPince(equipe, m_controleurAX12, m_ports.pince);
            petitBras = new CPetitBras(equipe, m_controleurAX12, m_ports.petitBras);
            poussoir = new CPoussoir(equipe, m_controleurAX12, m_ports.poussoir);

            
            // idIO = idPortDeLaSpider, idJack = idPinSurLExtendeur
            m_jack = new Jack(m_ports.idIO, m_ports.idJack);
            m_IR = new GroupeInfrarouge(m_ports.idIO, m_ports.idInfrarougeAVD, m_ports.idInfrarougeAVG, m_ports.idInfrarougeARD, m_ports.idInfrarougeARG);

            m_ultrason = new CCapteurUltrason(m_ports.idCapteurUltrason);

            // et c'est parti pour la boucle !
            m_threadRun = new Thread(new ThreadStart(Demarrer));    //Création d'un thread
            
        }

        #endregion

        #region Initialisation

        /// <summary>
        /// Méthode qui va initialiser les paramètres du robot en fonction de la couleur et de la disposition choisie
        /// </summary>
        public void Initialisation()
        {
            m_ihm.Selection(ref m_etatRobot.couleurEquipe, ref m_etatRobot.disposition);        //Retourne la couleur de l'equipe et la disposition du terrain choisi sur l'IHM
            m_ihm.Afficher("Selection : OK");
            m_ihm.Afficher("Configuration de la table : OK");

            m_baseRoulante.setCouleur((m_etatRobot.couleurEquipe == Couleur.Bleu ? Couleur.Bleu : Couleur.Jaune));       //Envoi la couleur sélectionné pour définir à la base roulante sa position de départ
            
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
            while (!m_jack.Etat) Thread.Sleep(1);
        }

        /// <summary>
        /// Démarre le thread pour la méthode robotStart
        /// </summary>
        
        /*
        public void Start()
        {
            double distance=0;
            //m_threadRun.Start();
            do
            {
                distance = m_ultrason.GetDistance(5);
            } while (distance > 30 || distance==-1);
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
         * */


        // cette fonction allerEn utilise detecter !
        etatBR robotGoToXY(ushort x, ushort y, sens s, bool boolDetection = false)
        {
            etatBR retour;
            if (boolDetection)
            {
                // on passe le sens "dir" au timer via la variable "state"
                // analogue au timeout-callback pour les amoureux du js
                Timer t = new Timer(new TimerCallback(Detecter), s, 0, 1000);
                retour = m_baseRoulante.allerEn(x, y, s);
                t.Dispose();
            }
            else
            {
                retour = m_baseRoulante.allerEn(x, y, s);
            }
            return retour;
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
                bool obstacle = false;
                // mesure une distance moyenne avec 5 mesures rapides
                m_ultrason.getDistance(5, ref distance);
                if (distance < 30 && distance != -1)
                    obstacle = true;
               if ((!m_IR.AVG.Read() || !m_IR.AVD.Read()) && obstacle )
                    {                    
                         m_baseRoulante.stop();
                    }
                }
            // si on recule, les ultrasons ne sont plus utiles
            else
            {
                // on teste les capteurs IR arrières
                if (!m_IR.ARG.Read() || !m_IR.ARD.Read())
                {
                    m_baseRoulante.stop();
                }
            }
        }
        
        /// <summary>
        /// Execution des différentes tâches
        /// </summary>
        public void Demarrer()
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
        /*

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
