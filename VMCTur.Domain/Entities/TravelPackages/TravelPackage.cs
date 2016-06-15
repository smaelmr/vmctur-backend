using System;
using System.Collections;
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
        public virtual TourGuide GuideTour { get; private set; }
        public decimal TotalAmount { get; private set; }        

        public string AddictionalReservs { get; private set; }
        public string Comments { get; private set; }

        public int QuantityAdult { get; private set; }
        public int QuantityChild { get; private set; }
        public int QuantityEderly { get; private set; }

        public DateTime? ArrivalDate { get; private set; }
        public DateTime? LeaveDate { get; private set; }

        public decimal AmountForAdult { get; private set; }
        public decimal AmountForEderly { get; private set; }
        public decimal AmountForChild { get; private set; }

        public string DescServices { get; private set; }
        public string PayForms { get; private set; }

        public string CustomerName
        {
            get
            {
                return Customer.Name;
            }

        }

        #endregion

        #region Ctor

        public TravelPackage() { }

        public TravelPackage(int id, int companyId, int customerId, List<TravelPackageParticipant> participants,
                             List<TravelPackageTour> tours, List<BillReceive> bills, string host, int quantityTickets,
                             int vehicleUsedId, int guideTourId, decimal totalAmount, string addictionalReservs, string comments,
                             DateTime? arrivalDate, DateTime? leaveDate, decimal amountForAdult, decimal amountForEderly,
                             decimal amountForChild, string descServices, string payForms)
        {
            Id = id;
            CompanyId = companyId;
            CreationDate = DateTime.Now;
            CustomerId = customerId;
            Participants = participants;
            Tours = tours;
            Bills = bills;
            Host = host;
            QuantityTickets = quantityTickets;
            VehicleUsedId = vehicleUsedId;
            GuideTourId = guideTourId;
            TotalAmount = totalAmount;
            AddictionalReservs = addictionalReservs;
            Comments = comments;

            ArrivalDate = arrivalDate;
            LeaveDate = leaveDate;

            AmountForAdult = amountForAdult;
            AmountForEderly = amountForEderly;
            AmountForChild = amountForChild;
            DescServices = descServices;
            PayForms = payForms;
    }

        public TravelPackage(int id, int companyId, DateTime creationDate, int customerId, Customer customer, List<TravelPackageParticipant> participants,
                             List<TravelPackageTour> tours, List<BillReceive> bills, string host, int quantityTickets,
                             int vehicleUsedId, Vehicle vehicle, int guideTourId, TourGuide tourGuide, decimal totalAmount, 
                             string addictionalReservs, string comments, DateTime? arrivalDate, DateTime? leaveDate, 
                             decimal amountForAdult, decimal amountForEderly, decimal amountForChild, string descServices, string payForms)
        {
            Id = id;
            CompanyId = companyId;
            CreationDate = DateTime.Now;
            CustomerId = customerId;
            Customer = customer;
            Participants = participants;
            Tours = tours;
            Bills = bills;
            Host = host;
            QuantityTickets = quantityTickets;
            VehicleUsedId = vehicleUsedId;
            VehicleUsed = vehicle;
            GuideTourId = guideTourId;
            GuideTour = tourGuide;
            TotalAmount = totalAmount;
            AddictionalReservs = addictionalReservs;
            Comments = comments;

            ArrivalDate = arrivalDate;
            LeaveDate = leaveDate;

            AmountForAdult = amountForAdult;
            AmountForEderly = amountForEderly;
            AmountForChild = amountForChild;
            DescServices = descServices;
            PayForms = payForms;
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
            
            switch(participant.AgeGoupBelong)
            {
                
                case Domain.Enums.AgeGroup.Crianca:
                    QuantityChild++;
                    break;
                case Domain.Enums.AgeGroup.Idoso:
                    QuantityEderly++;
                    break;
                default:
                    QuantityAdult++;
                    break;

            }
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
