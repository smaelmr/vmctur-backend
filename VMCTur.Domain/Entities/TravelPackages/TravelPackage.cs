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
        //public int QuantityTickets { get; private set; }
        
        public decimal TotalAmount { get; private set; }        

        public string AddictionalReservs { get; private set; }
        public string Comments { get; private set; }

        public int QuantityAdult { get; private set; }
        public int QuantityChild { get; private set; }
        public int QuantityElderly { get; private set; }

        public DateTime? ArrivalDate { get; private set; }
        public DateTime? LeaveDate { get; private set; }

        public decimal AmountForAdult { get; private set; }
        public decimal AmountForElderly { get; private set; }
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
                             List<TravelPackageTour> tours, List<BillReceive> bills, string host,
                             decimal totalAmount, string addictionalReservs, string comments, DateTime? arrivalDate, 
                             DateTime? leaveDate, decimal amountForAdult, decimal amountForElderly, decimal amountForChild, 
                             string descServices, string payForms, DateTime creationDate)
        {
            Id = id;
            CompanyId = companyId;
            CreationDate = creationDate;
            CustomerId = customerId;
            Participants = participants;
            Tours = tours;
            Bills = bills;
            Host = host;
            TotalAmount = totalAmount;
            AddictionalReservs = addictionalReservs;
            Comments = comments;

            ArrivalDate = arrivalDate;
            LeaveDate = leaveDate;

            AmountForAdult = amountForAdult;
            AmountForElderly = amountForElderly;
            AmountForChild = amountForChild;
            DescServices = descServices;
            PayForms = payForms;
    }

        public TravelPackage(int id, int companyId, DateTime creationDate, int customerId, 
                             Customer customer, List<TravelPackageParticipant> participants, List<TravelPackageTour> tours, List<BillReceive> bills, 
                             string host, decimal totalAmount, string addictionalReservs, 
                             string comments, DateTime? arrivalDate, DateTime? leaveDate, decimal amountForAdult, 
                             decimal amountForElderly, decimal amountForChild, string descServices, string payForms)
        {
            Id = id;
            CompanyId = companyId;
            CreationDate = creationDate;
            CustomerId = customerId;
            Customer = customer;
            Participants = participants;
            Tours = tours;
            Bills = bills;
            Host = host;
            TotalAmount = totalAmount;
            AddictionalReservs = addictionalReservs;
            Comments = comments;

            ArrivalDate = arrivalDate;
            LeaveDate = leaveDate;

            AmountForAdult = amountForAdult;
            AmountForElderly = amountForElderly;
            AmountForChild = amountForChild;
            DescServices = descServices;
            PayForms = payForms;
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertIsGreaterThan(CustomerId, 0, "Cliente inválido.");            
        }

        public void AddParticipant(TravelPackageParticipant participant)
        {
            participant.Validate();
            
            Participants.Add(participant);
            
            switch(participant.AgeGroupBelong)
            {
                
                case Domain.Enums.AgeGroup.Crianca:
                    QuantityChild++;
                    break;
                case Domain.Enums.AgeGroup.Idoso:
                    QuantityElderly++;
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
