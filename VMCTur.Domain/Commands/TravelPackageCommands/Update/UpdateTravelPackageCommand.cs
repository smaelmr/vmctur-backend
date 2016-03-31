using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands.Update;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Update
{
    public class UpdateTravelPackageCommand
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CustomerId { get; set; }

        public List<UpdateParticipantCommand> Participants { get; set; }
        public List<UpdateTourCommand> Tours { get; set; }
        public List<UpdateBillReceiveCommand> Bills { get; set; }

        public DateTime DateStart { get; set; }
        public TimeSpan HourStart { get; set; }

        public string HostLocal { get; set; }
        public int QuantityTickets { get; set; }

        public int VehicleUsedId { get; set; }
        public int GuideTourId { get; set; }

        public double PaymentAmount { get; set; }
        public DateTime PayDayFirst { get; set; }
        public double PaymentFirst { get; set; }
        public string PaymentTermsRemaining { get; set; }
        public string AddictionalReservs { get; set; }
        public string Comments { get; set; }
    }
}