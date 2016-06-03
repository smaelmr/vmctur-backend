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
        public string Comments { get; private set; }

        //Smael: get or set if tour is priavate or shared
        public bool Shared { get; private set; }

        //Smael: return just date of tha datetime
        public DateTime DateStart
        {
            get
            {
                return DateHourStart.Date;
            }
        }

        //Smael: return just time of the datetime
        public TimeSpan HourStart
        {
            get
            {
                TimeSpan ts = new TimeSpan(DateHourStart.Hour, DateHourStart.Minute, 0);

                return ts;
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

        public TravelPackageTour(int id, int tourId, int travelPackageId, DateTime dateHourStart, string comments, bool shared)
        {
            Id = id;
            TourId = tourId;
            TravelPackageId = travelPackageId;
            DateHourStart = dateHourStart;
            Comments = comments;
            Shared = shared;
        }

        #endregion

        #region Methods

        public void Validate()
        {            
            AssertionConcern.AssertIsGreaterThan(this.TourId, 0, "O passeio deve ser informado.");
            //AssertionConcern.AssertIsGreaterThan(this.TravelPackageId, 0, "O pacote deve ser informado.");
        }

        public void SetId(long id)
        {
            Id = Int32.Parse(id.ToString());
        }

        #endregion
    }
}
