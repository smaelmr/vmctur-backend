using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMCTur.Domain.Entities.TravelPackages
{
    public class TourSchedule
    {
        #region Properties

        public DateTime DateHourTour { get; set; }
        public string CustomerName { get; set; }
        public int QuantityParticipants { get; set; }
        public string TourNamePasseio { get; set; }
        public string TourGuidename { get; set; }
        public string VehicleModel { get; set; }

        public TourSchedule() { }

        public TourSchedule(DateTime dateHourTour, string customerName, int quantityParticipants, string tourNamePasseio, string tourGuidename, string vehicleModel)
        {
            DateHourTour = dateHourTour;
            CustomerName = customerName;
            QuantityParticipants = quantityParticipants;
            TourNamePasseio = tourNamePasseio;
            TourGuidename = tourGuidename;
            VehicleModel = vehicleModel;
    }

        #endregion
    }
}
