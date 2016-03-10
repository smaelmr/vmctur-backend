using System;
using System.Collections.Generic;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Create
{
    public class CreateTravelPackageCommand
    {
        public int CompanyId { get; private set; }
        public int CustomerId { get; private set; }

        public List<CreateParticipantCommand> Participants { get; private set; }
        public List<CreateTourCommand> Tours { get; private set; }

        public DateTime DateHourStart { get; private set; }
        public string Host { get; private set; }
        public string QuantityTickets { get; private set; }

        public int VehicleUsedId { get; private set; }
        public int GuideTourId { get; private set; }

        public double PaymentAmount { get; private set; }
        public DateTime PayDayFirst { get; private set; }
        public double PaymentFirst { get; private set; }
        public string PaymentTermsRemaining { get; private set; }
        public string AddictionalReservs { get; private set; }
        public string Comments { get; private set; }
    }
}