using System;
using Microsoft.SPOT;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace GadgeteerApp4
{
    class CCapteurCouleur
    {
        ColorSense myColorSense;
        static int White = 2, Yellow = 1, Blue = 0;
        int c1, c2, c3, c4;
        int ourColor = Blue;

        int config = -1; int i = -1;
            // sens de rotation direct
            float pricisionAngle = Pi / 10;
            float defaultAngle = Pi / 4; //Toujours Pi/4

        public static int getHue(int red, int green, int blue)
        {

            float min = System.Math.Min(System.Math.Min(red, green), blue);
            float max = System.Math.Max(System.Math.Max(red, green), blue);

            float hue = 0f;
            if (max == red)
            {
                hue = (green - blue) / (max - min);

            }
            else if (max == green)
            {
                hue = 2f + (blue - red) / (max - min);

            }
            else
            {
                hue = 4f + (red - green) / (max - min);
            }

            hue = hue * 60;
            if (hue < 0) hue = hue + 360;

            return (int)System.Math.Round(hue);

        }
      

        
        //Constructeur
        void CCapteurCouleur(int id, int ourColor)
        {
            myColorSense = new ColorSense(id);
            /* Choix de la couleur 'par defaut la couleur est Bleu' */
            int config;
                        if (ourColor == Yellow)
            {
                c1 = -3; c2 = 1; c3 = 3; c4 = -1;
            }
            else
            {
                c1 = 1; c2 = -3; c3 = -1; c4 = 3;
            }
        }


        //Continuer la rotation
        bool ContinuerRotation(ref int couleurInitiale)
        {

            //Lire les couleurs 
            //***********
            ColorSense.ColorData colors = myColorSense.ReadColor();
            int HUE = getHue(colors.Red, colors.Blue, colors.Green);
            bool white = colors.Red + colors.Blue + colors.Green > 700;
            //**********
            //Attention !!!!!! il faut mettre le parametre couleurInitiale � -1 pour chaque cylindre
            if (couleurInitiale == -1)
            {
                if (white) couleurInitiale = 2;
                else if (HUE > 150) couleurInitiale = 0; //Blue
                else if (HUE < 100) couleurInitiale = 1; //Yellow
            }

            //--// 
            //Un cas particulier 
            if (i > 0)
            {
                if (i == 3) // 3 : nombre de pas a faire pour etre sure de la couleur (on peut changer ce parametre)
                {
                    wcolors = myColorSense.ReadColor();
                    HUE = getHue(wcolors.Red, wcolors.Blue, wcolors.Green);
                    if (HUE > 150) { config = 4; return false; }
                    else { config = 3; return false; }
                    //TurnAngle(-3 * pricisionAngle);
                    return true;
                }
                i++;
                return true;
            }

            if (HUE > 150)
            {
                if (couleurInitiale != 0)
                {
                    config = 1;
                    return false;
                }
                return true;

            }
            else if (HUE < 100)
            {
                if (couleurInitiale != 1)
                {
                    config = 2;
                    return false;
                }
                return true;
            }
            else if (white)
            {
                if (couleurInitiale != 0) { 
                i = 1; //TurnAngle(3 * pricisionAngle);
                   return true;
                 }
                return true;
            }
            else { return true; /* la couleur n'est pas claire*/ }
        }

//
        int CompleterRotation()
        {
            
            //
            switch (config)
            {
                case 1:
                    return c1 * defaultAngle;
                    break;
                case 2:
                    return c2 * defaultAngle;
                    break;
                case 3:
                    return c3 * defaultAngle;
                    break;
                case 4:
                    return c4 * defaultAngle;
                    break;
                default:
                    Debug.Print("Error 'config cylinder' !");
                    break;
            }
            return 0;
        }
    }
}
