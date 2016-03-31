using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands.Create;

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
        public int QuantityTickets { get; set; }

        public int VehicleUsedId { get; set; }
        public int GuideTourId { get; set; }

        public double TotalAmount { get; set; }
        public DateTime PayDayFirst { get; set; }
        public double PaymentFirst { get; set; }
        public string PaymentTermsRemaining { get; set; }
        public string AddictionalReservs { get; set; }
        public string Comments { get; set; }
    }
}