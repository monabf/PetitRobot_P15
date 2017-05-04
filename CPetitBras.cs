using System;
using Microsoft.SPOT;

namespace PR.Membres
{
    public class CPetitBras
    {

        CAX12 m_ax12Coude;
        CRouletteIntelligente m_rouletteIntelligente;
        Couleur couleurEquipe;

        public struct configPetitBras
        {
            public byte idAX12CoudeBleu;   //mouvement de rentré et sortie du bras
            public byte idAX12RotateurBleu;
            public byte idCapteurBrasBleu;
            public byte idAX12CoudeJaune;   //mouvement de rentré et sortie du bras
            public byte idAX12RotateurJaune;
            public byte idCapteurBrasJaune;
        };

        enum positionPetitBrasBleu
        {
            rentree = 800,
            intermediaire_rentree = 512,
            sortie = 350,
            intermediaire_sortie = 512, // Valeurs à changer après tests sur servos
            intialisiation_roulette = 90
        };

        enum positionPetitBrasJaune
        {
            rentree = 821,
            intermediaire_rentree = 650,
            sortie = 206,
            intermediaire_sortie = 512, // Valeurs à changer après tests sur servos
            intialisiation_roulette = 90
        };


        public Couleur getCouleur()
        {
            return couleurEquipe;
        }

        public CPetitBras(Couleur equipe, ControleurAX12 controleur, configPetitBras config) //le constructeur
        {
            couleurEquipe = equipe;
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Coude = new CAX12(config.idAX12CoudeJaune, controleur.m_port, controleur.m_direction);
                m_ax12Coude.setMode(AX12Mode.joint);
                // corriger le deuxième paramètre du contructeur CCapteurCouleur ci-dessous
                CAX12 ax12RotateurJaune = new CAX12(config.idAX12RotateurJaune, controleur.m_port, controleur.m_direction);
                CCapteurCouleur capteurCouleurJaune = new CCapteurCouleur(config.idCapteurBrasJaune, equipe, false);
                m_rouletteIntelligente = new CRouletteIntelligente(capteurCouleurJaune, ax12RotateurJaune);
            }
            else
            {
                m_ax12Coude = new CAX12(config.idAX12CoudeBleu, controleur.m_port, controleur.m_direction);
                m_ax12Coude.setMode(AX12Mode.joint);
                // corriger le deuxième paramètre du contructeur CCapteurCouleur ci-dessous
                CAX12 ax12RotateurBleu = new CAX12(config.idAX12RotateurBleu, controleur.m_port, controleur.m_direction);
                Debug.Print("Rotateur opérationnel");
                Debug.Print("" + config.idCapteurBrasBleu);
                CCapteurCouleur capteurCouleurBleu = new CCapteurCouleur(config.idCapteurBrasBleu, equipe, false);
                Debug.Print("Capteur couleur opérationnel");
                m_rouletteIntelligente = new CRouletteIntelligente(capteurCouleurBleu, ax12RotateurBleu);
            }

            Debug.Print("CPetitBras opérationnel");

        }


        public void deplie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Coude.move((int)positionPetitBrasJaune.intermediaire_sortie);
            }
            else
            {
                m_ax12Coude.move((int)positionPetitBrasBleu.intermediaire_sortie);
            }
        }

        public void replie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Coude.move((int)positionPetitBrasJaune.rentree);
            }
            else
            {
                m_ax12Coude.move((int)positionPetitBrasBleu.rentree);
            }

        }

        public void semiReplie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Coude.move((int)positionPetitBrasJaune.intermediaire_rentree);
            }
            else
            {
                m_ax12Coude.move((int)positionPetitBrasBleu.intermediaire_rentree);
            }
        }


        public void semiDeplie()
        {
            if (couleurEquipe == Couleur.Jaune)
            {
                m_ax12Coude.move((int)positionPetitBrasJaune.intermediaire_sortie);
            }
            else
            {
                m_ax12Coude.move((int)positionPetitBrasBleu.intermediaire_sortie);
            }
        }

        public void initialiserRoue()
        {
            m_rouletteIntelligente.getRoulette().setMode(AX12Mode.joint);
            if (couleurEquipe == Couleur.Jaune)
            {
                m_rouletteIntelligente.getRoulette().move((int)positionPetitBrasJaune.intermediaire_sortie);
            }
            else
            {
                m_rouletteIntelligente.getRoulette().move((int)positionPetitBrasBleu.intermediaire_sortie);
            }
            // la ligne ci-dessous a été commentée pour les tests. La décommenter après
            // m_rouletteIntelligente.getRoulette().setMode(AX12Mode.wheel);
        }

        public void tourner()
        {
            // il faut cette fois-ci que la couleur soit près du capteur
            m_rouletteIntelligente.Tourner();
        }


    }
}