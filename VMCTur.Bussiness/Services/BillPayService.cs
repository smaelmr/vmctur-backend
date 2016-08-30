using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.BillCommands.BillPayCommands;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Financial.BillsPay;

namespace VMCTur.Bussiness.Services
{
    public class BillPayService : IBillPayService
    {
        private IBillPayRepository _repository;

        public BillPayService(IBillPayRepository repository)
        {
            _repository = repository;
        }

        public void Update(UpdateBillPayCommand bill)
        {
            //Smael: carrega a conta original do banco de dados.
            BillPay currentbill = Get(bill.Id);

            BillPay billPay = new BillPay(
                bill.Id, currentbill.CreateDate, currentbill.ReservationId, 
                bill.Amount, 0, bill.Concerning,
                bill.DueDate, null, bill.Comments);

            billPay.Validate();

            _repository.Update(billPay);
        }

        public void Delete(int id)
        {
            BillPay billPay = _repository.Get(id);

            _repository.Delete(billPay);
        }

        public void Receipt(ReceiptBillPayCommand bill)
        {
            BillPay b = _repository.Get(bill.Id);

            b.Receipt(bill.PayDay, bill.AmountPaid, bill.Comments);

            b.Validate();

            _repository.Update(b);

        }

        public BillPay Get(int id)
        {
            return _repository.Get(id);
        }

        public List<BillPay> Get(string search)
        {
            return _repository.Get(search);
        }

        public List<BillPay> Get(int skip, int take)
        {
            return _repository.Get(skip, take);
        }
        
        public List<BillPay> GetAll(DateTime startPeriod, DateTime finishPeriod)
        {
            return _repository.Get(startPeriod, finishPeriod);
        }

        public List<BillPay> GetOverdueBills()
        {
            return _repository.GetOverdueBills();
        }

        public List<BillPay> GetToWinBills()
        {
            return _repository.GetToWinBills();
        }

        public List<BillPay> GetWinningTodayBills()
        {
            return _repository.GetWinningTodayBills();
        }

        public List<BillPay> GetPaidBills(DateTime startPeriod, DateTime finishPeriod)
        {
            return _repository.GetPaidBills(startPeriod, finishPeriod);
        }

        public List<BillPay> GetOpenBills(DateTime startPeriod, DateTime finishPeriod)
        {
            return _repository.GetOpenBills(startPeriod, finishPeriod);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
