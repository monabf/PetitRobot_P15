
using PR.Membres;
namespace PR
{
    /// <summary>
    /// Configuration des ports d'un robot
    /// </summary>

    public class ConfigurationPorts
    {
        public int idIO;
        public int idJack;
        public int idDetecteurIR;
        public int idBaseRoulante;
        public int idContAX12;
        public CPince.configPince pince;
        public CPetitBras.configPetitBras petitBras;
        public CPoussoir.configPoussoir poussoir;
        public int idInfrarougeAVD;
        public int idInfrarougeAVG;
        public int idInfrarougeARD;
        public int idInfrarougeARG;
        public int idCapteurUltrason;
    }

}
