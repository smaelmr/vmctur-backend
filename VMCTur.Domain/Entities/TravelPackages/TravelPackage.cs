using System;
using System.Collections.Generic;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Customers;
using VMCTur.Domain.Entities.TourGuides;
using VMCTur.Domain.Entities.Vehicles;

namespace VMCTur.Domain.Entities.TravelPackages
{
    public class TravelPackage
    {
        #region Properties

        public int Id { get; private set; }
        public int CompanyId { get; private set; }
        public DateTime CreationDate { get; private set; }
        public Customer Customer { get; private set; }
        public int CustomerId { get; private set; }

        public List<TravelPackageParticipant> Participants { get; private set; }
        public List<TravelPackageTour> Tours { get; private set; }

        public DateTime DateHourStart { get; private set; }
        public string Host { get; private set; }
        public int QuantityTickets { get; private set; }

        public int VehicleUsedId { get; private set; }
        public virtual Vehicle VehicleUsed { get; private set; }

        public int GuideTourId { get; private set; }
        public TourGuide GuideTour { get; private set; }

        public double PaymentAmount { get; private set; }
        public DateTime PayDayFirst { get; private set; }
        public double PaymentFirst { get; private set; }
        public string PaymentTermsRemaining { get; private set; }
        public string AddictionalReservs { get; private set; }
        public string Comments { get; private set; }

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

        #endregion

        #region Ctor

        protected TravelPackage() { }

        public TravelPackage(int id, int companyId, int customerId,  
                        List<TravelPackageParticipant> participants, List<TravelPackageTour> tours,
                        DateTime dateHourStart, string host, int quantityTickets,
                        int vehicleUsedId, int guideTourId, double paymentAmount, DateTime payDayFirst,
                        double paymentFirst, string paymentTermsRemaining, string addictionalReservs, string comments)
        {
            Id = id;
            CompanyId = companyId;
            CreationDate = DateTime.Now;
            CustomerId = customerId;
            Participants = participants;
            Tours = tours;
            DateHourStart = dateHourStart;
            Host = host;
            QuantityTickets = quantityTickets;
            VehicleUsedId = vehicleUsedId;
            GuideTourId = guideTourId;
            PaymentAmount = paymentAmount;
            PayDayFirst = payDayFirst;
            PaymentFirst = paymentFirst;
            PaymentTermsRemaining = paymentTermsRemaining;
            AddictionalReservs = addictionalReservs;
            Comments = comments;
    }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertIsGreaterThan(CustomerId, 0, "Cliente inválido.");
            AssertionConcern.AssertIsGreaterThan(VehicleUsedId, 0, "Veículo inválido.");
            AssertionConcern.AssertIsGreaterThan(GuideTourId, 0, "Guia inválido.");
        }

        public void AddParticipante(TravelPackageParticipant participant)
        {
            participant.Validate();

            Participants.Add(participant);
        }
    

        #endregion
    }
}
