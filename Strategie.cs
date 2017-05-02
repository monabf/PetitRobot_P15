using System;
using Microsoft.SPOT;
using System.Threading;
using System.Collections;
using PR;
using PR.BR2;


namespace PR
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
           setSpeed(50);

           /*ATTENTION : on a deux sets de 3 pinces, un pour chaque couleur, donc il faut quand on initialise les composants créer
           deux instances de la classe Pince, pinceJaune et pinceBleue....*/

           GestionStrat.Ajouter(new ActionRobot(() => //Initialisation de l'homologation
           {
             m_ihm.Afficher("Homologation");
             //on va vers la fusée, on récupère un cylindre et on le met dans la rigole et c'est tout !

             //on va vers la fusée :
             if (m_etatRobot.couleurEquipe == Couleur.Jaune)
             {
               robotGoToXY((ushort)157, (ushort)2130, sens.avancer);
               robotRotate(-90);
               robotGoToXY((ushort)140, (ushort)2130, sens.avancer);
               robotGoToXY((ushort)885, (ushort)2130, sens.avancer);
               robotRotate(90);
               robotGoToXY((ushort)885, (ushort)2862, sens.avancer);
               robotGoToXY((ushort)885, (ushort)2803, sens.avancer);
             }
             else
             {
               robotGoToXY((ushort)157, (ushort)870, sens.avancer);
               robotRotate(90);
               robotGoToXY((ushort)140, (ushort)870, sens.avancer);
               robotGoToXY((ushort)885, (ushort)870, sens.avancer);
               robotRotate(-90);
               robotGoToXY((ushort)885, (ushort)138, sens.avancer);
               robotGoToXY((ushort)885, (ushort)197, sens.avancer);
             }

             //on fait le créneau
             setSpeed(20);

             if (m_etatRobot.couleurEquipe == Couleur.Jaune)
             {
               robotRotate(90);
               robotGoToXY((ushort)1387, (ushort)2803, sens.avancer);
             }
             else
             {
               robotRotate(-90);
               robotGoToXY((ushort)1387, (ushort)197, sens.avancer);
             }

             //on récupère le cylindre
             if (m_etatRobot.couleurEquipe == Couleur.Jaune)
             {
               //En même temps!!!! ????? :
               pince.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens ANTIhoraire car on est de l'autre côté du robot puisqu'on est jaunes cette fois!!!)
               petitBras.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

               poussoir.deplie(); //À CODER : doit tourner de la même façon
               petitBras.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras
                  // petite rotation qui permet de bien attraper le cylindre

               //En même temps!!!! ???? :
               pince.semiReplie();//À CODER : doit fermer la pince à moitié (45° dans le sens horaire)
               petitBras.semiReplie(); //idem c'est bien petitBrasJaune et pas rouletteJaune

               robotGoToXY((ushort)1287, (ushort)2803, sens.avancer);
               pince.semiDeplie();//À CODER : doit rouvrir la pince fermée à moitié (45° dans le sens antihoraire)
               petitBras.semiDeplie();
             }
             else
             {
               //En même temps!!!! ????? :
               pince.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens horaire)
               petitBras.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!! Cette fois c'est le sens antihoraire

               poussoir.deplie(); //À CODER : doit tourner de la même façon
               roulette.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras, dans le sens antihoraire

               //En même temps!!!! ???? :
               pince.semiDeplie(); //À CODER : doit continuer à ouvrir la pince à moitié (45° dans le sens horaire)
               petitBras.semiReplie(); //À CODER : doit fermer la pince à moitié (45° dans le sens horaire)

               robotGoToXY((ushort)1287, (ushort)197, sens.avancer);
               pince.semiReplie(); //À CODER : doit refermer la pince fermée à moitié (45° dans le sens antihoraire)
               petitBras.semiDeplie(); //À CODER : doit rouvrir le petit bras à moitié (45° dans le sens antihoraire)
             }

             //on tourne et on dépose le cylindre
             setSpeed(35);

             if (m_etatRobot.couleurEquipe == Couleur.Jaune)
             {
               petitBras.tourner(Equipe);

               robotGoToXY((ushort)1200, (ushort)2803, sens.avancer);
               pince.replie();
               petitBras.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
               robotGoToXY((ushort)1150, (ushort)2803, sens.avancer);
               robotGoToXY((ushort)1180, (ushort)2803, sens.reculer);

               pince.deplie();//on ressort la pince
               robotGoToXY((ushort)824-i*100, (ushort)2803, sens.avancer);

               pince.replie();
               poussoir.replie();
               robotGoToXY((ushort)1387, (ushort)2803, sens.avancer);
             }
             else
             {
               petitBras.tourner();

               robotGoToXY((ushort)1200, (ushort)197, sens.avancer);
               pince.replie();
               petitBras.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
               robotGoToXY((ushort)1150, (ushort)197, sens.avancer);
               robotGoToXY((ushort)1180, (ushort)197, sens.reculer);

               pince.deplie();//on ressort la pince
               robotGoToXY((ushort)824-i*100, (ushort)197, sens.avancer);

               pince.replie();
               poussoir.replie();
               robotGoToXY((ushort)1387, (ushort)197, sens.avancer);
             }



           }, calculPriorite: () => 100, executionUnique: true));

