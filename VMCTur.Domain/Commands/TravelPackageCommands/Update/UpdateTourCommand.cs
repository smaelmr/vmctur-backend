namespace VMCTur.Domain.Commands.TravelPackageCommands.Update
{
    public class UpdateTourCommand
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public int TravelPackageId { get; set; }
    }
}