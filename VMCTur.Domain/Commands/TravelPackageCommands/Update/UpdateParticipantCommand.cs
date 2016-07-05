using System;
using VMCTur.Domain.Enums;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Update
{
    public class UpdateParticipantCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NumberDocument { get; set; }
        public DateTime? BirthDate { get; set; }
        public string AgeGroupBelong { get; set; }
        public bool Paying { get; private set; }
        public int TravelPackageId { get; set; }        
    }
}