using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Domain.Commands.VehicleCommands
{
    public class CreateVehicleCommand
    {
        public int CompanyId { get; set; }
        public string Plate { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int NumberOfPassengers { get; set; }
        public bool Inactive { get; set; }
        public string TypeAcquisition { get; set; }
        public string Comments { get; set; }
    }
}