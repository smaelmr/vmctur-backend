using System;
using VMCTur.Domain.Enums;

namespace VMCTur.Domain.Commands.TravelPackageCommands.Create
{
    public class CreateParticipantCommand
    {        
        public string Name { get; set; }
        public string NumberDocument { get; set; }
        public DateTime? BirthDate { get; set; }
        public string AgeGroupBelong { get; set; }
        public bool Paying { get; set; }
        public int TravelPackageId { get; set; }        
    }
}