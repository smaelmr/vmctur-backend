using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands;
using VMCTur.Domain.Entities.Financial.BillsReceive;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IBillReceiveService : IDisposable
    {
        List<BillReceive> Get(string search);
        BillReceive Get(int id);
        List<BillReceive> Get(int skip, int take);

        List<BillReceive> GetOverdueBills();
        List<BillReceive> GetToWinBills();
        List<BillReceive> GetWinningTodayBills();
        List<BillReceive> GetReceivedBills();

        //void Create(CreateBillReceiveCommand bill);
        void Update(UpdateBillReceiveCommand bill);
        void Receipt(ReceiptBillReceiveCommand bill);
        void Delete(int id);
    }
}
