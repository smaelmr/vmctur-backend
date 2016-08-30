using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.BillCommands.BillPayCommands;
using VMCTur.Domain.Entities.Financial.BillsPay;


namespace VMCTur.Domain.Contracts.Services
{
    public interface IBillPayService : IDisposable
    {
        List<BillPay> Get(string search);
        List<BillPay> GetAll(DateTime startPeriod, DateTime finishPeriod);
        BillPay Get(int id);
        List<BillPay> Get(int skip, int take);

        List<BillPay> GetOpenBills(DateTime startPeriod, DateTime finishPeriod);
        List<BillPay> GetOverdueBills();
        List<BillPay> GetToWinBills();
        List<BillPay> GetWinningTodayBills();        
        List<BillPay> GetPaidBills(DateTime startPeriod, DateTime finishPeriod);

        void Update(UpdateBillPayCommand bill);
        void Receipt(ReceiptBillPayCommand bill);
        void Delete(int id);
    }
}
