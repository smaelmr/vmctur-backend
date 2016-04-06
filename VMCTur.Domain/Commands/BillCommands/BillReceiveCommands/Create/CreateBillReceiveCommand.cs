using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Commands.BillCommands.BillReceiveCommands.Create
{
    public class CreateBillReceiveCommand
    {
        public int TravelPackageId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountReceived { get; set; }
        public string Concerning { get; set; }       
        public DateTime DueDate { get; set; }
        public DateTime PayDay { get; set; }
        public string Comments { get; set; }
    }
}
