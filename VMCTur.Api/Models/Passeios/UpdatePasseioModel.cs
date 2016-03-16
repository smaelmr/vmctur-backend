namespace VMCTur.Api.Models.Tours
{
    public class UpdatePasseioModel
    {     
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public string OpenHour { get; set; }
        public string CloseHour { get; set; }
        public bool Inactive { get; set; }
        public string Comments { get; set; }
    }
}