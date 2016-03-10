using System;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Update
{
    public class UpdateParticipantCommand
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string NumberDocument { get; private set; }
        public DateTime BirthDate { get; private set; }
        public int TravelPackageId { get; private set; }        
    }
}