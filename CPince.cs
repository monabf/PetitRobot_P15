using System;
using Microsoft.SPOT;

namespace PR.Membres
{
    public class CPince
    {
        CAX12 m_ax12Pince;
        Couleur couleurEquipe;

        public struct configPince
        {
            public byte idAX12PinceJaune;   //mouvement de rentr� et sortie du bras
            public byte idAX12PinceBleue;   //mouvement de rentr� et sortie du bras
        };

        enum positionPinceJaune
        {
            rentree = 804,// vendredi 19 mai 821
            intermediaire_rentree = 584, // vendredi 19 mai 584
            sortie = 174, // vendredi 19 mai 206
            intermediaire = 512, // vendredi 19 mai 512
            intermediaire_sortie = 695, // vendredi 19 mai 670
        };

        enum positionPinceBleue
        {
            rentree = 200,
            intermediaire_rentree = 420,//512 452
            sortie = 830,
            intermediaire = 492,//512
            intermediaire_sortie = 675// Valeurs � changer apr�s tests sur servos
        };


        public Couleur getCouleur()
        {
            return couleurEquipe;
        }

        public CPince(Couleur equipe, ControleurAX12 controleur, configPince config) //le constructeur
        {
            couleurEquipe = equipe;
            if (equipe == Couleur.Jaune)
            {
                m_ax12Pince = new CAX12(config.idAX12PinceJaune, controleur.m_port, controleur.m_direction);
                m_ax12Pince.setMode(AX12Mode.joint);
            }
            else
            {
                m_ax12Pince = new CAX12(config.idAX12PinceBleue, controleur.m_port, controleur.m_direction);
                m_ax12Pince.setMode(AX12Mode.joint);
            }
            m_ax12Pince.setMovingSpeed(150);
            Debug.Print("CPince op�rationnel");
        }


        public void deplie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Pince.move((int)positionPinceJaune.sortie);
            }
            else
            {
                m_ax12Pince.move((int)positionPinceBleue.sortie);
            }

        }

        public void replie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Pince.move((int)positionPinceJaune.rentree);
            }
            else
            {
                m_ax12Pince.move((int)positionPinceBleue.rentree);
            }
        }

        public void semiReplie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Pince.move((int)positionPinceJaune.intermediaire_rentree);
            }
            else
            {
                m_ax12Pince.move((int)positionPinceBleue.intermediaire_rentree);
            }
        }


        public void milieu()//semiDeplie
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Pince.move((int)positionPinceJaune.intermediaire);
            }
            else
            {
                m_ax12Pince.move((int)positionPinceBleue.intermediaire);
            }
        }

        public void semiDeplie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Pince.move((int)positionPinceJaune.intermediaire_sortie);
            }
            else
            {
                m_ax12Pince.move((int)positionPinceBleue.intermediaire_sortie);
            }
        }


    }
}