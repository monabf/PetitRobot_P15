using System;
using Microsoft.SPOT;

namespace PetitRobot_V1
{
    struct configBras
    {
        public byte idAX12Coude;   //mouvement de rentré et sortie du bras
        public byte idAX12Poignet;    //mouvement de rotation du bras
    };
    enum positionBras
    {
        rentreCoude=177,
        rentrePoignet=648,
        sortiCoude=512,
        sortiPoignet=512
    };

    class CBras
    {
        CAX12 m_ax12Coude, m_ax12Poignet;

        public CBras(ControleurAX12 controleur,configBras config)
        {
            //AX12 rentré/sortie
            m_ax12Coude = new CAX12(config.idAX12Coude, controleur.m_port, controleur.m_direction);
            m_ax12Coude.setMode(AX12Mode.joint);
            
            //AX12 tourner
            m_ax12Poignet = new CAX12(config.idAX12Poignet, controleur.m_port, controleur.m_direction);
            m_ax12Poignet.setMode(AX12Mode.joint);
            rentrer();
        }


        public void rentrer()
        {
            m_ax12Coude.move(300);
            m_ax12Poignet.move((int)positionBras.rentrePoignet);
            m_ax12Coude.move((int)positionBras.rentreCoude);
            
        }

        public void sortir()
        {
            m_ax12Coude.move(300);
            m_ax12Poignet.move((int)positionBras.sortiPoignet);
            m_ax12Coude.move((int)positionBras.sortiCoude); 
        }

        public void tourner(int angle)
        {
            m_ax12Poignet.move(angle);
        }


    
    }
}
