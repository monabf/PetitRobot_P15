using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace PR
{
    public partial class Program
    {

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            //Attribution des différents numéros de ports
            // numéros de ports à corriger



            var ports = new ConfigurationPorts
            {
                idBaseRoulante = 8, // pin Kangaroo
                idIO = 5,  // le groupe IO (extendeur) est relié au pin 5 de la spider. le groupe IR & le jack sont branchés sur l'extendeur IO
                idJack = 8, // pin 8 de l'extendeur
                idInfrarougeAVG = 4, // id infra-rouge de l'extendeur. AVG = avant-gauche vu de l'arrière du robot
                idInfrarougeAVD = 5, // idem
                idInfrarougeARG = 6, // idem
                idInfrarougeARD = 7, // idem
                idCapteurUltrason = 6, // pin 6 de la spider
                idContAX12 = 11, // pin AX12
                // idDetecteurIR = 12, // je ne comprends pas ce port. Il doit s'agit d'une erreur. PE
            };

            ports.poussoir.idAX12PoussoirBleu = 2;
            ports.poussoir.idAX12PoussoirJaune = 2;

            ports.pince.idAX12PinceBleue = 1;
            ports.pince.idAX12PinceJaune = 1;

            ports.petitBras.idAX12CoudeBleu = 3;
            ports.petitBras.idAX12RotateurBleu = 4; // ATTENTION : il faudra mettre le numéro 4 avec le logiciel !
            ports.petitBras.idAX12CoudeJaune = 3;
            ports.petitBras.idAX12RotateurJaune = 4;

            // ces ports là sont des ports de la spyder/spider
            ports.petitBras.idCapteurBrasBleu = 4;
            ports.petitBras.idCapteurBrasJaune = 9; // ce port 4 n'est pas adapté, il faut en trouver un autre !!

            // initialisation de l'IHM de sélection
            /*
            IHMSelection selection;
            selection = new IHMSelection();
            Debug.Print("test1");
            // affiche l'IHM de sélection et attend que la couleur et la disposition aient été choisis
            selection.Afficher("Renseigner configuration");
            Debug.Print("test2");
            while (selection.getEquipe() != Couleur.Null || selection.getDisposition() == 0)
            {
                Thread.Sleep(1);
            }
            selection.Fermer();
            Debug.Print("test3");
             
            PetitRobot robot;
            robot = new PetitRobot(ports, selection.getEquipe());
             * */
            PetitRobot robot;
            robot = new PetitRobot(ports, Couleur.Bleu);            

            // attente du jack
            Debug.Print("Godot viendra-t-il ?");
            robot.AttendreJack();
            // démarre le robot
            Debug.Print("jack");
            
            //robot.petitBras.tourner();
            robot.Demarrer(90d);
            
            
            Debug.Print("fin");
            // l'action suivante n'est-elle pas prise en charge dans PetitRobot ?
            //robot.Demarrer();
            //Debug.Print("robot démarré");
        }

    }
}
