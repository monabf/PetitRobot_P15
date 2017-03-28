using System;
using Microsoft.SPOT;

namespace PR.Membres
{

    class CPoussoir
    {

      Couleur couleurEquipe;
      CAX12 m_ax12Poussoir;


        struct configPoussoir
        {
            public byte idAX12PoussoirBleu;
            public byte idAX12PoussoirJaune;
         };

        enum positionPoussoirBleu
        {
            rentree=20,
            sortie=20,
           // Valeurs a changer apres tests sur servos
        };

        enum positionPoussoirJaune
        {
            rentree=20,
            sortie=20,
           // Valeurs a changer apres tests sur servos
        };


            public CPoussoir(Couleur equipe, ControleurAX12 controleur,configPoussoir config) //le constructeur
            {

              if (equipe == Couleur.Jaune)
              {
                m_ax12Poussoir = new CAX12(config.idAX12PoussoirJaune, controleur.m_port, controleur.m_direction);
              }
              else
              {
                m_ax12Poussoir = new CAX12(config.idAX12PoussoirBleu, controleur.m_port, controleur.m_direction);
              }
               m_ax12Poussoir.setMode(AX12Mode.joint);
            }


            public void rotate()
            {
              if (equipe == Couleur.Jaune)
              {
                  m_ax12Poussoir.move((int)positionPoussoirJaune.sortie);
              }

              else
              {
                  m_ax12Poussoir.move((int)positionPoussoirBleu.sortie);
              }
            }

            public void replie()
            {
              if (equipe == Couleur.Jaune){
                m_ax12Poussoir.move((int)positionPoussoirJaune.rentree);
              }

              else {
                  m_ax12Poussoir.move((int)positionPoussoirBleu.rentree);
              }
            }


    }
}
