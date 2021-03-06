﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Financial.BillsReceive;

namespace VMCTur.Bussiness.Services
{
    public class BillReceiveService : IBillReceiveService
    {
        private IBillReceiveRepository _repository;

        public BillReceiveService(IBillReceiveRepository repository)
        {
            _repository = repository;
        }

        //public void Create(CreateBillReceiveCommand bill)
        //{
        //    BillReceive billReceive = new BillReceive(
        //        0, DateTime.Now, bill.TravelPackageId, bill.Amount, 0, bill.Concerning, 
        //        bill.DueDate, null, bill.Comments);

        //    billReceive.Validate();

        //    _repository.Create(billReceive);
        //}

        public void Update(UpdateBillReceiveCommand bill)
        {
            //Smael: carrega a conta original do banco de dados.
            BillReceive currentbill = Get(bill.Id);

            BillReceive billReceive = new BillReceive(
                bill.Id, currentbill.CreateDate, currentbill.TravelPackageId, 
                bill.Amount, 0, bill.Concerning,
                bill.DueDate, null, bill.Comments);

            billReceive.Validate();

            _repository.Update(billReceive);
        }

        public void Delete(int id)
        {
            BillReceive billReceive = _repository.Get(id);

            _repository.Delete(billReceive);
        }

        public void Receipt(ReceiptBillReceiveCommand bill)
        {
            BillReceive b = _repository.Get(bill.Id);

            b.Receipt(bill.PayDay, bill.AmountReceived, bill.Comments);

            b.Validate();

            _repository.Update(b);

        }

        public BillReceive Get(int id)
        {
            return _repository.Get(id);
        }

        public List<BillReceive> Get(string search)
        {
            return _repository.Get(search);
        }

        public List<BillReceive> Get(int skip, int take)
        {
            return _repository.Get(skip, take);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public List<BillReceive> GetAll(DateTime startPeriod, DateTime finishPeriod)
        {
            return _repository.Get(startPeriod, finishPeriod);
        }

        public List<BillReceive> GetOverdueBills()
        {
            return _repository.GetOverdueBills();
        }

        public List<BillReceive> GetToWinBills()
        {
            return _repository.GetToWinBills();
        }

        public List<BillReceive> GetWinningTodayBills()
        {
            return _repository.GetWinningTodayBills();
        }

        public List<BillReceive> GetReceivedBills(DateTime startPeriod, DateTime finishPeriod)
        {
            return _repository.GetReceivedBills(startPeriod, finishPeriod);
        }

        public List<BillReceive> GetOpenBills(DateTime startPeriod, DateTime finishPeriod)
        {
            return _repository.GetOpenBills(startPeriod, finishPeriod);
        }
    }
}
