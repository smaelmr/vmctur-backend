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

        public DateTime DateHourTour { get; private set; }
        public string CustomerName { get; private set; }
        public int QuantityParticipants { get; private set; }
        public string TourNamePasseio { get; private set; }
        public string TourGuidename { get; private set; }
        public string VehicleModel { get; private set; }

        #endregion
    }
}
