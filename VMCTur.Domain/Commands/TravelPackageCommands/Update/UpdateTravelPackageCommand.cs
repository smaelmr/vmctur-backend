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

        public string Host { get; set; }
        public int QuantityTickets { get; set; }

        public int VehicleUsedId { get; set; }
        public int GuideTourId { get; set; }

        public double TotalAmount { get; set; }
        public string AddictionalReservs { get; set; }
        public string Comments { get; set; }
    }
}