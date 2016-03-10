using System.ComponentModel;

namespace VMCTur.Domain.Entities.Enums
{
    public enum TipoVinculoGuia
    {
        [Description("Funcionário")]
        Funcionario = 1,

        [Description("Terceirizado")]
        Terceirizado = 2
    }
}
