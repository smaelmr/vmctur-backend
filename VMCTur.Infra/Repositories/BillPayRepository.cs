using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Financial.BillsPay;
using VMCTur.Infra.Conn;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class BillPayRepository : IBillPayRepository
    {
        private AppDataContext _context;

        public BillPayRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(BillPay bill)
        {
            _context.BillPays.Add(bill);
            _context.SaveChanges();
        }

        public void Update(BillPay bill)
        {
            _context.Entry<BillPay>(bill).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(BillPay bill)
        {
            _context.BillPays.Remove(bill);
            _context.SaveChanges();
        }

        public BillPay Get(int id)
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            BillPay bill = null;
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT BillPay.Id, ");
            sql.Append("BillPay.ReservationId, ");
            sql.Append("BillPay.Amount, ");
            sql.Append("BillPay.AmountPaid, ");
            sql.Append("BillPay.Concerning, ");
            sql.Append("BillPay.CreateDate, ");
            sql.Append("BillPay.DueDate, ");
            sql.Append("BillPay.PayDay, ");
            sql.Append("BillPay.Comments, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM BillPay ");
            sql.Append("INNER JOIN Reservation ON BillPay.ReservationId = Reservation.Id ");
            sql.Append("INNER JOIN Customer ON Reservation.CustomerId = Customer.Id ");
            sql.Append("WHERE BillPay.Id = @id ");

            cmm.Parameters.Add("Id", MySqlDbType.Int32).Value = id;

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            if (dr.HasRows)
            {
                dr.Read();

                DateTime? auxPayDay = null;

                if (!dr.IsDBNull(dr.GetOrdinal("PayDay")))
                    auxPayDay = (DateTime)dr["PayDay"];

                bill = new BillPay(
                        (int)dr["Id"],
                        (DateTime)dr["CreateDate"],
                        (int)dr["ReservationId"],
                        (decimal)dr["Amount"],
                        (decimal)dr["AmountPaid"],
                        (string)dr["Concerning"],
                        (DateTime)dr["DueDate"],
                        auxPayDay,
                        dr.IsDBNull(dr.GetOrdinal("Comments")) ? "" : (string)dr["Comments"]);

                bill.SetCustomerName((string)dr["Name"]);

            }

            dr.Close();

            return bill;
        }

        public List<BillPay> Get(string search)
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<BillPay> bills = new List<BillPay>();
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT BillPay.Id, ");
            sql.Append("BillPay.ReservationId, ");
            sql.Append("BillPay.Amount, ");
            sql.Append("BillPay.AmountPaid, ");
            sql.Append("BillPay.Concerning, ");
            sql.Append("BillPay.CreateDate, ");
            sql.Append("BillPay.DueDate, ");
            sql.Append("BillPay.PayDay, ");
            sql.Append("BillPay.Comments, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM BillPay ");
            sql.Append("INNER JOIN Reservation ON BillPay.ReservationId = Reservation.Id ");
            sql.Append("INNER JOIN Customer ON Reservation.CustomerId = Customer.Id ");

            if (!string.IsNullOrEmpty(search))
            {
                sql.Append("WHERE Customer.Name LIKE @name ");
                cmm.Parameters.Add("@name", MySqlDbType.VarChar).Value = "%" + search + "%";
            }

            sql.Append("ORDER BY BillPay.DueDate ASC;");

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {

                DateTime? auxPayDay = null;

                if (!dr.IsDBNull(dr.GetOrdinal("PayDay")))
                    auxPayDay = (DateTime)dr["PayDay"];

                BillPay bill = new BillPay(
                        (int)dr["Id"], 
                        (DateTime)dr["CreateDate"], 
                        (int)dr["ReservationId"],
                        (decimal)dr["Amount"], 
                        (decimal)dr["AmountPaid"], 
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

        public List<BillPay> Get(DateTime startPeriod, DateTime finishPeriod)
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<BillPay> bills = new List<BillPay>();
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT BillPay.Id, ");
            sql.Append("BillPay.ReservationId, ");
            sql.Append("BillPay.Amount, ");
            sql.Append("BillPay.AmountPaid, ");
            sql.Append("BillPay.Concerning, ");
            sql.Append("BillPay.CreateDate, ");
            sql.Append("BillPay.DueDate, ");
            sql.Append("BillPay.PayDay, ");
            sql.Append("BillPay.Comments, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM BillPay ");
            sql.Append("INNER JOIN Reservation ON BillPay.ReservationId = Reservation.Id ");
            sql.Append("INNER JOIN Customer ON Reservation.CustomerId = Customer.Id ");

            sql.Append("WHERE BillPay.DueDate BETWEEN @startPeriod AND @finishPeriod ");            

            sql.Append("ORDER BY BillPay.DueDate ASC;");

            cmm.Parameters.Add("@startPeriod", MySqlDbType.DateTime).Value = new DateTime(startPeriod.Year, startPeriod.Month, startPeriod.Day, 0, 0, 0);
            cmm.Parameters.Add("@finishPeriod", MySqlDbType.DateTime).Value = new DateTime(finishPeriod.Year, finishPeriod.Month, finishPeriod.Day, 23, 59, 59);

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {

                DateTime? auxPayDay = null;

                if (!dr.IsDBNull(dr.GetOrdinal("PayDay")))
                    auxPayDay = (DateTime)dr["PayDay"];

                BillPay bill = new BillPay(
                        (int)dr["Id"],
                        (DateTime)dr["CreateDate"],
                        (int)dr["ReservationId"],
                        (decimal)dr["Amount"],
                        (decimal)dr["AmountPaid"],
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

        public List<BillPay> GetOpenBills(DateTime startPeriod, DateTime finishPeriod)
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<BillPay> bills = new List<BillPay>();
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT BillPay.Id, ");
            sql.Append("BillPay.ReservationId, ");
            sql.Append("BillPay.Amount, ");
            sql.Append("BillPay.AmountPaid, ");
            sql.Append("BillPay.Concerning, ");
            sql.Append("BillPay.CreateDate, ");
            sql.Append("BillPay.DueDate, ");
            sql.Append("BillPay.PayDay, ");
            sql.Append("BillPay.Comments, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM BillPay ");
            sql.Append("INNER JOIN Reservation ON BillPay.ReservationId = Reservation.Id ");
            sql.Append("INNER JOIN Customer ON Reservation.CustomerId = Customer.Id ");

            sql.Append("WHERE BillPay.PayDay IS NULL ");
            sql.Append("AND BillPay.DueDate BETWEEN @startPeriod AND @finishPeriod ");

            cmm.Parameters.Add("@startPeriod", MySqlDbType.DateTime).Value = new DateTime(startPeriod.Year, startPeriod.Month, startPeriod.Day, 0, 0, 0);
            cmm.Parameters.Add("@finishPeriod", MySqlDbType.DateTime).Value = new DateTime(finishPeriod.Year, finishPeriod.Month, finishPeriod.Day, 23, 59, 59);


            sql.Append("ORDER BY BillPay.DueDate ASC;");

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {

                DateTime? auxPayDay = null;

                if (!dr.IsDBNull(dr.GetOrdinal("PayDay")))
                    auxPayDay = (DateTime)dr["PayDay"];

                BillPay bill = new BillPay(
                        (int)dr["Id"],
                        (DateTime)dr["CreateDate"],
                        (int)dr["ReservationId"],
                        (decimal)dr["Amount"],
                        (decimal)dr["AmountPaid"],
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

        public List<BillPay> GetOverdueBills()
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<BillPay> bills = new List<BillPay>();
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT BillPay.Id, ");
            sql.Append("BillPay.ReservationId, ");
            sql.Append("BillPay.Amount, ");
            sql.Append("BillPay.AmountPaid, ");
            sql.Append("BillPay.Concerning, ");
            sql.Append("BillPay.CreateDate, ");
            sql.Append("BillPay.DueDate, ");
            sql.Append("BillPay.PayDay, ");
            sql.Append("BillPay.Comments, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM BillPay ");
            sql.Append("INNER JOIN Reservation ON BillPay.ReservationId = Reservation.Id ");
            sql.Append("INNER JOIN Customer ON Reservation.CustomerId = Customer.Id ");

            sql.Append("WHERE BillPay.PayDay IS NULL AND BillPay.DueDate < @today ");

            cmm.Parameters.Add("@today", MySqlDbType.DateTime).Value = DateTime.Today;


            sql.Append("ORDER BY BillPay.DueDate ASC;");

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {

                DateTime? auxPayDay = null;

                if (!dr.IsDBNull(dr.GetOrdinal("PayDay")))
                    auxPayDay = (DateTime)dr["PayDay"];

                BillPay bill = new BillPay(
                        (int)dr["Id"],
                        (DateTime)dr["CreateDate"],
                        (int)dr["ReservationId"],
                        (decimal)dr["Amount"],
                        (decimal)dr["AmountPaid"],
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
        
        public List<BillPay> GetToWinBills()
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<BillPay> bills = new List<BillPay>();
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT BillPay.Id, ");
            sql.Append("BillPay.ReservationId, ");
            sql.Append("BillPay.Amount, ");
            sql.Append("BillPay.AmountPaid, ");
            sql.Append("BillPay.Concerning, ");
            sql.Append("BillPay.CreateDate, ");
            sql.Append("BillPay.DueDate, ");
            sql.Append("BillPay.PayDay, ");
            sql.Append("BillPay.Comments, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM BillPay ");
            sql.Append("INNER JOIN Reservation ON BillPay.ReservationId = Reservation.Id ");
            sql.Append("INNER JOIN Customer ON Reservation.CustomerId = Customer.Id ");            

            sql.Append("WHERE BillPay.PayDay IS NULL AND BillPay.DueDate >= @today ");

            cmm.Parameters.Add("@today", MySqlDbType.DateTime).Value = DateTime.Today;


            sql.Append("ORDER BY BillPay.DueDate ASC;");

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {

                DateTime? auxPayDay = null;

                if (!dr.IsDBNull(dr.GetOrdinal("PayDay")))
                    auxPayDay = (DateTime)dr["PayDay"];

                BillPay bill = new BillPay(
                        (int)dr["Id"],
                        (DateTime)dr["CreateDate"],
                        (int)dr["ReservationId"],
                        (decimal)dr["Amount"],
                        (decimal)dr["AmountPaid"],
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

        public List<BillPay> GetWinningTodayBills()
        {
            var query = from it in _context.BillPays
                        join reservation in _context.Reservations on it.ReservationId equals reservation.Id
                        join customer in _context.Customers on reservation.CustomerId equals customer.Id
                        where it.PayDay == null && it.DueDate == DateTime.Today
                        orderby customer.Name
                        select new
                        {
                            it.Id,
                            it.CreateDate,
                            it.ReservationId,
                            it.Amount,
                            it.AmountPaid,
                            it.Concerning,
                            customer.Name,
                            it.DueDate,
                            it.PayDay,
                            it.Comments
                        };

            //Smael: isso é feito para manter o set das propriedades privadas, pois para carregar o objeto direto na query as propriedades teriam quer ser publicas para que o valor fosse atribuido a elas.
            return query.ToList().Select(r => new BillPay(r.Id, r.CreateDate, r.ReservationId, r.Amount, r.AmountPaid, r.Concerning, r.Name, r.DueDate, r.PayDay, r.Comments)).ToList<BillPay>();

        }

        /// <summary>
        /// Smael: get only the bills paids in the period.
        /// </summary>
        /// <param name="startPeriod"></param>
        /// <param name="finishPeriod"></param>
        /// <returns></returns>
        public List<BillPay> GetPaidBills(DateTime startPeriod, DateTime finishPeriod)
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<BillPay> bills = new List<BillPay>();
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT BillPay.Id, ");
            sql.Append("BillPay.ReservationId, ");
            sql.Append("BillPay.Amount, ");
            sql.Append("BillPay.AmountPaid, ");
            sql.Append("BillPay.Concerning, ");
            sql.Append("BillPay.CreateDate, ");
            sql.Append("BillPay.DueDate, ");
            sql.Append("BillPay.PayDay, ");
            sql.Append("BillPay.Comments, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM BillPay ");
            sql.Append("INNER JOIN Reservation ON BillPay.ReservationId = Reservation.Id ");
            sql.Append("INNER JOIN Customer ON Reservation.CustomerId = Customer.Id ");

            sql.Append("WHERE BillPay.PayDay IS NOT NULL ");
            sql.Append("AND BillPay.PayDay BETWEEN @startPeriod AND @finishPeriod ");

            sql.Append("ORDER BY BillPay.PayDay DESC;");

            cmm.Parameters.Add("@startPeriod", MySqlDbType.DateTime).Value = new DateTime(startPeriod.Year, startPeriod.Month, startPeriod.Day, 0, 0, 0);
            cmm.Parameters.Add("@finishPeriod", MySqlDbType.DateTime).Value = new DateTime(finishPeriod.Year, finishPeriod.Month, finishPeriod.Day, 23, 59, 59);

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {               
                BillPay bill = new BillPay(
                        (int)dr["Id"],
                        (DateTime)dr["CreateDate"],
                        (int)dr["ReservationId"],
                        (decimal)dr["Amount"],
                        (decimal)dr["AmountPaid"],
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
        
        public List<BillPay> Get(int skip, int take)
        {
            return _context.BillPays.OrderBy(x => x.DueDate).Skip(skip).Take(take).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
