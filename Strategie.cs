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
#if HOMOLOGATION
           positionBaseRoulante positionRobot = new positionBaseRoulante();
           m_baseRoulante.setSpeed(50);

           /*ATTENTION : on a deux sets de 3 pinces, un pour chaque couleur, donc il faut quand on initialise les composants créer
           deux instances de la classe Pince, PinceJaune et PinceBleue....*/

           GestionStrat.Ajouter(new ActionRobot(() => //Initialisation de l'homologation
           {
             m_ihm.Afficher("Homologation");
             //on va vers la fusée, on récupère un cylindre et on le met dans la rigole et c'est tout !

             //on va vers la fusée :
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

             //on fait le créneau
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

             //on récupère le cylindre
             if (m_etatRobot.couleurEquipe == Couleur.Bleu)
             {
               //En même temps!!!! ????? :
               PinceBleue.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens horaire)
               PetitBrasBleue.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

               PoussoirBleu.deplie(); //À CODER : doit tourner de la même façon
               RouletteBleue.deplie(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras

               //En même temps!!!! ???? :
               PinceBleue.semiReplie(); //À CODER : doit fermer la pince à moitié (45° dans le sens horaire)
               RouletteBleue.semiReplie();

               m_baseRoulante.goToXY((ushort)1287, (ushort)197, sens.avancer);
               PinceBleue.semiDeplie(); //À CODER : doit rouvrir la pince fermée à moitié (45° dans le sens antihoraire)
               PetitBrasBleu.semiDeplie();
             }
             else
             {
               //En même temps!!!! ????? :
               PinceJaune.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens ANTIhoraire car on est de l'autre côté du robot puisqu'on est jaunes cette fois!!!)
               PetitBrasJaune.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

               PoussoirJaune.deplie(); //À CODER : doit tourner de la même façon
               RouletteJaune.deplie(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras

               //En même temps!!!! ???? :
               PinceJaune.semiReplie();//À CODER : doit fermer la pince à moitié (45° dans le sens horaire)
               RouletteJaune.semiReplie();

               m_baseRoulante.goToXY((ushort)1287, (ushort)2803, sens.avancer);
               PinceJaune.semiDeplie();//À CODER : doit rouvrir la pince fermée à moitié (45° dans le sens antihoraire)
               PetitBrasJaune.semiDeplie();
             }

             //on tourne et on dépose le cylindre
             m_baseRoulante.setSpeed(35);

             if (m_etatRobot.couleurEquipe == Couleur.Bleu)
             {
               //WHILE COULEUR LU PAR LE CAPTEUR != BLEU : ROULETTE.ROTATE(+,20) ?????
               //À CODER!!!!!!!!!!!!!

               m_baseRoulante.goToXY((ushort)1200, (ushort)197, sens.avancer);
               PinceBleue.replie();
               PetitBrasBleu.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
               m_baseRoulante.goToXY((ushort)1150, (ushort)197, sens.avancer);
               m_baseRoulante.goToXY((ushort)1180, (ushort)197, sens.reculer);

               PinceBleue.deplie();//on ressort la pince
               m_baseRoulante.goToXY((ushort)824-i*100, (ushort)197, sens.avancer);

               PinceBleue.replie();
               PoussoirBleu.replie();
               m_baseRoulante.goToXY((ushort)1387, (ushort)197, sens.avancer);
             }
             else
             {
               //WHILE COULEUR LU PAR LE CAPTEUR != JAUNE : ROULETTE.ROTATE(+,20) ?????
               //À CODER!!!!!!!!!!!!!

               m_baseRoulante.goToXY((ushort)1200, (ushort)2803, sens.avancer);
               PinceJaune.replie();
               PetitBrasJaune.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
               m_baseRoulante.goToXY((ushort)1150, (ushort)2803, sens.avancer);
               m_baseRoulante.goToXY((ushort)1180, (ushort)2803, sens.reculer);

               Pince.deplie();//on ressort la pince
               m_baseRoulante.goToXY((ushort)824-i*100, (ushort)2803, sens.avancer);

               Pince.replie();
               Poussoir.replie();
               m_baseRoulante.goToXY((ushort)1387, (ushort)2803, sens.avancer);
             }

           }, calculPriorite: () => 100, executionUnique: true));

#else
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
                  PinceBleue.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens horaire)
                  PetitBrasBleue.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

                  PoussoirBleu.deplie(); //À CODER : doit tourner de la même façon
                  RouletteBleue.deplie(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras

                  //En même temps!!!! ???? :
                  PinceBleue.semiReplie(); //À CODER : doit fermer la pince à moitié (45° dans le sens horaire)
                  RouletteBleue.semiReplie();

                  m_baseRoulante.goToXY((ushort)1287, (ushort)197, sens.avancer);
                  PinceBleue.semiDeplie(); //À CODER : doit rouvrir la pince fermée à moitié (45° dans le sens antihoraire)
                  PetitBrasBleu.semiDeplie();
                }
                else
                {
                  //En même temps!!!! ????? :
                  PinceJaune.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens ANTIhoraire car on est de l'autre côté du robot puisqu'on est jaunes cette fois!!!)
                  PetitBrasJaune.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

                  PoussoirJaune.deplie(); //À CODER : doit tourner de la même façon
                  RouletteJaune.deplie(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras

                  //En même temps!!!! ???? :
                  PinceJaune.semiReplie();//À CODER : doit fermer la pince à moitié (45° dans le sens horaire)
                  RouletteJaune.semiReplie();

                  m_baseRoulante.goToXY((ushort)1287, (ushort)2803, sens.avancer);
                  PinceJaune.semiDeplie();//À CODER : doit rouvrir la pince fermée à moitié (45° dans le sens antihoraire)
                  PetitBrasJaune.semiDeplie();
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
                  PinceBleue.replie();
                  PetitBrasBleu.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
                  m_baseRoulante.goToXY((ushort)1150, (ushort)197, sens.avancer);
                  m_baseRoulante.goToXY((ushort)1180, (ushort)197, sens.reculer);

                  PinceBleue.deplie();//on ressort la pince
                  m_baseRoulante.goToXY((ushort)824-i*100, (ushort)197, sens.avancer);

                  PinceBleue.replie();
                  PoussoirBleu.replie();
                  m_baseRoulante.goToXY((ushort)1387, (ushort)197, sens.avancer);
                }
                else
                {
                  //WHILE COULEUR LU PAR LE CAPTEUR != JAUNE : ROULETTE.ROTATE(+,20) ?????
                  //À CODER!!!!!!!!!!!!!

                  m_baseRoulante.goToXY((ushort)1200, (ushort)2803, sens.avancer);
                  PinceJaune.replie();
                  PetitBrasJaune.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
                  m_baseRoulante.goToXY((ushort)1150, (ushort)2803, sens.avancer);
                  m_baseRoulante.goToXY((ushort)1180, (ushort)2803, sens.reculer);

                  PinceJaune.deplie();//on ressort la pince
                  m_baseRoulante.goToXY((ushort)824-i*100, (ushort)2803, sens.avancer);

                  PinceJaune.replie();
                  PoussoirJaune.replie();
                  m_baseRoulante.goToXY((ushort)1387, (ushort)2803, sens.avancer);
                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise
                return true;
            }, calculPriorite: () => 97-i, executionUnique: false));

          }


        }
    }
}
