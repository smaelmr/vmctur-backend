using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Customers;
using VMCTur.Domain.Entities.TourGuides;
using VMCTur.Domain.Entities.Tours;
using VMCTur.Domain.Entities.Vehicles;

namespace VMCTur.Domain.Entities.TravelPackages
{
    public class TravelPackage
    {
        #region Properties

        public int Id { get; private set; }
        public int CompanyId { get; private set; }

        public Customer Customer { get; private set; }
        public int CustomerId { get; private set; }

        public List<ParticipantTravelPackage> Participants { get; private set; }
        public List<TourTravelPackage> Tours { get; private set; }

        public DateTime DateHourStart { get; private set; }
        public string Host { get; private set; }
        public string QuantityTickets { get; private set; }

        public int VehicleUsedId { get; private set; }
        public Vehicle VehicleUsed { get; private set; }

        public int GuideTourId { get; private set; }
        public TourGuide GuideTour { get; private set; }

        public double PaymentAmount { get; private set; }
        public DateTime PayDayFirst { get; private set; }
        public double PaymentFirst { get; private set; }
        public string PaymentTermsRemaining { get; private set; }
        public string AddictionalReservs { get; private set; }
        public string Comments { get; private set; }

        #endregion

        #region Ctor

        protected TravelPackage() { }

        public TravelPackage(int id, int companyId, int customerId, List<ParticipantTravelPackage> participants, List<TourTravelPackage> tours,
                      DateTime dateHourStart, string host, string quantityTickets,
                      int vehicleUsedId, int guideTourId, double paymentAmount, DateTime payDayFirst,
                      double paymentFirst, string paymentTermsRemaining, string addictionalReservs, string comments)
        {
            Id = id;
            CompanyId = companyId;
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
            AssertionConcern.AssertIsLowerOrEqualThan(CustomerId, 0, "Cliente inválido.");
            AssertionConcern.AssertIsLowerOrEqualThan(VehicleUsedId, 0, "Veículo inválido.");
            AssertionConcern.AssertIsLowerOrEqualThan(GuideTourId, 0, "Guia inválido.");
        }

        public void AddParticipante(ParticipantTravelPackage participant)
        {
            participant.Validate();

            Participants.Add(participant);
        }
    

        #endregion
    }
}
