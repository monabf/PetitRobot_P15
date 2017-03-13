using System;
using Microsoft.SPOT;

namespace RobotEve
{
    class PetitBrasJaune
    {
    struct configPetitBrasJaune
    {
        public byte idAX12PetitBrasJaune;   //mouvement de rentrée et sortie du bras
     };

    enum positionPetitBrasJaune
    {
        rentree=177,
        intermediaire_rentree= 0,
        sortie=512,
        intermediaire_sortie= 125 // Valeurs à changer après tests sur servos

    };

  
        CAX12 m_ax12PetitBrasJaune;

        public PetitBrasJaune(ControleurAX12 controleur,configPetitBrasJaune config) //le constructeur
        {
           
            m_ax12PetitBrasJaune = new CAX12(config.idAX12PetitBrasJaune, controleur.m_port, controleur.m_direction);
            m_ax12PetitBrasJaune.setMode(AX12Mode.joint);

         }


        public void deplie()
        {
            m_ax12PetitBrasJaune.move((int)positionPetitBrasJaune.sortie);
            
        }

        public void replie()
        {
            m_ax12PetitBrasJaune.move((int)positionPetitBrasJaune.rentree);
           
        }

        public void semiReplie()
        {
            m_ax12PetitBrasJaune.move((int)positionPetitBrasJaune.intermediaire_rentree);
        }


        public void semiDeplie()
        {
            m_ax12PetitBrasJaune.move((int)positionPetitBrasJaune.intermediaire_sortie);
        }

    
    }
}
