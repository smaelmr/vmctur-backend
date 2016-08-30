using System;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Create
{
    public class CreateTourCommand
    {   
        public int TourId { get; set; }
        public int VehicleUsedId { get; set; }
        public int GuideTourId { get; set; }
        public DateTime DateStart { get; set; }
        public TimeSpan HourStart { get; set; }
        public int TravelPackageId { get; set; }
        public string Comments { get; set; } 
        public bool Shared { get; set; }
        public int QuantityTickets { get; set; }
        public string ContractNumber { get; set; }
    }
}