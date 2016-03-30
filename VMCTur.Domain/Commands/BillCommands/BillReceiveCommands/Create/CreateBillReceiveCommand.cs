using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Commands.BillCommands.BillReceiveCommands.Create
{
    public class CreateBillReceiveCommand
    {
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public decimal Extra { get; set; }
        public decimal Discount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Comments { get; set; }

        public List<CreateReceiptCommand> Receipts { get; set; }
    }
}
