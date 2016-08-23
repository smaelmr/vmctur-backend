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
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            BillReceive bill = null;
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
            sql.Append("WHERE BillReceive.Id = @id ");

            cmm.Parameters.Add("Id", MySqlDbType.Int32).Value = id;

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            if (dr.HasRows)
            {
                dr.Read();

                DateTime? auxPayDay = null;

                if (!dr.IsDBNull(dr.GetOrdinal("PayDay")))
                    auxPayDay = (DateTime)dr["PayDay"];

                bill = new BillReceive(
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

            }

            dr.Close();

            return bill;
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

            sql.Append("ORDER BY BillReceive.DueDate ASC;");

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

        public List<BillReceive> Get(DateTime startPeriod, DateTime finishPeriod)
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

            sql.Append("WHERE BillReceive.DueDate BETWEEN @startPeriod AND @finishPeriod ");            

            sql.Append("ORDER BY BillReceive.DueDate ASC;");

            cmm.Parameters.Add("@startPeriod", MySqlDbType.DateTime).Value = new DateTime(startPeriod.Year, startPeriod.Month, startPeriod.Day, 0, 0, 0);
            cmm.Parameters.Add("@finishPeriod", MySqlDbType.DateTime).Value = new DateTime(finishPeriod.Year, finishPeriod.Month, finishPeriod.Day, 23, 59, 59);

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

        public List<BillReceive> GetOpenBills(DateTime startPeriod, DateTime finishPeriod)
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

            sql.Append("WHERE BillReceive.PayDay IS NULL ");
            sql.Append("AND BillReceive.DueDate BETWEEN @startPeriod AND @finishPeriod ");

            cmm.Parameters.Add("@startPeriod", MySqlDbType.DateTime).Value = new DateTime(startPeriod.Year, startPeriod.Month, startPeriod.Day, 0, 0, 0);
            cmm.Parameters.Add("@finishPeriod", MySqlDbType.DateTime).Value = new DateTime(finishPeriod.Year, finishPeriod.Month, finishPeriod.Day, 23, 59, 59);


            sql.Append("ORDER BY BillReceive.DueDate ASC;");

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

        public List<BillReceive> GetOverdueBills()
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

            sql.Append("WHERE BillReceive.PayDay IS NULL AND BillReceive.DueDate < @today ");

            cmm.Parameters.Add("@today", MySqlDbType.DateTime).Value = DateTime.Today;


            sql.Append("ORDER BY BillReceive.DueDate ASC;");

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
        
        public List<BillReceive> GetToWinBills()
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

            sql.Append("WHERE BillReceive.PayDay IS NULL AND BillReceive.DueDate >= @today ");

            cmm.Parameters.Add("@today", MySqlDbType.DateTime).Value = DateTime.Today;


            sql.Append("ORDER BY BillReceive.DueDate ASC;");

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

        public List<BillReceive> GetWinningTodayBills()
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

            sql.Append("WHERE BillReceive.PayDay IS NULL AND BillReceive.DueDate = @today ");
            
            cmm.Parameters.Add("@today", MySqlDbType.DateTime).Value = DateTime.Today;            


            sql.Append("ORDER BY BillReceive.DueDate ASC;");

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

        /// <summary>
        /// Smael: get only the bills receveds in the period.
        /// </summary>
        /// <param name="startPeriod"></param>
        /// <param name="finishPeriod"></param>
        /// <returns></returns>
        public List<BillReceive> GetReceivedBills(DateTime startPeriod, DateTime finishPeriod)
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

            sql.Append("WHERE BillReceive.PayDay IS NOT NULL ");
            sql.Append("AND BillReceive.PayDay BETWEEN @startPeriod AND @finishPeriod ");

            sql.Append("ORDER BY BillReceive.PayDay DESC;");

            cmm.Parameters.Add("@startPeriod", MySqlDbType.DateTime).Value = new DateTime(startPeriod.Year, startPeriod.Month, startPeriod.Day, 0, 0, 0);
            cmm.Parameters.Add("@finishPeriod", MySqlDbType.DateTime).Value = new DateTime(finishPeriod.Year, finishPeriod.Month, finishPeriod.Day, 23, 59, 59);

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {               
                BillReceive bill = new BillReceive(
                        (int)dr["Id"],
                        (DateTime)dr["CreateDate"],
                        (int)dr["TravelPackageId"],
                        (decimal)dr["Amount"],
                        (decimal)dr["AmountReceived"],
                        (string)dr["Concerning"],
                        (DateTime)dr["DueDate"],
                        (DateTime)dr["PayDay"],
                        dr.IsDBNull(dr.GetOrdinal("Comments")) ? "" : (string)dr["Comments"]);

                bill.SetCustomerName((string)dr["Name"]);

                bills.Add(bill);
            }

            dr.Close();

            return bills;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<BillReceive> Get(int skip, int take)
        {
            return _context.BillReceives.OrderBy(x => x.DueDate).Skip(skip).Take(take).ToList();
        }

        /// <summary>
        /// Smael: get all bills receveds.
        /// </summary>        
        /// <returns></returns>
        //public List<BillReceive> GetReceivedBills()
        //{
        //    StringBuilder sql = new StringBuilder();
        //    MySqlConn ctx = MySqlConn.GetInstancia();
        //    List<BillReceive> bills = new List<BillReceive>();
        //    MySqlCommand cmm = new MySqlCommand();

        //    sql.Append("SELECT BillReceive.Id, ");
        //    sql.Append("BillReceive.TravelPackageId, ");
        //    sql.Append("BillReceive.Amount, ");
        //    sql.Append("BillReceive.AmountReceived, ");
        //    sql.Append("BillReceive.Concerning, ");
        //    sql.Append("BillReceive.CreateDate, ");
        //    sql.Append("BillReceive.DueDate, ");
        //    sql.Append("BillReceive.PayDay, ");
        //    sql.Append("BillReceive.Comments, ");
        //    sql.Append("Customer.Name ");
        //    sql.Append("FROM BillReceive ");
        //    sql.Append("INNER JOIN TravelPackage ON BillReceive.TravelPackageId = TravelPackage.Id ");
        //    sql.Append("INNER JOIN Customer ON TravelPackage.CustomerId = Customer.Id ");

        //    sql.Append("WHERE BillReceive.PayDay IS NOT NULL ");

        //    sql.Append("ORDER BY BillReceive.PayDay DESC;");

        //    cmm.CommandText = sql.ToString();

        //    MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

        //    while (dr.Read())
        //    {

        //        BillReceive bill = new BillReceive(
        //                (int)dr["Id"],
        //                (DateTime)dr["CreateDate"],
        //                (int)dr["TravelPackageId"],
        //                (decimal)dr["Amount"],
        //                (decimal)dr["AmountReceived"],
        //                (string)dr["Concerning"],
        //                (DateTime)dr["DueDate"],
        //                (DateTime)dr["PayDay"],
        //                dr.IsDBNull(dr.GetOrdinal("Comments")) ? "" : (string)dr["Comments"]);

        //        bill.SetCustomerName((string)dr["Name"]);

        //        bills.Add(bill);
        //    }

        //    dr.Close();

        //    return bills;
        //}
    }
}
