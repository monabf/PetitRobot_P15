using System;
using Microsoft.SPOT;

namespace PetitRobot
{
  class CRoulette
  {

    struct configRoulette
    {
      public byte idAx12Roulette;
      public byte idCapteurCouleur;
    }

    CAX_12 m_ax12Roulette;
    CCapteurCouleur m_capteurCouleur;

    public CRoulette(ControleurAX12 controleur, configReservoir config)
    {
      m_capteurCouleur = new CCapteurCouleur(config.idCapteurCouleur);
      m_ax12Roulette = new CAX_12(config.idAx12Roulette, controleur.m_port, controleur.m_direction);
      m_ax12Roulette.setMode(CAX_12.AX12Mode.wheel);
    }

    public void mettreBonneCouleur(Couleur equipe) {
      m_ax12Roulette.setMovingSpeed(speed.forward);
      while (m_capteurCouleur.continuerRotation())
      {
        // Mettre un Thread.sleep?
        Thread.sleep(100);
      }
      m_ax12Roulette.setMovingSpeed(speed.stop);
    }
  }
}
