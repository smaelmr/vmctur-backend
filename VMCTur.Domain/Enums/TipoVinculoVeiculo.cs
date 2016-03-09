using System.ComponentModel;

namespace VMCTur.Domain.Entities.Enum
{
    public enum TipoVinculoVeiculo
    {
        [Description("Próprio")]
        Proprio = 1,

        [Description("Terceirizado")]
        Terceirizado = 2        
    }
}
