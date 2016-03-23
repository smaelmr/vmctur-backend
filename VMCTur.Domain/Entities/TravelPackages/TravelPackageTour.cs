using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Entities.Tours;

namespace VMCTur.Domain.Entities.TravelPackages
{
    public class TravelPackageTour
    {
        #region Properties

        public int Id { get; private set; }

        public int TourId { get; private set; }
        public Tour Tour { get; private set; }

        public int TravelPackageId { get; private set; }
        public TravelPackage TravelPackage { get; private set; }

        public string TourName
        {
            get
            {
                return Tour.Name;
            }
        }


        #endregion

        #region Ctor

        protected TravelPackageTour()
        { }

        public TravelPackageTour(int id, int tourId, int travelPackageId)
        {
            TourId = tourId;
            TravelPackageId = travelPackageId;
        }

        #endregion
    }
}
