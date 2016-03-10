using System;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Create
{
    public class CreateParticipantCommand
    {        
        public string Name { get; private set; }
        public string NumberDocument { get; private set; }
        public DateTime BirthDate { get; private set; }
        public int TravelPackageId { get; private set; }        
    }
}