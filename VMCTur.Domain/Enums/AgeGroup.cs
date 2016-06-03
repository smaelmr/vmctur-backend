using System.ComponentModel;

namespace VMCTur.Domain.Enums
{
    public enum AgeGroup
    {
        [Description("Criança")]
        Crianca = 1,

        [Description("Adulto")]
        Adulto = 2,

        [Description("Idoso")]
        Idoso = 3
    }
}
