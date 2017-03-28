using System;
using Microsoft.SPOT;

namespace RobotEve
{
    class PetitBras
    {

        Couleur equipe;
        CAX12 m_ax12PetitBras;


        struct configPetitBras
        {
            public byte idAX12PetitBrasBleu;   //mouvement de rentré et sortie du bras
            public byte idAX12PetitBrasJaune;   //mouvement de rentré et sortie du bras
        };

        enum positionPetitBrasBleu
        {
            rentree = 177,
            intermediaire_rentree = 0,
            sortie = 512,
            intermediaire_sortie = 125 // Valeurs à changer après tests sur servos

        };

        enum positionPetitBrasJaune
         {
        rentree=177,
        intermediaire_rentree= 0,
        sortie=512,
        intermediaire_sortie= 125 // Valeurs à changer après tests sur servos

         };



        public PetitBras(Couleur equipe, ControleurAX12 controleur, configPetitBras config) //le constructeur
        {
            // ce n'est pas superbe, mais c'est efficace !
            if (equipe == Couleur.Jaune)
            {
                m_ax12PetitBras = new CAX12(config.idAX12PetitBrasJaune, controleur.m_port, controleur.m_direction);
            }
            else
            {
                m_ax12PetitBras = new CAX12(config.idAX12PetitBrasBleu, controleur.m_port, controleur.m_direction);
            }
            m_ax12PetitBras.setMode(AX12Mode.joint);

        }


        public void deplie()
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12PetitBras.move((int)positionPetitBrasJaune.intermediaire_sortie);
            }
            else
            {
                m_ax12PetitBras.move((int)positionPetitBrasBleu.intermediaire_sortie);
            }
        }

        public void replie()
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12PetitBras.move((int)positionPetitBrasJaune.rentree);
            }
            else
            {
                m_ax12PetitBras.move((int)positionPetitBrasBleu.rentree);
            }

        }

        public void semiReplie()
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12PetitBras.move((int)positionPetitBrasJaune.intermediaire_rentree);
            }
            else
            {
                m_ax12PetitBras.move((int)positionPetitBrasBleu.intermediaire_rentree);
            }
        }


        public void semiDeplie()
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12PetitBras.move((int)positionPetitBrasJaune.intermediaire_sortie);
            }
            else
            {
                m_ax12PetitBras.move((int)positionPetitBrasBleu.intermediaire_sortie);
            }
        }


    }
}
