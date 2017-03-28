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
           deux instances de la classe Pince, pinceJaune et pinceBleue....*/

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
               pinceBleue.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens horaire)
               petitBrasBleue.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!! Cette fois c'est le sens antihoraire

               poussoirBleu.deplie(); //À CODER : doit tourner de la même façon
               rouletteBleue.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras, dans le sens antihoraire

               //En même temps!!!! ???? :
               pinceBleue.semiDeplie(); //À CODER : doit continuer à ouvrir la pince à moitié (45° dans le sens horaire)
               petitBrasBleu.semiReplie(); //À CODER : doit fermer la pince à moitié (45° dans le sens horaire)

               m_baseRoulante.goToXY((ushort)1287, (ushort)197, sens.avancer);
               pinceBleu.semiReplie(); //À CODER : doit refermer la pince fermée à moitié (45° dans le sens antihoraire)
               petitBrasBleu.semiDeplie(); //À CODER : doit rouvrir le petit bras à moitié (45° dans le sens antihoraire)
             }
             else
             {
               //En même temps!!!! ????? :
               pinceJaune.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens ANTIhoraire car on est de l'autre côté du robot puisqu'on est jaunes cette fois!!!)
               petitBrasJaune.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!! Cette fois dans le sens horaire

               poussoirJaune.deplie(); //À CODER : doit tourner de la même façon
               rouletteJaune.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras

               //En même temps!!!! ???? :
               pinceJaune.semiDeplie();//À CODER : doit ouvrir la pince à moitié (45° dans le sens antihoraire)
               petitBrasJaune.semiReplie();//À CODER : 45° dans le sens antihoraire

               m_baseRoulante.goToXY((ushort)1287, (ushort)2803, sens.avancer);
               pinceJaune.semiReplie();//À CODER : doit refermer la pince fermée à moitié (45° dans le sens horaire)
               petitBrasJaune.semiDeplie(); //À CODER : doit rouvrir le petit bras fermé à moitié (45° dans le sens horaire)
             }

             //on tourne et on dépose le cylindre
             m_baseRoulante.setSpeed(35);

             if (m_etatRobot.couleurEquipe == Couleur.Bleu)
             {
               petitBrasBleu.tourner();

               m_baseRoulante.goToXY((ushort)1200, (ushort)197, sens.avancer);
               pinceBleue.replie();
               petitBrasBleu.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
               m_baseRoulante.goToXY((ushort)1150, (ushort)197, sens.avancer);
               m_baseRoulante.goToXY((ushort)1180, (ushort)197, sens.reculer);

               pinceBleue.deplie();//on ressort la pince
               m_baseRoulante.goToXY((ushort)824-i*100, (ushort)197, sens.avancer);

               pinceBleue.replie();
               poussoirBleu.replie();
               m_baseRoulante.goToXY((ushort)1387, (ushort)197, sens.avancer);
             }
             else
             {
               petitBrasJaune.tourner(Equipe);

               m_baseRoulante.goToXY((ushort)1200, (ushort)2803, sens.avancer);
               pinceJaune.replie();
               petitBrasJaune.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
               m_baseRoulante.goToXY((ushort)1150, (ushort)2803, sens.avancer);
               m_baseRoulante.goToXY((ushort)1180, (ushort)2803, sens.reculer);

               pinceJaune.deplie();//on ressort la pince
               m_baseRoulante.goToXY((ushort)824-i*100, (ushort)2803, sens.avancer);

               pinceJaune.replie();
               poussoirJaune.replie();
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
                  pinceBleue.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens horaire)
                  petitBrasBleue.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

                  poussoirBleu.deplie(); //À CODER : doit tourner de la même façon
                  rouletteBleue.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras
                    // petite rotation qui permet de bien attraper le cylindre

                  //En même temps!!!! ???? :
                  pinceBleue.semiReplie(); //À CODER : doit fermer la pince à moitié (45° dans le sens horaire)
                  petitBrasBleue.semiReplie(); //ATTENTION c'est bien petitBrasBleu et pas roulette comme dans la version précédente

                  m_baseRoulante.goToXY((ushort)1287, (ushort)197, sens.avancer);
                  pinceBleue.semiDeplie(); //À CODER : doit rouvrir la pince fermée à moitié (45° dans le sens antihoraire)
                  petitBrasBleu.semiDeplie();
                }
                else
                {
                  //En même temps!!!! ????? :
                  pinceJaune.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens ANTIhoraire car on est de l'autre côté du robot puisqu'on est jaunes cette fois!!!)
                  petitBrasJaune.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

                  poussoirJaune.deplie(); //À CODER : doit tourner de la même façon
                  rouletteJaune.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras
                     // petite rotation qui permet de bien attraper le cylindre

                  //En même temps!!!! ???? :
                  pinceJaune.semiReplie();//À CODER : doit fermer la pince à moitié (45° dans le sens horaire)
                  petitBrasJaune.semiReplie(); //idem c'est bien petitBrasJaune et pas rouletteJaune

                  m_baseRoulante.goToXY((ushort)1287, (ushort)2803, sens.avancer);
                  pinceJaune.semiDeplie();//À CODER : doit rouvrir la pince fermée à moitié (45° dans le sens antihoraire)
                  petitBrasJaune.semiDeplie();
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
                  pinceBleue.replie();
                  petitBrasBleu.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
                  m_baseRoulante.goToXY((ushort)1150, (ushort)197, sens.avancer);
                  m_baseRoulante.goToXY((ushort)1180, (ushort)197, sens.reculer);

                  pinceBleue.deplie();//on ressort la pince
                  m_baseRoulante.goToXY((ushort)824-i*100, (ushort)197, sens.avancer);

                  pinceBleue.replie();
                  poussoirBleu.replie();
                  m_baseRoulante.goToXY((ushort)1387, (ushort)197, sens.avancer);
                }
                else
                {
                  //WHILE COULEUR LU PAR LE CAPTEUR != JAUNE : ROULETTE.ROTATE(+,20) ?????
                  //À CODER!!!!!!!!!!!!!

                  m_baseRoulante.goToXY((ushort)1200, (ushort)2803, sens.avancer);
                  pinceJaune.replie();
                  petitBrasJaune.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
                  m_baseRoulante.goToXY((ushort)1150, (ushort)2803, sens.avancer);
                  m_baseRoulante.goToXY((ushort)1180, (ushort)2803, sens.reculer);

                  pinceJaune.deplie();//on ressort la pince
                  m_baseRoulante.goToXY((ushort)824-i*100, (ushort)2803, sens.avancer);

                  pinceJaune.replie();
                  poussoirJaune.replie();
                  m_baseRoulante.goToXY((ushort)1387, (ushort)2803, sens.avancer);
                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise
                return true;
            }, calculPriorite: () => 97-i, executionUnique: false));

          }


        }
    }
}
#endif
