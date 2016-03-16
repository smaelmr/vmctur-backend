using System.ComponentModel;

namespace VMCTur.Domain.Entities.Enums
{
    public enum TypeAcquisition
    {
        [Description("Próprio")]
        Proprio = 1,

        [Description("Terceirizado")]
        Terceirizado = 2        
    }
}
