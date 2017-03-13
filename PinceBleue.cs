using System;
using Microsoft.SPOT;

namespace RobotEve
{
    class PinceBleue
    {
    struct configPinceBleue
    {
        public byte idAX12PinceBleue;   //mouvement de rentré et sortie du bras
     };

    enum positionPinceBleue
    {
        rentree=177,
        intermediaire_rentree= 0,
        sortie=512,
        intermediaire_sortie= 125 // Valeurs à changer après tests sur servos

    };

   
        CAX12 m_ax12PinceBleue;

        public PinceBleue(ControleurAX12 controleur,configBras config) //le constructeur
        {
           
            m_ax12PinceBleue = new CAX12(config.idAX12PinceBleue, controleur.m_port, controleur.m_direction);
            m_ax12PinceBleue.setMode(AX12Mode.joint);

         }


        public void deplie()
        {
            m_ax12PinceBleue.move((int)positionPinceBleue.sortie);
            
        }

        public void replie()
        {
            m_ax12PinceBleue.move((int)positionPinceBleue.rentree);
           
        }

        public void semiReplie()
        {
            m_ax12PinceBleue.move((int)positionPinceBleue.intermediaire_rentree);
        }


        public void semiDeplie()
        {
            m_ax12PinceBleue.move((int)positionPinceBleue.intermediaire_sortie);
        }

    
    }
}
