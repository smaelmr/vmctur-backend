using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Update
{
    public class UpdateTravelPackageCommand
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CustomerId { get; set; }

        public DateTime CreationDate { get; set; }

        public List<UpdateParticipantCommand> Participants { get; set; }
        public List<UpdateTourCommand> Tours { get; set; }
        public List<UpdateBillReceiveCommand> Bills { get; set; }

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