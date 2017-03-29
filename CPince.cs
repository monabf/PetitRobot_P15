using System;
using Microsoft.SPOT;

namespace PR.Membres
{
    class CPince
    {
        CAX12 m_ax12Pince;

        public struct configPince
        {
            public byte idAX12PinceJaune;   //mouvement de rentré et sortie du bras
            public byte idAX12PinceBleue;   //mouvement de rentré et sortie du bras
        };

        enum positionPinceJaune
        {
            rentree = 177,
            intermediaire_rentree = 0,
            sortie = 512,
            intermediaire_sortie = 125 // Valeurs à changer après tests sur servos
        };

        enum positionPinceBleue
        {
            rentree = 177,
            intermediaire_rentree = 0,
            sortie = 512,
            intermediaire_sortie = 125 // Valeurs à changer après tests sur servos
        };



        public CPince(Couleur equipe, ControleurAX12 controleur, configPince config) //le constructeur
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12Pince = new CAX12(config.idAX12PinceJaune, controleur.m_port, controleur.m_direction);
            }
            else
            {
                m_ax12Pince = new CAX12(config.idAX12PinceBleue, controleur.m_port, controleur.m_direction);
            }
            m_ax12Pince.setMode(AX12Mode.joint);
        }


        public void deplie(Couleur equipe)
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12Pince.move((int)positionPinceJaune.sortie);
            }
            else
            {
                m_ax12Pince.move((int)positionPinceBleue.sortie);
            }

        }

        public void replie(Couleur equipe)
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12Pince.move((int)positionPinceJaune.rentree);
            }
            else
            {
                m_ax12Pince.move((int)positionPinceBleue.rentree);
            }
        }

        public void semiReplie(Couleur equipe)
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12Pince.move((int)positionPinceJaune.intermediaire_rentree);
            }
            else
            {
                m_ax12Pince.move((int)positionPinceBleue.intermediaire_rentree);
            }
        }


        public void semiDeplie(Couleur equipe)
        {
            if (equipe == Couleur.Jaune)
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
