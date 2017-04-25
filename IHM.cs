using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Touch;
using GHI.Glide;
using GHI.Glide.Display;
using GHI.Glide.UI;
using System.Threading;

namespace PR
{
    class IHMSelection
    {
        #region Attributs

        GHI.Glide.Display.Window fenetreSelection, fenetreAffichage;

        Couleur equipe = Couleur.Null;
        int disposition = 0;
        public bool validation = false;

        #endregion

        #region Attributs selection
        
        GHI.Glide.UI.Button BoutonBleu;
        GHI.Glide.UI.Button BoutonJaune;
        GHI.Glide.UI.Button BoutonDispo1;
        GHI.Glide.UI.Button BoutonDispo2;
        GHI.Glide.UI.Button BoutonDispo3;
        GHI.Glide.UI.Button BoutonDispo4;
        GHI.Glide.UI.Button BoutonDispo5;
        GHI.Glide.UI.Button BoutonValider;
        TextBlock TexteCouleur;
        TextBlock TexteDispo;

        #endregion

        #region Attributs affichage

        TextBlock TexteAffichage;
        string[] liste = new string[150];
        int nbLignes;
        int hauteurLigne;
        int largeurLigne;
        
        #endregion


        public IHMSelection()
        {
            GlideTouch.Initialize();
            Debug.Print("IHM created");
            fenetreSelection = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.fenetreSelection));     //Charge le fichier XML pour la fenetre de selection
            fenetreAffichage = GlideLoader.LoadWindow(Resources.GetString(Resources.StringResources.fenetreAffichage));     // -- -- -- -- -- -- -- -- -- -- -- -- -- d'affichage
            Debug.Print("IHM completed");
                        Glide.FitToScreen = true;       //Dimensionne la fenetre pour l'adapter à l'écran LCD
            
        }

        /// <summary>
        /// Selection de l'équipe et de la disposition du terrais parmi les 5 différentes
        /// </summary>
        /// 
        public Couleur getEquipe()
        {
            return equipe;
        }

        public int getDisposition()
        {
            return disposition;
        }

        public void Selection(ref Couleur m_equipe, ref int m_disposition)
        {
            Glide.MainWindow = fenetreSelection;        //Affiche la fenetre de selection sur l'écran LCD
            
            BoutonBleu = (GHI.Glide.UI.Button)fenetreSelection.GetChildByName("BoutonBleu");            //Créer les différents boutons
            BoutonJaune = (GHI.Glide.UI.Button)fenetreSelection.GetChildByName("BoutonJaune");
            BoutonDispo1 = (GHI.Glide.UI.Button)fenetreSelection.GetChildByName("BoutonDispo1");
            BoutonDispo2 = (GHI.Glide.UI.Button)fenetreSelection.GetChildByName("BoutonDispo2");
            BoutonDispo3 = (GHI.Glide.UI.Button)fenetreSelection.GetChildByName("BoutonDispo3");
            BoutonDispo4 = (GHI.Glide.UI.Button)fenetreSelection.GetChildByName("BoutonDispo4");
            BoutonDispo5 = (GHI.Glide.UI.Button)fenetreSelection.GetChildByName("BoutonDispo5");
            BoutonValider = (GHI.Glide.UI.Button)fenetreSelection.GetChildByName("BoutonValider");

            TexteCouleur = (TextBlock)fenetreSelection.GetChildByName("TexteCouleur");        //Bloc de texte pour la selection (affiche la couleur et la disposition choisie)
            TexteDispo = (TextBlock)fenetreSelection.GetChildByName("TexteDispo");            //

            BoutonBleu.TapEvent += new OnTap(sender => { equipe = Couleur.Bleu;
                                                         TexteCouleur.Text = "Equipe bleue";
                                                         TexteCouleur.BackColor = (Color)0x1E7FCB;
                                                         fenetreSelection.FillRect(TexteCouleur.Rect);
                                                         TexteCouleur.Invalidate(); });

            BoutonJaune.TapEvent += new OnTap(sender => { equipe = Couleur.Jaune;
                                                           TexteCouleur.Text = "Equipe jaune";
                                                           TexteCouleur.BackColor = (Color)0xE8D630;
                                                           fenetreSelection.FillRect(TexteCouleur.Rect);
                                                           TexteCouleur.Invalidate(); });

            BoutonDispo1.TapEvent += new OnTap(sender => { disposition = 1;
                                                           TexteDispo.Text = "Dispo. no. 1";
                                                           fenetreSelection.FillRect(TexteDispo.Rect);
                                                           TexteDispo.Invalidate(); });

            BoutonDispo2.TapEvent += new OnTap(sender => { disposition = 2;
                                                           TexteDispo.Text = "Dispo. no. 2";
                                                           fenetreSelection.FillRect(TexteDispo.Rect);
                                                           TexteDispo.Invalidate(); });

            BoutonDispo3.TapEvent += new OnTap(sender => { disposition = 3;
                                                           TexteDispo.Text = "Dispo. no. 3";
                                                           fenetreSelection.FillRect(TexteDispo.Rect);
                                                           TexteDispo.Invalidate(); });

            BoutonDispo4.TapEvent += new OnTap(sender => { disposition = 4;
                                                           TexteDispo.Text = "Dispo. no. 4";
                                                           fenetreSelection.FillRect(TexteDispo.Rect);
                                                           TexteDispo.Invalidate(); });

            BoutonDispo5.TapEvent += new OnTap(sender => { disposition = 5;
                                                           TexteDispo.Text = "Dispo. no. 5";
                                                           fenetreSelection.FillRect(TexteDispo.Rect);
                                                           TexteDispo.Invalidate(); });

            while (equipe == Couleur.Null || disposition == 0)      //Execution de la boucle tant que l'equipe et la disposition du terrain ne sont pas validés
            
            m_equipe = equipe;
            m_disposition = disposition;

            while (validation == false)
            {
                BoutonValider.TapEvent += new OnTap(sender => { validation = true; });
            }
            
            Fermer();   //ferme la fenetre de selection  

            Glide.MainWindow = fenetreAffichage;            //Affiche la fenetre d'affichage sur l'écran LCD
            TexteAffichage = (TextBlock)fenetreAffichage.GetChildByName("TexteAffichage");          //Bloc de texte pour l'affichage des instructions
        }

        /// <summary>
        /// Affiche un message sur l'IHM
        /// </summary>
        /// <param name="message">Message a afficher sur l'IHM</param>
        public void Afficher(string message)
        {         
            //TexteAffichage.Font.ComputeTextInRect("", out largeurLigne, out hauteurLigne);          //Retourne la largeur d'une ligne et la hauteur d'une ligne
            //nbLignes = TexteAffichage.Height / hauteurLigne;
            
            TexteAffichage.Text = message + '\n';
            fenetreAffichage.FillRect(TexteAffichage.Rect);
            TexteAffichage.Invalidate();

            Thread.Sleep(500);     //test
        }

        /// <summary>
        /// Ferme la fenetre
        /// </summary>
        public void Fermer()
        {
            Glide.MainWindow = null;
            Glide.Screen.Clear();       
            Glide.Screen.Flush();       
        }
    }
}
