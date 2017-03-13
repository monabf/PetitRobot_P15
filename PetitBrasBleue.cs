using System;
using Microsoft.SPOT;

namespace RobotEve
{
    class PetitBrasBleue
    {
    struct configPetitBrasBleue
    {
        public byte idAX12PetitBrasBleue;   //mouvement de rentré et sortie du bras
     };

    enum positionPetitBrasBleue
    {
        rentree=177,
        intermediaire_rentree= 0,
        sortie=512,
        intermediaire_sortie= 125 // Valeurs à changer après tests sur servos

    };

    
        CAX12 m_ax12PetitBrasBleue;

        public PetitBrasBleue(ControleurAX12 controleur,configPetitBrasBleue config) //le constructeur
        {
           
            m_ax12PetitBrasBleue = new CAX12(config.idAX12PetitBrasBleue, controleur.m_port, controleur.m_direction);
            m_ax12PetitBrasBleue.setMode(AX12Mode.joint);

            // m_ax12PetitBrasBleue.setMode(CAX_12.AX12Mode.joint);
            //position initiale
            // m_ax12PetitBrasBleue.setMode(CAX_12.AX12Mode.joint);

         }


        public void deplie()
        {
            m_ax12PetitBrasBleue.move((int)positionPetitBrasBleue.sortie);
            
        }

        public void replie()
        {
            m_ax12PetitBrasBleue.move((int)positionPetitBrasBleue.rentree);
           
        }

        public void semiReplie()
        {
            m_ax12PetitBrasBleue.move((int)positionPetitBrasBleue.intermediaire_rentree);
        }


        public void semiDeplie()
        {
            m_ax12PetitBrasBleue.move((int)positionPetitBrasBleue.intermediaire_sortie);
        }

    
    }
}
