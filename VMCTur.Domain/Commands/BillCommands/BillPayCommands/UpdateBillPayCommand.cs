using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Commands.BillCommands.BillPayCommands
{
    public class UpdateBillPayCommand
    {
        public int Id { get; set; }        
        public int ReservationId { get; set; }
        public decimal Amount { get; set; }    
        public string Concerning { get; set; }        
        public DateTime DueDate { get; set; }       
        public string Comments { get; set; }
    }
}
