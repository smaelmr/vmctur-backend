using System;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Create
{
    public class CreateParticipantCommand
    {        
        public string Name { get; set; }
        public string NumberDocument { get; set; }
        public DateTime BirthDate { get; set; }
        public int TravelPackageId { get; set; }        
    }
}