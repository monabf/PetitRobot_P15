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
    public partial class PetitRobot
    {

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            //Attribution des différents numéros de ports
            // numéros de ports à corriger



            var ports = new ConfigurationPorts
            {
                idBaseRoulante = 10,
                idIO = 5,
                idJack = 4,
                idInfrarougeAVG = 6,
                idInfrarougeAVD = 8,
                idInfrarougeARG = 7,
                idInfrarougeARD = 9,
                idCapteurUltrason = 6,
                idContAX12 = 11,
                idDetecteurIR = 12,
            };

            ports.poussoir.idAX12PoussoirBleu = 1;
            ports.poussoir.idAX12PoussoirJaune = 2;

            ports.pince.idAX12PinceBleue = 3;
            ports.pince.idAX12PinceJaune = 4;

            ports.petitBras.idAX12CoudeBleu = 12;
            ports.petitBras.idAX12RotateurBleu = 13;
            ports.petitBras.idCapteurBrasBleu = 14;
            ports.petitBras.idAX12CoudeJaune = 15;
            ports.petitBras.idAX12RotateurJaune = 16;
            ports.petitBras.idCapteurBrasJaune = 17;

            // initialisation de l'IHM de sélection
            IHMSelection selection;
            selection = new IHMSelection();
            // affiche l'IHM de sélection et attend que la couleur et la disposition aient été choisis
            selection.Afficher("Renseigner configuration");
            while (selection.getEquipe() != Couleur.Null || selection.getDisposition() == 0)
            {
                Thread.Sleep(1);
            }
            selection.Fermer();
          
            PetitRobot robot;
            robot = new PetitRobot(ports, selection.getEquipe());

            // attente du jack
            robot.AttendreJack();
            // démarre le robot
            robot.Demarrer();
        }

    }
}
