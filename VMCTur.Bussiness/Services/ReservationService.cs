using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Commands.BillCommands.BillPayCommands;
using VMCTur.Domain.Commands.ReservationCommands;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Financial.BillsPay;
using VMCTur.Domain.Entities.Reservations;

namespace VMCTur.Bussiness.Services
{
    public class ReservationService : IReservationService
    {
        private IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository;
        }

        public void Create(CreateReservationCommand reserveCreate)
        {
            var reserve = new Reservation(
                0, null, reserveCreate.CustomerId, reserveCreate.DateReservation, reserveCreate.QuantityTickets, reserveCreate.DeparturePlace, 
                reserveCreate.Notification, reserveCreate.ContractNumber, reserveCreate.Status, new List<BillPay>());

            foreach (CreateBillPayCommand p in reserveCreate.Bills)
            {
                reserve.AddBillPay(new BillPay(0, DateTime.Now, p.ReservationId, p.Amount, 0, p.Concerning, p.DueDate, null, p.Comments));
            }

            reserve.Validate();

            _repository.Create(reserve);
        }

        public void Update(UpdateReservationCommand reserveUpdate)
        {
            ///Smael: busca o registro original.
            Reservation reserveOld = Get(reserveUpdate.Id);

            var reserve = new Reservation(
                0, null, reserveUpdate.CustomerId, reserveUpdate.DateReservation, reserveUpdate.QuantityTickets, reserveUpdate.DeparturePlace, 
                reserveUpdate.Notification, reserveUpdate.ContractNumber, reserveUpdate.Status, new List<BillPay>());

            #region Bills

            foreach (UpdateBillPayCommand p in reserveUpdate.Bills)
            {
                reserve.AddBillPay(new BillPay(p.Id, DateTime.Now, reserve.Id, p.Amount, 0, p.Concerning, p.DueDate, null, p.Comments));
            }

            #endregion

            reserve.Validate();

            _repository.Update(reserve, reserveOld);
        }

        public void Delete(int id)
        {
            var reserve = _repository.Get(id);

            _repository.Delete(reserve);
        }
        
        public Reservation Get(int id)
        {
           var reserve = _repository.Get(id);

            return reserve;
        }

        public List<Reservation> Get(string search)
        {
            var reserve = _repository.Get(search);

            return reserve;
        }

        public List<Reservation> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }        

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
