using System;
using Microsoft.SPOT;

namespace RobotEve
{
    class PinceJaune
    {
    struct configPinceJaune
    {
        public byte idAX12PinceJaune;   //mouvement de rentré et sortie du bras
     };

    enum positionPinceJaune
    {
        rentree=177,
        intermediaire_rentree= 0,
        sortie=512,
        intermediaire_sortie= 125 // Valeurs à changer après tests sur servos

    };

   
        CAX12 m_ax12PinceJaune;

        public PinceJaune(ControleurAX12 controleur,configBras config) //le constructeur
        {
           
            m_ax12PinceJaune = new CAX12(config.idAX12PinceJaune, controleur.m_port, controleur.m_direction);
            m_ax12PinceJaune.setMode(AX12Mode.joint);

         }


        public void deplie()
        {
            m_ax12PinceJaune.move((int)positionPinceJaune.sortie);
            
        }

        public void replie()
        {
            m_ax12PinceJaune.move((int)positionPinceJaune.rentree);
           
        }

        public void semiReplie()
        {
            m_ax12PinceJaune.move((int)positionPinceJaune.intermediaire_rentree);
        }


        public void semiDeplie()
        {
            m_ax12PinceJaune.move((int)positionPinceJaune.intermediaire_sortie);
        }

    
    }
}
