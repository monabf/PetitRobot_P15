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
            // la ligne ci-dessous a �t� comment�e pour les tests. La d�commenter apr�s
            //m_ax12Roulette.setMode(AX12Mode.wheel);
            Debug.Print("roulette op�rationnelle");
        }

        public CAX12 getRoulette(){
            return m_ax12Roulette;
        }

        public void Tourner() {
            // les lignes ci-dessous a �t� comment�es pour les tests. La d�commenter apr�s
            /*
            m_ax12Roulette.setMovingSpeed(speed.forward);
            while (m_capteurCouleur.ContinuerRotation())
            {
                Thread.Sleep(100);
            }
            m_ax12Roulette.setMovingSpeed(speed.stop);
             * */
        }
      /*
        public void MettreBonneCouleurLoinDuCapteur(Couleur equipe)
        {
            // "oppos�" de la fonction pr�c�dente !
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
