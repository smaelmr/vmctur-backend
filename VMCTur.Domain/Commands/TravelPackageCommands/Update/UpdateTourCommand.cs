namespace VMCTur.Domain.Commands.TravelPackageCommands.Update
{
    public class UpdateTourCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Comments { get; set; }
    }
}