using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.IO.Ports;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace Kangaroo
{
    enum mode
    {
        moteur1=0x31,moteur2=0x32,drive=0x44,turn=0x54
    };

    enum unite
    {
        mm = 6, cm = 65, m = 650, degre = 9, kmh =(int) (15000/12)//20000
    };
    class CKangaroo
    {
        SerialPort m_port;

        public CKangaroo(int numPort)
        {

            string COMPort = GT.Socket.GetSocket(numPort, true, null, null).SerialPortName;
            m_port = new SerialPort(COMPort, 9600, Parity.None, 8, StopBits.One);
            m_port.ReadTimeout = 500;
            m_port.WriteTimeout = 500;
            m_port.Open();

        }

        public bool start(mode m)
        {
            String commande;
            byte[] buffer = new byte[100];
            bool retour = false;
            buffer[0]=(byte)m;
            commande=BitConverter.ToChar(buffer,0).ToString()+ ",start\r\n";
            buffer = System.Text.Encoding.UTF8.GetBytes(commande);
            if (m_port.IsOpen)
            {
                m_port.Write(buffer, 0, commande.Length);
                retour = true;
            }
            return retour;
        }

        public bool resetCodeur()
        {
            bool retour = false;
            String commande;
            byte[] buffer = new byte[100];

            if (m_port.IsOpen)
            {
                start(mode.drive);
                commande = "D, UNITS 1696 mm = 128 lines";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);
                start(mode.drive);
                commande = "D,p0s0\r\n";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);

                start(mode.turn);
                commande = "T, UNITS 360 degrees = 37 lines";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);
                start(mode.turn);
                commande = "T,p0s0\r\n";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);
            }
            return retour;
        }

        public bool init()
        {
            bool retour = false;
            String commande;
            byte[] buffer = new byte[100];

            if (m_port.IsOpen)
            {
                start(mode.drive);
                commande = "D, UNITS 1696 mm = 128 lines";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);
                start(mode.drive);
                commande = "D,p0s0\r\n";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);

                start(mode.turn);
                commande = "T, UNITS 360 degrees = 37 lines";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);
                start(mode.turn);
                commande = "T,p0s0\r\n";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);
            }
            return retour;
        }

        //retourne un code erreur
        //0 pas d'erreur
        public int getPosition(mode m, ref int position)
        {
            String commande, sPosition,sErreur;
            byte[] reponse = new byte[100];
            char[] tempo = new char[10];
            int codeErreur = 0;
            byte[] buffer = new byte[100];
            
            if (m_port.IsOpen)
            {
                buffer[0] = (byte)m;
                commande = BitConverter.ToChar(buffer, 0).ToString() + ",getp\r\n";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                int t = commande.Length;
                m_port.Write(buffer, 0, commande.Length);

                int i = 0;
                do
                {
                    reponse[i++] = (byte)m_port.ReadByte();
                } while (reponse[i - 1] != '\n' && i < 99);
                reponse[i] = (byte)'\0';
                if (reponse[2] != 'E')
                {
                    int j = 0;
                    
                    int taille=0;
                    do
                    {                        
                    }while(reponse[taille++]!=0x00);
                    taille--;
                    for (i = 3; i < taille - 2; i++)
                    {
                        tempo[j++] = (char)reponse[i];
                    }
                    sPosition = new string(tempo);
                    position = Convert.ToInt32(sPosition);
                }
                else
                {
                    tempo[0] = (char)reponse[2];
                    tempo[1] = (char)reponse[3];
                    sErreur = new string(tempo);
                    codeErreur = Convert.ToInt32(sErreur,16);

                }
            }
            return codeErreur;
        }

        public bool allerEn(int distance, int speed, unite u)
        {
            String commande;
            bool retour = false;
            byte[] buffer = new byte[100];

            distance = (int) (6.5 * distance);//u
            init();
            start(mode.drive);
            speed = speed * (int) unite.kmh;
            if (m_port.IsOpen)
            {
                commande = "D,p" + distance.ToString() + "s" + speed.ToString() + "\r\n";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);
            }

            return retour;
        }



        public bool tourner(double angle)
        {
            String commande;
            bool retour = false;
            byte[] buffer = new byte[100];

            angle = (int)(angle * 9.1);
            //angle = angle * (int)unite.degre;
            //speed = speed * (int)unite.kmh;
            init();
            start(mode.turn);
            if (m_port.IsOpen)
            {
               // commande = "T,p" + angle.ToString() + "s" + speed.ToString() + "\r\n";
                commande = "T,p" + angle.ToString() + "s3000\r\n";//15000
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);
                retour = true;
            }
            return retour;


        }

        public bool powerdown(mode m)
        {
            String commande;
            bool retour = false;
            byte[] buffer = new byte[100];

            if (m_port.IsOpen)
            {
                commande = m.ToString() + ",powerdown\r\n";
                buffer = System.Text.Encoding.UTF8.GetBytes(commande);
                m_port.Write(buffer, 0, commande.Length);
                retour = true;
            }
            return retour;

        }
    }


}



