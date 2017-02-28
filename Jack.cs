using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.SocketInterfaces;

namespace PetitRobot_V1
{
    /// <summary>
    /// Classe repr�sentant la prise jack pour le d�part du robot
    /// </summary>
    class Jack
    {
        private readonly DigitalInput Entree;

        /// <summary>
        /// Etat de l'entr�e num�rique
        /// </summary>
        public bool Etat { get { return Entree.Read(); } }

        /// <summary>
        /// Initialise le jack
        /// </summary>
        /// <param name="etendeur">Etendeur IO60P16</param>
        /// <param name="numPort">Num�ro de port</param>
        /// <param name="numPin">Num�ro de pin</param>
        public Jack(IO60P16 etendeur, int numPort, int numPin)
        {
            Entree = etendeur.CreateDigitalInput(numPort, numPin, GlitchFilterMode.On, ResistorMode.PullUp);
        }


    }
}
