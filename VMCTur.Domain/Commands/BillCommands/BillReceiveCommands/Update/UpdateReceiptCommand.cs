using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Commands.BillCommands.BillReceiveCommands.Update
{
    public class UpdateReceiptCommand
    {
        public int id { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCreate { get; set; }
        public int BillReceiveId { get; set; }
        public string Comments { get; set; }
    }
}
