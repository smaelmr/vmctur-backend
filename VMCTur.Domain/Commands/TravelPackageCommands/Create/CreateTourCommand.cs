using System;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Create
{
    public class CreateTourCommand
    {   
        public int TourId { get; set; }
        public DateTime DateStart { get; set; }
        public TimeSpan HourStart { get; set; }
        public int TravelPackageId { get; set; }
    }
}