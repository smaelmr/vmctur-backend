using System.ComponentModel;

namespace VMCTur.Domain.Enums
{
    public enum AgeGroup
    {
        [Description("Criança")]
        Crianca,

        [Description("Adulto")]
        Adulto,

        [Description("Idoso")]
        Idoso
    }
}
