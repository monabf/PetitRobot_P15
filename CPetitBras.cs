using System;
using Microsoft.SPOT;

namespace PR.Membres
{
    class CPetitBras
    {

        Couleur equipe;
        CAX12 m_ax12Coude;
        CRouletteIntelligente m_rouletteIntelligente;


        struct configPetitBras
        {
            public byte idAX12CoudeBleu;   //mouvement de rentr� et sortie du bras
            public byte idAx12RotateurBleu;
            public byte idCapteurBrasBleu;
            public byte idAX12CoudeJaune;   //mouvement de rentr� et sortie du bras
            public byte idAx12RotateurJaune;
            public byte idCapteurBrasJaune;
        };

        enum positionPetitBrasBleu
        {
            rentree = 177,
            intermediaire_rentree = 0,
            sortie = 512,
            intermediaire_sortie = 125, // Valeurs � changer apr�s tests sur servos
            intialisiation_roulette = 90
        };

        enum positionPetitBrasJaune
        {
            rentree = 177,
            intermediaire_rentree = 0,
            sortie = 512,
            intermediaire_sortie = 125, // Valeurs � changer apr�s tests sur servos
            intialisiation_roulette = 90
        };



        public CPetitBras(Couleur equipe, ControleurAX12 controleur, configPetitBras config) //le constructeur
        {
            // ce n'est pas superbe, mais c'est efficace !
            if (equipe == Couleur.Jaune)
            {
                m_ax12Coude = new CAX12(config.idAX12PetitBrasJaune, controleur.m_port, controleur.m_direction);
                m_ax12Coude.setMode(CAX_12.AX12Mode.joint);
                CCapteurCouleur capteurCouleurJaune = new CCapteurCouleur(config.idCapteurBrasJaune);
                CAX_12 ax12RotateurJaune = new CAX_12(config.idAx12RotateurJaune, controleur.m_port, controleur.m_direction);
                m_rouletteIntelligente = CRouletteIntelligente(capteurCouleurJaune, ax12RotateurJaune);
                m_ax12Poussoir = new CAX_12(config.idAx12Poussoir, controleur.m_port, controleur.m_direction);
            }
            else
            {
                m_ax12Coude = new CAX12(config.idAX12PetitBrasBleu, controleur.m_port, controleur.m_direction);
                m_ax12Coude.setMode(CAX_12.AX12Mode.joint);
                CCapteurCouleur capteurCouleurBleu = new CCapteurCouleur(config.idCapteurBrasBleu);
                CAX_12 ax12RotateurBleu = new CAX_12(config.idAx12Rotateur, controleur.m_port, controleur.m_direction);
                m_rouletteIntelligente = CRouletteIntelligente(capteurCouleurBleu, ax12RotateurBleu);
                m_ax12Poussoir = new CAX_12(config.idAx12Poussoir, controleur.m_port, controleur.m_direction);
            }
        }


        public void deplie(Couleur equipe)
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12Coude.move((int)positionPetitBrasJaune.intermediaire_sortie);
            }
            else
            {
                m_ax12Coude.move((int)positionPetitBrasBleu.intermediaire_sortie);
            }
        }

        public void replie(Couleur equipe)
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12Coude.move((int)positionPetitBrasJaune.rentree);
            }
            else
            {
                m_ax12Coude.move((int)positionPetitBrasBleu.rentree);
            }

        }

        public void semiReplie(Couleur equipe)
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12Coude.move((int)positionPetitBrasJaune.intermediaire_rentree);
            }
            else
            {
                m_ax12Coude.move((int)positionPetitBrasBleu.intermediaire_rentree);
            }
        }


        public void semiDeplie(Couleur equipe)
        {
            if (equipe == Couleur.Jaune)
            {
                m_ax12Coude.move((int)positionPetitBrasJaune.intermediaire_sortie);
            }
            else
            {
                m_ax12Coude.move((int)positionPetitBrasBleu.intermediaire_sortie);
            }
        }

        public void initialiserRoue(Couleur equipe)
        {
            if (equipe == Couleur.Jaune)
            {
                m_rouletteIntelligente.m_ax12roulette.move((int)positionPetitBrasJaune.intermediaire_sortie);
            }
            else
            {
                m_rouletteIntelligente.m_ax12roulette.move((int)positionPetitBrasBleu.intermediaire_sortie);
            }
        }

        public void tourner(Couleur equipe)
        {
            // il faut cette fois-ci que la couleur de 
            m_rouletteIntelligente.MettreBonneCouleurPresDuCapteur(equipe);
        }


    }
}