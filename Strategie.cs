using System;
using Microsoft.SPOT;
using System.Threading;
using System.Collections;

namespace PetitRobot_V1
{
    partial class PetitRobot
    {
        /// <summary>
        /// Méthode permettant l'initialisation de la stratégie du robot
        /// </summary>
        public void InitialisationStrategie()
        {
            positionBaseRoulante positionRobot = new positionBaseRoulante();
            m_baseRoulante.setSpeed(50);

            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 1ère action : aller vers la fusée
            {
                m_ihm.Afficher("Se deplacer vers la fusee");

                if (m_etatRobot.couleurEquipe == Couleur.Bleu)
                {
                    m_baseRoulante.goToXY((ushort)244, (ushort)788, sens.avancer);
                    m_baseRoulante.rotate(+,90);
                    m_baseRoulante.goToXY((ushort)147, (ushort)872, sens.avancer);
                    m_baseRoulante.goToXY((ushort)715, (ushort)872, sens.avancer);
                    m_baseRoulante.rotate(-,90);
                    m_baseRoulante.goToXY((ushort)800, (ushort)130, sens.avancer);
                    m_baseRoulante.goToXY((ushort)800, (ushort)282, sens.avancer);
                }
                else
                {
                  m_baseRoulante.goToXY((ushort)244, (ushort)2212, sens.avancer);
                  m_baseRoulante.rotate(+,90);
                  m_baseRoulante.goToXY((ushort)157, (ushort)2128, sens.avancer);
                  m_baseRoulante.goToXY((ushort)147, (ushort)873, sens.avancer);
                  m_baseRoulante.rotate(-,90);
                  m_baseRoulante.goToXY((ushort)800, (ushort)2870, sens.avancer);
                  m_baseRoulante.goToXY((ushort)800, (ushort)2718, sens.avancer);
                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise
                return true;
            }, calculPriorite: () => 100, executionUnique: true));




            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 2ème action : Créneau pour être face à la fusée
            {
                m_ihm.Afficher("Creneau");

                m_baseRoulante.setSpeed(20);

                if (m_etatRobot.couleurEquipe == Couleur.Bleu)
                {
                  m_baseRoulante.rotate(-,90);
                  m_baseRoulante.goToXY((ushort)1387, (ushort)197, sens.avancer);
                }
                else
                {
                  m_baseRoulante.rotate(+,90);
                  m_baseRoulante.goToXY((ushort)1387, (ushort)2803, sens.avancer);
                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise

                return true;
            }, calculPriorite: () => 99, executionUnique: true));




            for(int i=0;i<4;i++){

            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 3ème action : Récupérer le cylindre
            {
                m_ihm.Afficher("Recupere le cylindre");

                if (m_etatRobot.couleurEquipe == Couleur.Bleu)
                {
                  //En même temps!!!! ????? :
                  Pince.rotate(+,90); //À CODER : doit tourner dans le sens antihoraire de l'angle indiqué en degrés, même fonctionnement que baseroulante.rotate
                  PetitBras.rotate(-,95); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

                  Poussoir.rotate(-,90); //À CODER : doit tourner de la même façon
                  Roulette.rotate(+,90); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du eptit bras

                  //En même temps!!!! ???? :
                  Pince.rotate(-,45);
                  Roulette.rotate(-,45);

                  m_baseRoulante.goToXY((ushort)1287, (ushort)197, sens.avancer);
                  Pince.rotate(+,45);
                  PetitBras.rotate(+,45);
                }
                else
                {
                  //En même temps!!!! ????? :
                  Pince.rotate(-,90); //À CODER : doit tourner dans le sens antihoraire de l'angle indiqué en degrés, même fonctionnement que baseroulante.rotate
                  PetitBras.rotate(+,95); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

                  Poussoir.rotate(+,90); //À CODER : doit tourner de la même façon
                  Roulette.rotate(-,90); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du eptit bras

                  //En même temps!!!! ???? :
                  Pince.rotate(+,45);
                  Roulette.rotate(+,45);

                  m_baseRoulante.goToXY((ushort)1287, (ushort)2803, sens.avancer);
                  Pince.rotate(-,45);
                  PetitBras.rotate(-,45);
                }
                return true;
            }, calculPriorite: () => 98-i, executionUnique: false));




            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 4ème action : RFaire tourner le cylindre pour mettre la même couleur sur le dessus et dépôt du cylindre dans la rainure
            {
                m_ihm.Afficher("Rotation et depot du cylindre");

                m_baseRoulante.setSpeed(35);

                if (m_etatRobot.couleurEquipe == Couleur.Bleu)
                {
                  //WHILE COULEUR LU PAR LE CAPTEUR != BLEU : ROULETTE.ROTATE(+,20) ?????
                  //À CODER!!!!!!!!!!!!!

                  m_baseRoulante.goToXY((ushort)1200, (ushort)197, sens.avancer);
                  Pince.rotate(+,90);
                  PetitBras.rotate(-,90);//on range pince et petit bras et on va jusqu'à la gouttière
                  m_baseRoulante.goToXY((ushort)1150, (ushort)197, sens.avancer);
                  m_baseRoulante.goToXY((ushort)1180, (ushort)197, sens.reculer);

                  Pince.rotate(-,90);//on ressort la pince
                  m_baseRoulante.goToXY((ushort)824-i*100, (ushort)197, sens.avancer);

                  Pince.rotate(+,90);
                  Poussoir.rotate(+,90);
                  m_baseRoulante.goToXY((ushort)1387, (ushort)197, sens.avancer);
                }
                else
                {
                  //WHILE COULEUR LU PAR LE CAPTEUR != JAUNE : ROULETTE.ROTATE(+,20) ?????
                  //À CODER!!!!!!!!!!!!!

                  m_baseRoulante.goToXY((ushort)1200, (ushort)2803, sens.avancer);
                  Pince.rotate(-,90);
                  PetitBras.rotate(+,90);//on range pince et petit bras et on va jusqu'à la gouttière
                  m_baseRoulante.goToXY((ushort)1150, (ushort)2803, sens.avancer);
                  m_baseRoulante.goToXY((ushort)1180, (ushort)2803, sens.reculer);

                  Pince.rotate(+,90);//on ressort la pince
                  m_baseRoulante.goToXY((ushort)824-i*100, (ushort)2803, sens.avancer);

                  Pince.rotate(-,90);
                  Poussoir.rotate(-,90);
                  m_baseRoulante.goToXY((ushort)1387, (ushort)2803, sens.avancer);
                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise
                return true;
            }, calculPriorite: () => 97-i, executionUnique: false));

          }


        }
    }
}
