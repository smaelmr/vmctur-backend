using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Customers;
using VMCTur.Domain.Entities.Financial.BillsPay;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Domain.Entities.Reservations
{
    public class Reservation
    {
        #region Properties

        public int Id { get; private set; }

        public Customer Customer { get; private set; }
        public int CustomerId { get; private set; }

        public DateTime DateReservation { get; private set; }
        public int QuantityTickets { get; private set; }
        public string DeparturePlace { get; private set; }  //local de partida (Bento ou Carlos Barbosa)

        public string Notification { get; private set; } //NOTIFICAÇÃO = campo string aberto para texto 

        public string ContractNumber { get; private set; }
        public string Status { get; private set; } //(Cancelado, efetivado)

        public string CustomerName
        {
            get
            {
                return Customer.Name;
            }

        }

        public List<BillPay> Bills { get; private set; }

        public List<TourSchedule> Tours { get; private set; }

        #endregion

        #region Ctor

        public Reservation()
        {
            Tours = new List<TourSchedule>();
        }

        public Reservation(int id, Customer customer, int customerId, DateTime dateReservation, int quantityTickets, string departurePlace, string notification, string contractNumber, string status, List<BillPay> bills)
        {
            Id = id;
            Customer = customer;
            CustomerId = customerId;
            DateReservation = dateReservation;
            QuantityTickets = quantityTickets;
            DeparturePlace = departurePlace;
            Notification = notification;
            ContractNumber = contractNumber;
            Status = status;
            Bills = bills;
    }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertIsGreaterThan(CustomerId, 0, "Cliente inválido.");
        }

        public void AddBillPay(BillPay bill)
        {
            bill.Validate();

            Bills.Add(bill);
        }

        public void AddTour(TourSchedule tour)
        {
            Tours.Add(tour);
        }

        #endregion
    }
}
