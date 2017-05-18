using System;
using Microsoft.SPOT;

namespace PR.Membres
{

    public class CPoussoir
    {

        Couleur couleurEquipe;
        CAX12 m_ax12Poussoir;

        public struct configPoussoir
        {
            public byte idAX12PoussoirBleu;
            public byte idAX12PoussoirJaune;
        };

        enum positionPoussoirBleu
        {
            rentree = 825,
            intermediaireRentré=572,
            intermediaire = 500,//512
            sortie = 224
            // Valeurs a changer apres tests sur servos
        };

        enum positionPoussoirJaune
        {
            rentree = 206,
            intermediaireRentré=485,//422
            intermediaire = 512,
            sortie = 834
            // Valeurs a changer apres tests sur servos
        };

        public Couleur getCouleur()
        {
            return couleurEquipe;
        }


        public CPoussoir(Couleur equipe, ControleurAX12 controleur, configPoussoir config) //le constructeur
        {
            couleurEquipe = equipe;
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Poussoir = new CAX12(config.idAX12PoussoirJaune, controleur.m_port, controleur.m_direction);
                m_ax12Poussoir.setMode(AX12Mode.joint);
                //                m_ax12Poussoir.setMovingSpeed();
            }
            else
            {
                m_ax12Poussoir = new CAX12(config.idAX12PoussoirBleu, controleur.m_port, controleur.m_direction);
                m_ax12Poussoir.setMode(AX12Mode.joint);
            }
            m_ax12Poussoir.setMovingSpeed(150);
            Debug.Print("CPoussoir opérationnel");

        }


        public void milieu()//deplie
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Poussoir.move((int)positionPoussoirJaune.intermediaire);
            }

            else
            {
                m_ax12Poussoir.move((int)positionPoussoirBleu.intermediaire);
            }
        }

        public void semiReplie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Poussoir.move((int)positionPoussoirJaune.intermediaireRentré);
            }

            else
            {
                m_ax12Poussoir.move((int)positionPoussoirBleu.intermediaireRentré);
            }
        }


        public void replie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Poussoir.move((int)positionPoussoirJaune.rentree);
            }

            else
            {
                m_ax12Poussoir.move((int)positionPoussoirBleu.rentree);
            }
        }

        public void deplie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Poussoir.move((int)positionPoussoirJaune.sortie);
            }

            else
            {
                m_ax12Poussoir.move((int)positionPoussoirBleu.sortie);
            }
        }
    }
}
