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
                Plateforme = 10,
                IO = 5,
                Jack = 4,
                InfrarougeAVG = 6,
                InfrarougeAVD = 8,
                InfrarougeARG = 7,
                InfrarougeARD = 9,
                CapteurUltrason = 6,
                ContAX12 = 11,
                DetecteurIR = 12,
            };

            ports.poussoir.idAx12PoussoirBleu = 1;
            ports.poussoir.idAx12PoussoirJaune = 2;

            ports.pince.idAx12PinceBleue = 3;
            ports.pince.idAx12PinceJaune = 4;

            ports.petitBras.idAX12CoudeBleu = 12;
            ports.petitBras.idAx12RotateurBleu = 13;
            ports.petitBras.idCapteurBrasBleu = 14;
            ports.petitBras.idAX12CoudeJaune = 15;
            ports.petitBras.idAx12RotateurJaune = 16;
            ports.petitBras.idCapteurBrasJaune = 17;

            // initialisation de l'IHM de sélection
            IHMSelection selection;
            selection = new IHMSelection();
            // ceci est une fonction et c'est normal !
            selection.Validation += SelectionEffectuee;

            // affiche l'IHM de sélection et attende de la validation
            selection.Afficher();
            while (!SelectionValidee) Thread.Sleep(1);
            selection.Fermer();
          
            PetitRobot robot;
            robot = new PetitRobot(ports, selection.Equipe);

            // attente du jack
            robot.AttendreJack();
            // démarre le robot
            robot.Demarrer(90d);
        }

        private void SelectionEffectuee(object sender, EventArgs e)
        {
            var selection = sender as IHMSelection;

            SelectionValidee = selection.Equipe != Couleur.Null && selection.Disposition > 0;
        }
    }
}
