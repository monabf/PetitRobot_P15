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
        /// 
        public void InitialisationStrategie()
        {

            bool homologation = (robotGetDisposition() == 1);

/*           positionBaseRoulante positionRobot = new positionBaseRoulante();
           setSpeed(50);

         //  ATTENTION : on a deux sets de 3 pinces, un pour chaque couleur, donc il faut quand on initialise les composants créer
         //  deux instances de la classe Pince, pinceJaune et pinceBleue....

           GestionStrat.Ajouter(new ActionRobot(() => //Initialisation de l'homologation
           {
             m_ihm.Afficher("Homologation");
             //on va vers la fusée, on récupère un cylindre et on le met dans la rigole et c'est tout !

             //on va vers la fusée :
             if (m_etatRobot.couleurEquipe == Couleur.Jaune)
             {
               robotGoToXY((ushort)244, (ushort)2212, sens.avancer);
               robotRotate(90);
               robotGoToXY((ushort)147, (ushort)2128, sens.avancer);
               robotGoToXY((ushort)715, (ushort)2128, sens.avancer);
               robotRotate(-90);
               robotGoToXY((ushort)800, (ushort)2870, sens.avancer);
               robotGoToXY((ushort)800, (ushort)2718, sens.avancer);
             }
             else
             {
               robotGoToXY((ushort)244, (ushort)788, sens.avancer);
               robotRotate(90);
               robotGoToXY((ushort)147, (ushort)872, sens.avancer);
               robotGoToXY((ushort)715, (ushort)872, sens.avancer);
               robotRotate(-90);
               robotGoToXY((ushort)800, (ushort)130, sens.avancer);
               robotGoToXY((ushort)800, (ushort)282, sens.avancer);
             }

             //on fait le créneau
             setSpeed(20);

             if (m_etatRobot.couleurEquipe == Couleur.Jaune)
             {
               robotRotate(-90);
               robotGoToXY((ushort)1387, (ushort)197, sens.avancer);
             }
             else
             {
               robotRotate(90);
               robotGoToXY((ushort)1387, (ushort)2803, sens.avancer);
             }

             //on récupère le cylindre
             if (m_etatRobot.couleurEquipe == Couleur.Jaune)
             {
               pinceJaune.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens ANTIhoraire car on est de l'autre côté du robot puisqu'on est jaunes cette fois!!!)
               petitBrasJaune.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!! Cette fois dans le sens horaire

               poussoirJaune.deplie(); //À CODER : doit tourner de la même façon
               rouletteJaune.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras

               //En même temps!!!! ???? :
               pinceJaune.semiDeplie();//À CODER : doit ouvrir la pince à moitié (45° dans le sens antihoraire)
               petitBrasJaune.semiReplie();//À CODER : 45° dans le sens antihoraire

               robotGoToXY((ushort)1287, (ushort)2803, sens.avancer);
               pinceJaune.semiReplie();//À CODER : doit refermer la pince fermée à moitié (45° dans le sens horaire)
               petitBrasJaune.semiDeplie(); //À CODER : doit rouvrir le petit bras fermé à moitié (45° dans le sens horaire)
             }
             else
             {
               pinceBleue.deplie(); //À CODER : doit ouvrir la pince (rotation de 90° dans le sens horaire)
               petitBrasBleue.deplie(); //À CODER : doit tourner de la même façon, le servomoteur est celui à la base du petit bras!! Cette fois c'est le sens antihoraire

               poussoirBleu.deplie(); //À CODER : doit tourner de la même façon
               rouletteBleue.initialiserRoue(); //À CODER : doit tourner avec les mêmes conventions, mais cette fois c'est la roulette qui tourne et non pas le servo à la base du petit bras, dans le sens antihoraire

               //En même temps!!!! ???? :
               pinceBleue.semiDeplie(); //À CODER : doit continuer à ouvrir la pince à moitié (45° dans le sens horaire)
               petitBrasBleu.semiReplie(); //À CODER : doit fermer la pince à moitié (45° dans le sens horaire)

               robotGoToXY((ushort)1287, (ushort)197, sens.avancer);
               pinceBleu.semiReplie(); //À CODER : doit refermer la pince fermée à moitié (45° dans le sens antihoraire)
               petitBrasBleu.semiDeplie(); //À CODER : doit rouvrir le petit bras à moitié (45° dans le sens antihoraire)
             }

             //on tourne et on dépose le cylindre
             setSpeed(35);

             if (m_etatRobot.couleurEquipe == Couleur.Jaune)
             {
               petitBrasJaune.tourner(Equipe);

               robotGoToXY((ushort)1200, (ushort)2803, sens.avancer);
               pinceJaune.replie();
               petitBrasJaune.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
               robotGoToXY((ushort)1150, (ushort)2803, sens.avancer);
               robotGoToXY((ushort)1180, (ushort)2803, sens.reculer);

               pinceJaune.deplie();//on ressort la pince
               robotGoToXY((ushort)824-i*100, (ushort)2803, sens.avancer);

               pinceJaune.replie();
               poussoirJaune.replie();
               robotGoToXY((ushort)1387, (ushort)2803, sens.avancer);
             }
             else
             {
               petitBrasBleu.tourner();

               robotGoToXY((ushort)1200, (ushort)197, sens.avancer);
               pinceBleue.replie();
               petitBrasBleu.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
               robotGoToXY((ushort)1150, (ushort)197, sens.avancer);
               robotGoToXY((ushort)1180, (ushort)197, sens.reculer);

               pinceBleue.deplie();//on ressort la pince
               robotGoToXY((ushort)824-i*100, (ushort)197, sens.avancer);

               pinceBleue.replie();
               poussoirBleu.replie();
               robotGoToXY((ushort)1387, (ushort)197, sens.avancer);
             }

           }, calculPriorite: () => 100, executionUnique: true));

      */
            positionBaseRoulante positionRobot = new positionBaseRoulante();
            int ki = 0;

            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 1ère action : aller vers la fusée
            {
//                m_ihm.Afficher("Se deplacer vers la fusee");
                m_ihm.retourPhase(Couleurs.rouge);

                if (m_etatRobot.couleurEquipe == Couleur.Jaune)
                {
                  robotGoToXY((ushort)157, (ushort)2065, sens.avancer, false, 12);
                  robotRotate(-90,500);
                  recalageX(90, 85,sens.reculer,10,1500);//30, temps 1s
                  getPosition(ref positionRobot);
                  robotGoToXY((ushort)840, (ushort)positionRobot.x, sens.avancer, homologation, 18);//y=880
                  robotGoToXY((ushort)840, (ushort)2790, sens.reculer,homologation,20);
                  getPosition(ref positionRobot);
                  recalageY(180, 2811,sens.reculer,8,700);//134
                  robotGoToXY((ushort)positionRobot.y, (ushort)2710, sens.avancer, homologation, 10);
                  pince.deplie();
                  poussoir.deplie();
                }
                else
                {

                  robotGoToXY((ushort)157, (ushort)935, sens.avancer, false, 12);//157,895
                  robotRotate(90,500);
                  recalageX(90, 85,sens.reculer,10,1500);// On peut gagner un peu de temps sur cette transition
                  getPosition(ref positionRobot);
                  robotGoToXY((ushort)840, (ushort)positionRobot.x, sens.avancer,homologation,18);// On peut accélérer un tout petit peu
                  robotGoToXY((ushort)840, (ushort)210, sens.reculer, homologation, 20); // Humm 840,170
                  getPosition(ref positionRobot);

                  recalageY(0, 189,sens.reculer,8,700);// Recalage sur la fusée

                  robotGoToXY((ushort)positionRobot.y, (ushort)290, sens.avancer, homologation, 10);// v=15 On peut peut être un peu accélérer
                  pince.deplie();
                  poussoir.deplie();





                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise
                return true;
            }, calculPriorite: () => 100, executionUnique: true));




            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 2ème action : Créneau pour être face à la fusée
            {
                //m_ihm.Afficher("Creneau");
                m_ihm.retourPhase(Couleurs.orange);

                if (m_etatRobot.couleurEquipe == Couleur.Jaune)
                {
                  robotGoToXY((ushort)1445, (ushort)2740, sens.reculer,false,15);//1387,197
                  pince.milieu();
                  poussoir.milieu();

                  getPosition(ref positionRobot);
                  robotGoToXY((ushort)1440, (ushort)positionRobot.x, sens.avancer);//1440,252
                  


                }
                else
                {
                    robotGoToXY((ushort)1445, (ushort)260, sens.reculer,false,15);//1387,197
                    pince.milieu();
                    poussoir.milieu();
                    getPosition(ref positionRobot);
                    robotGoToXY((ushort)1440, (ushort)positionRobot.x, sens.avancer);//1440,252

                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise

                return true;
            }, calculPriorite: () => 99, executionUnique: true));




            for(int i=0;i<4;i++){

            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 3ème action : Récupérer le cylindre
            {
                m_ihm.retourPhase(Couleurs.jaune);
                //m_ihm.Afficher("Recupere le cylindre");
                var thServo = new Thread(() =>
                {
                    int j = 0;
                    pince.semiReplie();
                    poussoir.semiReplie();
                    while (j!=2)
                    {

                        pince.semiReplie();
                        //poussoir.semiReplie();
                        Thread.Sleep(500);
                        pince.milieu();
                        //poussoir.deplie();
                        Thread.Sleep(500);
                        j++;

                    }
                    pince.milieu();
                    poussoir.milieu();

                });
                thServo.Start();
                int ei = 0;

                if (ki == 0) ei = 0;
                else if (ki == 1) ei = 2;
                else if (ki == 2) ei = 2;
                else if (ki == 3) ei = 3;

                if (m_etatRobot.couleurEquipe == Couleur.Jaune)
                {
                  robotGoToXY((ushort)1345, (ushort)(2740+ei), sens.avancer, false, 2);
                  thServo.Suspend();
                  poussoir.milieu();
                  pince.milieu();
                  petitBras.deplie();
                  getPosition(ref positionRobot);
                  robotGoToXY((ushort)1305, (ushort)positionRobot.x, sens.avancer, false, 5);//1305,249
                  petitBras.tourner();
                  petitBras.replie();
                  pince.semiDeplie();
                  if (ki != 3) robotGoToXY((ushort)1005, (ushort)(2740 + ei), sens.avancer, false, 10);
                  else robotGoToXY((ushort)1105, (ushort)(2740 + ei), sens.avancer, false, 10);
                  Thread.Sleep(50);
                  pince.deplie();
                  poussoir.deplie();
                  Thread.Sleep(500);
                  ki++;
                  if (ki == 0) ei = 0;
                  else if (ki == 1) ei = 4;
                  else if (ki == 2) ei = 5;
                  else if (ki == 3) ei = 6;
                  
                  robotGoToXY((ushort)1440, (ushort)(2740+ei), sens.reculer, false, 12);//1440,254
                  pince.milieu();
                  poussoir.milieu();
                  Thread.Sleep(500);

                }
                else
                {
                    robotGoToXY((ushort)1345, (ushort)(260-ei), sens.avancer, false, 3); //v=2 On peut essayer d'augmenter les vitesses
                    thServo.Suspend();// On peut également essayer d'augmenter les vitesses des servomoteurs.
                    poussoir.milieu();
                    pince.milieu();
                    petitBras.deplie();
                    getPosition(ref positionRobot);
                    Thread.Sleep(500);
                    //robotGoToXY((ushort)1305, (ushort)positionRobot.x, sens.avancer, homologation, 10);//1305,249 v=5 On peut augmenter les vitesses  
                    petitBras.tourner();
                    petitBras.replie();
                    pince.semiDeplie();
                    Thread.Sleep(50); // on peut peut être l'enlever

                    if (ki != 3) robotGoToXY((ushort)1005, (ushort)(260 - ei), sens.avancer, false, 10);//1005,254 v=7 On peut augmenter les vitesses.
                    else robotGoToXY((ushort)1105, (ushort)(260 - ei), sens.avancer, false, 10);
                    pince.deplie();
                    poussoir.deplie();
                    ki++;
                    if (ki == 0) ei = 0;
                    else if (ki == 1) ei = 2;
                    else if (ki == 2) ei = 2;
                    else if (ki == 3) ei = 3;
                    robotGoToXY((ushort)1450, (ushort)(260 - ei), sens.reculer, false, 20);//1440,254 v=12
                    pince.milieu();
                    poussoir.milieu();
                    Thread.Sleep(250);// on peut peut être l'enlever


                }

                return true;
            }, calculPriorite: () => 98-i, executionUnique: false));




            GestionStrat.Ajouter(new ActionRobot(() =>              //Initialisation de la 4ème action : RFaire tourner le cylindre pour mettre la même couleur sur le dessus et dépôt du cylindre dans la rainure
            {
                //m_ihm.Afficher("Rotation et depot du cylindre");
                m_ihm.retourPhase(Couleurs.vert);

                if (m_etatRobot.couleurEquipe == Couleur.Jaune)
                {
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
                }
                else
                {
                  robotGoToXY((ushort)1200, (ushort)197, sens.avancer);
                  pince.replie();
                  petitBras.deplie();//on range pince et petit bras et on va jusqu'à la gouttière
                  robotGoToXY((ushort)1150, (ushort)197, sens.avancer);
                  robotGoToXY((ushort)1180, (ushort)197, sens.reculer);

                  pince.deplie();//on ressort la pince
                  robotGoToXY((ushort) (824-i*100), (ushort)197, sens.avancer);

                  pince.replie();
                  poussoir.replie();
                  robotGoToXY((ushort)1387, (ushort)197, sens.avancer);
                }

                m_baseRoulante.getPosition(ref positionRobot); //on récupère la position réelle et on l'actualise
                return true;
            }, calculPriorite: () => 97-i, executionUnique: false));

          }


        }
    }
}
//#endif
