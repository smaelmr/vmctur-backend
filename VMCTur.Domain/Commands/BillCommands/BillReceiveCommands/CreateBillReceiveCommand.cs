using System;

namespace VMCTur.Domain.Commands.BillCommands.BillReceiveCommands
{
    public class CreateBillReceiveCommand
    {
        public int TravelPackageId { get; set; }
        public decimal Amount { get; set; }
        public string Concerning { get; set; }       
        public DateTime DueDate { get; set; }        
        public string Comments { get; set; }
    }
}