#else
            positionBaseRoulante positionRobot = new positionBaseRoulante();

            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 1ère action : aller vers la fusée
            {
//                m_ihm.Afficher("Se deplacer vers la fusee");

                if (m_etatRobot.couleurEquipe == Couleur.Jaune)
                {
                  robotGoToXY((ushort)157, (ushort)2130, sens.avancer);
                  robotRotate(-90);
                  robotGoToXY((ushort)140, (ushort)2130, sens.avancer);
                  robotGoToXY((ushort)885, (ushort)2130, sens.avancer);
                  robotRotate(90);
                  robotGoToXY((ushort)885, (ushort)2862, sens.avancer);
                  robotGoToXY((ushort)885, (ushort)2803, sens.avancer);
                }
                else
                {
                  robotGoToXY((ushort)157, (ushort)870, sens.avancer);
                  robotRotate(90);
                  robotGoToXY((ushort)140, (ushort)870, sens.avancer);
                  robotGoToXY((ushort)885, (ushort)870, sens.avancer);
                  robotRotate(-90);
                  robotGoToXY((ushort)885, (ushort)138, sens.avancer);
                  robotGoToXY((ushort)885, (ushort)197, sens.avancer);
                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise
                return true;
            }, calculPriorite: () => 100, executionUnique: true));




            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 2ème action : Créneau pour être face à la fusée
            {
                //m_ihm.Afficher("Creneau");

                // m_baseRoulante.setSpeed(20);

                if (m_etatRobot.couleurEquipe == Couleur.Jaune)
                {
                  robotRotate(90);
                  robotGoToXY((ushort)1387, (ushort)2803, sens.avancer);
                }
                else
                {
                  robotRotate(-90);
                  robotGoToXY((ushort)1387, (ushort)197, sens.avancer);
                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise

                return true;
            }, calculPriorite: () => 99, executionUnique: true));




            for(int i=0;i<4;i++){

            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 3ème action : Récupérer le cylindre
            {
                //m_ihm.Afficher("Recupere le cylindre");

                if (m_etatRobot.couleurEquipe == Couleur.Jaune)
                {
                  //En même temps!!!! ????? :
                  pince.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens ANTIhoraire car on est de l'autre côté du robot puisqu'on est jaunes cette fois!!!)
                  petitBras.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

                  poussoir.deplie(); //À CODER : doit tourner de la même façon
                  //petitBras.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras
                     // petite rotation qui permet de bien attraper le cylindre, finalement inutile

                  //En même temps!!!! ???? :
                  pince.semiReplie();//À CODER : doit fermer la pince à moitié (45° dans le sens horaire)
                  petitBras.semiReplie(); //idem c'est bien petitBrasJaune et pas rouletteJaune

                  robotGoToXY((ushort)1287, (ushort)2803, sens.avancer);
                  pince.semiDeplie();//À CODER : doit rouvrir la pince fermée à moitié (45° dans le sens antihoraire)
                  petitBras.semiDeplie();
                }
                else
                {
                  //En même temps!!!! ????? :
                  pince.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens horaire)
                  petitBras.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!!

                  poussoir.deplie(); //À CODER : doit tourner de la même façon
                  //petitBras.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras
                    // petite rotation qui permet de bien attraper le cylindre, finalement inutile

                  //En même temps!!!! ???? :
                  pince.semiReplie(); //À CODER : doit fermer la pince à moitié (45° dans le sens horaire)
                  petitBras.semiReplie(); //ATTENTION c'est bien petitBrasBleu et pas roulette comme dans la version précédente

                  robotGoToXY((ushort)1287, (ushort)197, sens.avancer);
                  pince.semiDeplie(); //À CODER : doit rouvrir la pince fermée à moitié (45° dans le sens antihoraire)
                  petitBras.semiDeplie();
                }

                return true;
            }, calculPriorite: () => 98-i, executionUnique: false));




            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 4ème action : Faire tourner le cylindre pour mettre la même couleur sur le dessus et dépôt du cylindre dans la rainure, puis retour face à la fusee
            {
                //m_ihm.Afficher("Rotation et depot du cylindre");

                if (m_etatRobot.couleurEquipe == Couleur.Jaune)
                {
                  //WHILE COULEUR LU PAR LE CAPTEUR != JAUNE : ROULETTE.robotRotate(+,20) ?????
                  //À CODER!!!!!!!!!!!!!

                  robotGoToXY((ushort)1200, (ushort)2803, sens.avancer);
                  pince.replie();
                  petitBras.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
                  robotGoToXY((ushort)1150, (ushort)2803, sens.avancer);
                  robotGoToXY((ushort)1180, (ushort)2803, sens.reculer);

                  pince.deplie();//on ressort la pince
                  robotGoToXY((ushort) (824-i*100), (ushort)2803, sens.avancer);

                  pince.replie();
                  poussoir.replie();
                  robotGoToXY((ushort)1387, (ushort)2803, sens.avancer);

                  //retour en face de la fusée après avoir déposé le cylindre
                  robotGoToXY((ushort)1387, (ushort)2803, sens.reculer);
                }
                else
                {
                  //WHILE COULEUR LU PAR LE CAPTEUR != BLEU : ROULETTE.robotRotate(+,20) ?????
                  //À CODER!!!!!!!!!!!!!

                  robotGoToXY((ushort)1200, (ushort)197, sens.avancer);
                  pince.replie();
                  petitBras.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
                  robotGoToXY((ushort)1150, (ushort)197, sens.avancer);
                  robotGoToXY((ushort)1180, (ushort)197, sens.reculer);

                  pince.deplie();//on ressort la pince
                  robotGoToXY((ushort) (824-i*100), (ushort)197, sens.avancer);

                  pince.replieArriere();//attention replieArriere pour la pince aussi
                  poussoir.replieArriere();//attention le poussoir doit cette fois se replier vers l'arriere donc il faut une autre fonction replieArriere qui fasse le même mouvement dans l'autre sens
                  robotGoToXY((ushort)1387, (ushort)197, sens.avancer);

                  //retour en face de la fusée après avoir déposé le cylindre
                  robotGoToXY((ushort)1387, (ushort)197, sens.reculer);
                }


                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise
                return true;
            }, calculPriorite: () => 97-i, executionUnique: false));

          }


        }
    }
}
#endif
