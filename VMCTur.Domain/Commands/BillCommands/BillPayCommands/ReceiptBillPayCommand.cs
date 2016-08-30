using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Commands.BillCommands.BillPayCommands
{
    public class ReceiptBillPayCommand
    {
        public int Id { get; set; }
        public DateTime PayDay { get; set; }
        public decimal AmountPaid { get; set; }
        public string Comments { get; set; }

    }
}
