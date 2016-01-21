using System.ComponentModel;

namespace VMCTur.Domain.Entities.Enum
{
    public enum TipoVinculoGuia
    {
        [Description("Funcionario")]
        Proprio,

        [Description("Terceirizado")]
        Terceirizado
    }
}
