using Gadgeteer;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.SocketInterfaces;
using Microsoft.SPOT.Hardware;


namespace PR
{
    /// <summary>
    /// Jack sur entr�e num�rique
    /// </summary>
    class Jack
    {
        private readonly InputPort Entree;

        /// <summary>
        /// Etat de l'entr�e num�rique
        /// </summary>
        public bool Etat { get { return Entree.Read(); } }

        /// <summary>
        /// Initialise le jack
        /// </summary>
        /// <param name="numPort">Num�ro de port</param>
        /// <param name="numPin">Num�ro de pin</param>
        public Jack(int numPort, int numPin)
        {
            Entree = new InputPort(Socket.GetSocket(numPort, true, null, null).CpuPins[numPin], false, Port.ResistorMode.PullUp);
        }
    }
}
