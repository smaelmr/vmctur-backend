using System;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Update
{
    public class UpdateTourCommand
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int TravelPackageId { get; set; }
        public DateTime DateStart { get; set; }
        public TimeSpan HourStart { get; set; }
        public string TourComments { get; set; }
        public bool Shared { get; private set; }

    }
}