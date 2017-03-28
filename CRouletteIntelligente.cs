using System;
using Microsoft.SPOT;

namespace PR.Membres
{
  class CRouletteIntelligente
  {

    CAX_12 m_ax12Roulette;
    CCapteurCouleur m_capteurCouleur;

    public CRouletteIntelligente(CCapteurCouleur capteurCouleur, CAX_12 ax12Roulette)
        {
            m_capteurCouleur = capteurCouleur;
            m_ax12Roulette = ax12Roulette;
            m_ax12Roulette.setMode(CAX_12.AX12Mode.wheel);
        }

        public void MettreBonneCouleurPresDuCapteur(Couleur equipe) {
            m_ax12Roulette.setMovingSpeed(speed.forward);
            while (m_capteurCouleur.ContinuerRotation(equipe))
            {
                Thread.sleep(100);
            }
            m_ax12Roulette.setMovingSpeed(speed.stop);
        }

        public void MettreBonneCouleurLoinDuCapteur(Couleur equipe)
        {
            // "opposé" de la fonction précédente !
            if (equipe == Couleur.Jaune)
            {
                MettreBonneCouleurPresDuCapteur(Couleur.Bleu);
            }
            else
            {
                MettreBonneCouleurPresDuCapteur(Couleur.Jaune);
            }
        }
    }
}
