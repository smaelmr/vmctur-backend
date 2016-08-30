using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Create
{
    public class CreateTravelPackageCommand
    {
        public int CompanyId { get; set; }
        public int CustomerId { get; set; }

        public List<CreateParticipantCommand> Participants { get; set; }
        public List<CreateTourCommand> Tours { get; set; }
        public List<CreateBillReceiveCommand> Bills { get; set; }

        public string Host { get; set; }
        
        public decimal TotalAmount { get; set; }
        public string AddictionalReservs { get; set; }
        public string Comments { get; set; }

        public DateTime? ArrivalDate { get; set; }
        public DateTime? LeaveDate { get; set; }

        public decimal AmountForAdult { get; set; }
        public decimal AmountForElderly { get; set; }
        public decimal AmountForChild { get; set; }

        public string DescServices { get; set; }
        public string PayForms { get; set; }
    }
}