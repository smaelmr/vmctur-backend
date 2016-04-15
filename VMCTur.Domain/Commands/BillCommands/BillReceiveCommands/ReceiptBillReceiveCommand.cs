using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Commands.BillCommands.BillReceiveCommands
{
    public class ReceiptBillReceiveCommand
    {
        public int Id { get; set; }
        public DateTime PayDay { get; set; }
        public decimal AmountReceived { get; set; }
    }
}
