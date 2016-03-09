using System.ComponentModel;

namespace VMCTur.Domain.Entities.Enum
{
    public enum TipoVinculoGuia
    {
        [Description("Funcionário")]
        Funcionario = 1,

        [Description("Terceirizado")]
        Terceirizado = 2
    }
}
