using System.Threading;
using Microsoft.SPOT;

namespace PR.Membres
{
  class CRouletteIntelligente
  {

        CAX12 m_ax12Roulette;
        CCapteurCouleur m_capteurCouleur;

        public CRouletteIntelligente(CCapteurCouleur capteurCouleur, CAX12 ax12Roulette)
        {
            m_capteurCouleur = capteurCouleur;
            m_ax12Roulette = ax12Roulette;
            m_ax12Roulette.setMode(AX12Mode.wheel);
        }

        public CAX12 getRoulette(){
            return m_ax12Roulette;
        }

        public void Tourner() {
            m_ax12Roulette.setMovingSpeed(speed.forward);
            while (m_capteurCouleur.ContinuerRotation())
            {
                Thread.Sleep(100);
            }
            m_ax12Roulette.setMovingSpeed(speed.stop);
        }
      /*
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
       * */
    }
}
