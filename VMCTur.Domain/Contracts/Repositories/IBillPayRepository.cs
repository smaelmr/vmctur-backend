using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Financial.BillsPay;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IBillPayRepository : IDisposable
    {
        List<BillPay> Get(string search);
        List<BillPay> Get(DateTime startPeriod, DateTime finishPeriod);
        BillPay Get(int id);
        
        List<BillPay> GetOpenBills(DateTime startPeriod, DateTime finishPeriod);
        List<BillPay> GetOverdueBills();
        List<BillPay> GetToWinBills();
        List<BillPay> GetWinningTodayBills();        
        List<BillPay> GetPaidBills(DateTime startPeriod, DateTime finishPeriod);

        List<BillPay> Get(int skip, int take);
        
        void Update(BillPay bill);
        void Delete(BillPay bill);
    }
}
