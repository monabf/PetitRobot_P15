
using PR.Membres;
namespace PR
{
    /// <summary>
    /// Configuration des ports d'un robot
    /// </summary>

    class ConfigurationPorts
    {
        public int IO;
        public int Jack;
        public int DetecteurIR;
        public int Plateforme;
        public int ContAX12;
        public CPince.configPince pince;
        public CPetitBras.configPetitBras petitBras;
        public CPoussoir.configPoussoir poussoir;
        public int InfrarougeAVD;
        public int InfrarougeAVG;
        public int InfrarougeARD;
        public int InfrarougeARG;
        public int CapteurUltrason;
    }

}
