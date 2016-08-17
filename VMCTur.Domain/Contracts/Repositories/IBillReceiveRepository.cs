using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Financial.BillsReceive;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IBillReceiveRepository : IDisposable
    {
        List<BillReceive> Get(string search);
        List<BillReceive> Get(DateTime startPeriod, DateTime finishPeriod);
        BillReceive Get(int id);
        
        List<BillReceive> GetOpenBills(DateTime startPeriod, DateTime finishPeriod);
        List<BillReceive> GetOverdueBills();
        List<BillReceive> GetToWinBills();
        List<BillReceive> GetWinningTodayBills();        
        List<BillReceive> GetReceivedBills(DateTime startPeriod, DateTime finishPeriod);

        List<BillReceive> Get(int skip, int take);
        //void Create(BillReceive bill);
        //List<BillReceive> GetReceivedBills();

        void Update(BillReceive bill);
        void Delete(BillReceive bill);
    }
}
