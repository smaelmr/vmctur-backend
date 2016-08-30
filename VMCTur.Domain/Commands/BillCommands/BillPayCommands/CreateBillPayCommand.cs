using System;

namespace VMCTur.Domain.Commands.BillCommands.BillPayCommands
{
    public class CreateBillPayCommand
    {
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }
        public string Concerning { get; set; }       
        public DateTime DueDate { get; set; }        
        public string Comments { get; set; }
    }
}
