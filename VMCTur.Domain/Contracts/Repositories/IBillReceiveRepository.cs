using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Financial.BillsReceive;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IBillReceiveRepository : IDisposable
    {
        List<BillReceive> Get(string search);
        BillReceive Get(int id);
        List<BillReceive> Get(int skip, int take);

        List<BillReceive> GetOverdueBills();
        List<BillReceive> GetToWinBills();
        List<BillReceive> GetWinningTodayBills();
        List<BillReceive> GetReceivedBills();

        void Create(BillReceive cliente);
        void Update(BillReceive cliente);
        void Delete(BillReceive cliente);
    }
}
