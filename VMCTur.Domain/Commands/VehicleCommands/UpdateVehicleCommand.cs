using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Domain.Commands.VehicleCommands
{
    public class UpdateVehicleCommand
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Plate { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int NumberOfPassengers { get; set; }
        public bool Inactive { get; set; }
        public string AcquisitionType { get; set; }
        public string Comments { get; set; }
    }
}