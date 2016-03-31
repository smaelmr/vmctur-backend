using System.ComponentModel;

namespace VMCTur.Domain.Enums
{
    public enum BillStatus
    {
        [Description("Em Aberto")]
        EmAberto = 1,

        [Description("Quitado")]
        Quitado = 2,

        [Description("Em Atraso")]
        EmAtraso = 3,

        [Description("Vencendo Hoje")]
        VencendoHoje = 4
    }
}
