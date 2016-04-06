using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Financial.BillsReceive;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class BillReceiveRepository : IBillReceiveRepository
    {
        private AppDataContext _context;

        public BillReceiveRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(BillReceive bill)
        {
            _context.BillReceives.Add(bill);
            _context.SaveChanges();
        }

        public void Update(BillReceive bill)
        {
            _context.Entry<BillReceive>(bill).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(BillReceive bill)
        {
            _context.BillReceives.Remove(bill);
            _context.SaveChanges();
        }        

        public BillReceive Get(int id)
        {
            return _context.BillReceives.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<BillReceive> Get(string search)
        {
            return _context.BillReceives.Where(x => x.Comments.Contains(search)).ToList();
        }

        public List<BillReceive> Get(int skip, int take)
        {
            return _context.BillReceives.OrderBy(x => x.DueDate).Skip(skip).Take(take).ToList();
        }

        public List<BillReceive> GetOverdueBills()
        {
            return (from itens in _context.BillReceives
                        where (itens.PayDay == null && itens.DueDate < DateTime.Today)
                        orderby itens.DueDate ascending
                    select itens).ToList<BillReceive>();
        }

        public List<BillReceive> GetReceivedBills()
        {
            return (from itens in _context.BillReceives
                    where (itens.PayDay != null)
                    orderby itens.DueDate ascending
                    select itens).ToList<BillReceive>();
        }

        public List<BillReceive> GetToWinBills()
        {
            return (from itens in _context.BillReceives
                    where (itens.PayDay == null && itens.DueDate >= DateTime.Today)
                    orderby itens.DueDate ascending
                    select itens).ToList<BillReceive>();
        }

        public List<BillReceive> GetWinningTodayBills()
        {
            return (from itens in _context.BillReceives
                    where (itens.PayDay == null && itens.DueDate == DateTime.Today)
                    orderby itens.DueDate ascending
                    select itens).ToList<BillReceive>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
