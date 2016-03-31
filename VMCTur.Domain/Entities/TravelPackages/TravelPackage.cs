using System;
using System.Collections.Generic;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Customers;
using VMCTur.Domain.Entities.Financial.BillsReceive;
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
        public List<BillReceive> Bills { get; private set; }

        public string Host { get; private set; }
        public int QuantityTickets { get; private set; }

        public int VehicleUsedId { get; private set; }
        public virtual Vehicle VehicleUsed { get; private set; }

        public int GuideTourId { get; private set; }
        public TourGuide GuideTour { get; private set; }
        public double TotalAmount { get; private set; }
        public string AddictionalReservs { get; private set; }
        public string Comments { get; private set; }

        //public DateTime PayDayFirst { get; private set; }
        //public double PaymentFirst { get; private set; }
        //public string PaymentTermsRemaining { get; private set; }


        #endregion

        #region Ctor

        protected TravelPackage() { }

        public TravelPackage(int id, int companyId, int customerId, List<TravelPackageParticipant> participants, 
                             List<TravelPackageTour> tours, List<BillReceive> Bills, string host, int quantityTickets, 
                             int vehicleUsedId, int guideTourId, double totalAmount, string addictionalReservs, string comments)
        {
            Id = id;
            CompanyId = companyId;
            CreationDate = DateTime.Now;
            CustomerId = customerId;
            Participants = participants;
            Tours = tours;            
            Host = host;
            QuantityTickets = quantityTickets;
            VehicleUsedId = vehicleUsedId;
            GuideTourId = guideTourId;
            TotalAmount = totalAmount;
            //PayDayFirst = payDayFirst;
            //PaymentFirst = paymentFirst;
            //PaymentTermsRemaining = paymentTermsRemaining;
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

        public void AddParticipant(TravelPackageParticipant participant)
        {
            participant.Validate();

            Participants.Add(participant);
        }

        public void AddTour(TravelPackageTour tour)
        {
            tour.Validate();

            Tours.Add(tour);
        }

        public void AddBillReceive(BillReceive bill)
        {
            bill.Validate();

            Bills.Add(bill);
        }

        #endregion
    }
}
