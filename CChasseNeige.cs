using System;
using System.IO;
using System.Threading;
using System.IO.Ports;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
//using GHI.Premium.Hardware;
//using GHI.Premium.Hardware.LowLevel;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GHI.Processor;

namespace PetitRobot_V1
{
    struct configChasseNeige
    {
        public byte idAX12BGauche;   //deploie et range le chasse neige
        public byte idAX12BDroit;
    };
    enum positionChasseNeige
    {
        rangerDroite = 1000,
        rangerGauche = 198,
        deployerDroite = 696,
        deployerGauche = 514
    };
    class CChasseNeige
    {
        CAX12 m_ax12BGauche, m_ax12BDroit;

        public CChasseNeige(ControleurAX12 controleur,configChasseNeige config)
        {
            //AX12 rentré/sortie
            m_ax12BGauche = new CAX12(config.idAX12BGauche, controleur.m_port, controleur.m_direction);
            m_ax12BGauche.setMode(AX12Mode.joint);
            m_ax12BGauche.move((int)positionChasseNeige.rangerGauche);

            //AX12 tourner
            m_ax12BDroit = new CAX12(config.idAX12BDroit, controleur.m_port, controleur.m_direction);
            m_ax12BDroit.setMode(AX12Mode.joint);
            m_ax12BDroit.move((int)positionChasseNeige.rangerDroite);
        }


        public void ranger()
        {
            m_ax12BGauche.move((int)positionChasseNeige.rangerGauche);
            m_ax12BDroit.move((int)positionChasseNeige.rangerDroite);
        }

        public void deployer()
        {
            m_ax12BGauche.move((int)positionChasseNeige.deployerGauche);
            m_ax12BDroit.move((int)positionChasseNeige.deployerDroite);
        }




    }

}
