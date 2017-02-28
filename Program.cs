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

namespace PetitRobot_V1
{
    public partial class Program
    {
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            //Attribution des différents numéros de ports
            ConfigPorts ports = new ConfigPorts();
            ports.numIO= 5;
            ports.pinJack = 8;
            ports.pinAVG = 4;
            ports.pinAVD = 5;
            ports.pinARG = 6;
            ports.pinARD = 7;
            ports.numSocketUltrason = 6;
            ports.numSerialPortBaseRoulante = 8;
            ports.numSerialPortMembres = 11;
            ports.confChasseNeige.idAX12BDroit = 3;
            ports.confChasseNeige.idAX12BGauche = 2;
            ports.confBras.idAX12Coude = 6;
            ports.confBras.idAX12Poignet = 5;

            PetitRobot Robot;
            Robot = new PetitRobot(ports, null);

            Robot.Initialisation();
            Robot.AttendreJack();       //Attends que le jack soit débranché
            Robot.Start();              //Démarrage du robot
            //Thread.Sleep(90000);        
            Robot.Stop();               //Arrêt forcé du robot au bout de 90s

        }
    }
}
