using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Financial.BillsReceive;
using VMCTur.Infra.Conn;
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
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<BillReceive> bills = new List<BillReceive>();
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT BillReceive.Id, ");
            sql.Append("BillReceive.TravelPackageId, ");
            sql.Append("BillReceive.Amount, ");
            sql.Append("BillReceive.AmountReceived, ");
            sql.Append("BillReceive.Concerning, ");
            sql.Append("BillReceive.CreateDate, ");
            sql.Append("BillReceive.DueDate, ");
            sql.Append("BillReceive.PayDay, ");
            sql.Append("BillReceive.Comments, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM BillReceive ");
            sql.Append("INNER JOIN TravelPackage ON BillReceive.TravelPackageId = TravelPackage.Id ");
            sql.Append("INNER JOIN Customer ON TravelPackage.CustomerId = Customer.Id ");

            if (!string.IsNullOrEmpty(search))
            {
                sql.Append("WHERE Customer.Name LIKE @name ");
                cmm.Parameters.Add("@name", MySqlDbType.VarChar).Value = "%" + search + "%";
            }

            sql.Append("ORDER BY BillReceive.DueDate DESC;");

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {

                DateTime? auxPayDay = null;

                if (!dr.IsDBNull(dr.GetOrdinal("PayDay")))
                    auxPayDay = (DateTime)dr["PayDay"];

                BillReceive bill = new BillReceive(
                        (int)dr["Id"], 
                        (DateTime)dr["CreateDate"], 
                        (int)dr["TravelPackageId"],
                        (decimal)dr["Amount"], 
                        (decimal)dr["AmountReceived"], 
                        (string)dr["Concerning"], 
                        (DateTime)dr["DueDate"], 
                        auxPayDay,
                        dr.IsDBNull(dr.GetOrdinal("Comments")) ? "" : (string)dr["Comments"]);

                bill.SetCustomerName((string)dr["Name"]);

                bills.Add(bill);
            }

            dr.Close();

            return bills;
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
