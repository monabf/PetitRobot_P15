using System;
using Microsoft.SPOT;
using Kangaroo;

using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using System.Threading;

namespace PR.BR2
{
     enum etatBR
    {
        arrive=1,bloque,stope
    };
    enum sens
    { 
        avancer=1, reculer=-1
    };
    
    struct positionBaseRoulante
    {
        public int x;
        public int y;
        public int alpha;
    };
    

    class CBaseRoulante
    {
        CKangaroo m_kangaroo;
        public positionBaseRoulante m_posBR;
        public etatBR m_status=0;
        RelayX1 relai = new RelayX1(4);

        public CBaseRoulante(int numPort)
        {
            m_kangaroo = new CKangaroo(numPort);
            m_posBR = new positionBaseRoulante();
            m_kangaroo.init();
          
        }
        
        public void setCouleur(Couleur c)
        {
            if (c == Couleur.Bleu)
            {
                //NB: constantes à modifier
                m_posBR.x = 678;
                m_posBR.y = 73;
                m_posBR.alpha = 90;
            }
            else
            {
                // idem
                m_posBR.x = 678;
                m_posBR.y = 1927;
                m_posBR.alpha = -90;
            }
        }
        
        public void getPosition(ref positionBaseRoulante posBR)
        {
            posBR=m_posBR;
        }

        public int getDistanceParcourue(ref int distance)
        {
            int erreur = 0;
            int posCodeur = 0;
            erreur=m_kangaroo.getPosition(mode.drive, ref posCodeur);
            distance = (int)(posCodeur / (int)unite.mm);
            return erreur;
        }

        public int getAngleTourne(ref int angle)
        {
            int erreur = 0;
            int posCodeur = 0;
            erreur=m_kangaroo.getPosition(mode.turn, ref posCodeur);
            angle = (int)(posCodeur / (int)unite.degre);
            return erreur;
        }

        public void stop()
        {
            m_status = etatBR.stope;
            m_kangaroo.start(mode.drive);
        }



        public int recallerAngle(int alpha)
        {
            int beta = alpha;
            if (beta <= -180)
                beta += 360;
            if (beta > 180)
                beta -= 360;
            return beta;
        }

        public etatBR tourner(int alphaConsigne)
        {
                     
            int erreur = 0;
            int  alphaReel = 0, alphaReel_tm1=0;
            int delta = 0;
            m_status = 0;
            m_kangaroo.tourner(alphaConsigne);
            //attente d'être arrive ou bloque ou stoppe
            do
            {
                alphaReel_tm1 = alphaReel;
              //  Thread.Sleep(1000);
                erreur = getAngleTourne(ref alphaReel);
                if (erreur == 0xE3)
                {
                    alphaReel = alphaReel_tm1;
                    m_status = etatBR.bloque;
                    m_kangaroo.powerdown(mode.drive);
                    m_kangaroo.powerdown(mode.turn);
                    m_kangaroo.init();

                }
                else
                {
                    
                    delta = System.Math.Abs(alphaConsigne - alphaReel);
                    if (delta < 1)
                    {
                        m_status = etatBR.arrive;
                        Thread.Sleep(1000);
                        getAngleTourne(ref alphaReel);
                    }
                }
            } while (m_status != etatBR.arrive && m_status != etatBR.bloque && m_status != etatBR.stope);
                       
            return m_status;
        }

        public etatBR allerEn(double x,double y, sens s, int speed=1)
        {
            
            int posCodeur=0;
            int erreur = 0;
            int distanceConsigne=0,distanceReelle = 0, distanceReelle_tm1=0;
            int alphaConsigne = 0, alphaReel = 0, alphaReel_tm1=0;
            int delta = 0;
            int ecart_t = 0, ecart_tm1 = 0;
            int dureeBlocage = 0;

            

            alphaConsigne = (int)(System.Math.Atan2((y - m_posBR.y),(x - m_posBR.x)) * 180 / System.Math.PI)-m_posBR.alpha;  //angle en degre
            if (s == sens.reculer)
            {
                alphaConsigne = (int)(alphaConsigne + 180);
            }
            alphaConsigne = recallerAngle(alphaConsigne);
         //   m_status= tourner(alphaConsigne);  
            m_status = 0;
            m_kangaroo.tourner(alphaConsigne);
            //attente d'être arrive ou bloque ou stoppe
            do
            {
                alphaReel_tm1 = alphaReel;
                //  Thread.Sleep(1000);
                erreur = getAngleTourne(ref alphaReel);
                if (erreur == 0xE3)
                {
                    alphaReel = alphaReel_tm1;
                    m_status = etatBR.bloque;
                    relai.TurnOn();
                    Thread.Sleep(1000);
                    relai.TurnOff();
                    Thread.Sleep(1000);
                    m_kangaroo.init();
                }
                else
                {

                    delta = System.Math.Abs(alphaConsigne - alphaReel);
                    if (delta < 2)
                    {
                        m_status = etatBR.arrive;
                        Thread.Sleep(500);
                        getAngleTourne(ref alphaReel);
                    }
                }
            } while (m_status != etatBR.arrive && m_status != etatBR.bloque && m_status != etatBR.stope);
            delta = 0;
            m_status = 0;
            distanceConsigne = (int)s*(int)System.Math.Sqrt(System.Math.Pow((x - m_posBR.x), 2) + System.Math.Pow((y -m_posBR.y), 2));
            m_kangaroo.allerEn(distanceConsigne , speed, unite.mm);
            //attente d'être arrive ou bloque ou stoppe
            do
            {
                distanceReelle_tm1 = distanceReelle;
                erreur= getDistanceParcourue(ref distanceReelle);
                if (distanceReelle == 0)
                {
                    dureeBlocage++;
                    
                    if (dureeBlocage >= 10)
                    {
                        distanceReelle = distanceReelle_tm1;
                        m_status = etatBR.bloque;
                        m_kangaroo.start(mode.drive);
                    }

                }
                else
                    dureeBlocage = 0;

                delta = System.Math.Abs(distanceConsigne - distanceReelle);
                if (delta < 5)
                {
                    m_status = etatBR.arrive;
                    Thread.Sleep(1000);
                    erreur = getDistanceParcourue(ref distanceReelle);
                }
                
                if (erreur == 0xE3)
                {
                    distanceReelle = distanceReelle_tm1;
                    m_status = etatBR.bloque;
                    relai.TurnOn();
                    Thread.Sleep(1000);
                    relai.TurnOff();
                    Thread.Sleep(1000);
                    m_kangaroo.init();
                    

                }
                

            } while (m_status != etatBR.arrive && m_status != etatBR.bloque && m_status != etatBR.stope);
            m_posBR.alpha = m_posBR.alpha + alphaReel;
            m_posBR.x = m_posBR.x + (int)(distanceReelle * System.Math.Cos(m_posBR.alpha * System.Math.PI / 180));
            m_posBR.y = m_posBR.y + (int)(distanceReelle * System.Math.Sin(m_posBR.alpha * System.Math.PI / 180));
          /*  m_kangaroo.powerdown(mode.drive);
            m_kangaroo.powerdown(mode.turn);
            m_kangaroo.init();*/
            return m_status;
        }

      /*  public void tourner(int angle)
        {
            int posCodeur = 0;
            int alpha = 0;
                       
            m_kangaroo.tourner(angle);
            m_kangaroo.getPosition(mode.turn, ref posCodeur);
            alpha = (int)(posCodeur / (int)unite.degre);
            m_posBR.alpha = m_posBR.alpha + alpha;
        }*/
    }
}
