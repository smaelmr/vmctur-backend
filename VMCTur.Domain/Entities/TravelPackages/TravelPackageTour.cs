using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Tours;

namespace VMCTur.Domain.Entities.TravelPackages
{
    public class TravelPackageTour
    {
        #region Properties

        public int Id { get; private set; }

        public int TourId { get; private set; }
        public Tour Tour { get; private set; }
        public DateTime DateHourStart { get; private set; }
        public int TravelPackageId { get; private set; }
        public TravelPackage TravelPackage { get; private set; }

        //Smael: return just date of tha datetime
        public DateTime DateStart
        {
            get
            {
                return DateHourStart.Date;
            }
        }

        //Smael: return just time of tha datetime
        public TimeSpan HourStart
        {
            get
            {
                return new TimeSpan(DateHourStart.Hour, DateHourStart.Minute, 0);
            }
        }

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

        public TravelPackageTour(int id, int tourId, int travelPackageId, DateTime dateHourStart)
        {
            TourId = tourId;
            TravelPackageId = travelPackageId;
            DateHourStart = dateHourStart;
        }

        #endregion

        #region Methods

        public void Validate()
        {            
            AssertionConcern.AssertIsGreaterThan(this.TourId, 0, "O passeio deve ser informado.");
            AssertionConcern.AssertIsGreaterThan(this.TravelPackageId, 0, "O pacote deve ser informado.");
        }

        #endregion
    }
}
