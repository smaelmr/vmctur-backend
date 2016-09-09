using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Commands.BillCommands.BillPayCommands;

namespace VMCTur.Domain.Commands.ReservationCommands
{
    public class CreateReservationCommand
    {        
        public DateTime DateReservation { get; set; }
        public int QuantityTickets { get; set; }
        public int CustomerId { get; set; }
        public string DeparturePlace { get; set; }
        public string Notification { get; set; }
        public string ContractNumber { get; set; }
        public string Status { get; set; } 
        public List<CreateBillPayCommand> Bills { get; set; }
    }
}
