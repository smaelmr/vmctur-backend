using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Enums
{
    public enum FaixaEtaria
    {
        [Description("Criança")]
        Crianca,

        [Description("Adulto")]
        Adulto,

        [Description("Idoso")]
        Idoso
    }
}
