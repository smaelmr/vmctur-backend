using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Customers;
using VMCTur.Domain.Entities.Financial.BillsPay;
using VMCTur.Domain.Entities.Reservations;
using VMCTur.Infra.Conn;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private AppDataContext _context;

        public ReservationRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(Reservation reserve)
        {
            _context.Reservations.Add(reserve);
            _context.SaveChanges();
        }

        public void Update(Reservation reserve, Reservation reserveOld)
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();

            sql.Append("UPDATE Reservation ");
            sql.Append("SET ");
            sql.Append("CustomerId = @CustomerId, ");
            sql.Append("DateReservation = @dateReservation, ");
            sql.Append("QuantityTickets = @QuantityTickets, ");
            sql.Append("DeparturePlace = @departurePlace, ");
            sql.Append("Notification = @notification, ");
            sql.Append("ContractNumber = @ContractNumber, ");
            sql.Append("Status = @status, ");

            sql.Append("WHERE Id = @Id; ");

            MySqlCommand cmm = new MySqlCommand(sql.ToString());

            cmm.Parameters.Add("@CustomerId", MySqlDbType.Int32).Value = reserve.CustomerId;
            cmm.Parameters.Add("@DateReservation", MySqlDbType.DateTime).Value = reserve.DateReservation;
            cmm.Parameters.Add("@QuantityTickets", MySqlDbType.Int32).Value = reserve.QuantityTickets;
            cmm.Parameters.Add("@DeparturePlace", MySqlDbType.Text).Value = reserve.DeparturePlace;
            cmm.Parameters.Add("@Notification", MySqlDbType.Text).Value = reserve.Notification;
            cmm.Parameters.Add("@ContractNumber", MySqlDbType.Decimal).Value = reserve.ContractNumber;
            cmm.Parameters.Add("@Status", MySqlDbType.Int32).Value = reserve.Status;
            
            cmm.Parameters.Add("@Id", MySqlDbType.Int32).Value = reserveOld.Id;

            ctx.BeginWork();

            try
            {
                ctx.ExecutaQuery(cmm);

                #region Bills

                #region Delete

                ///Smael: para cada item na lista old que não existe na lista new, exclui-se.
                if (reserveOld.Bills != null)
                    foreach (BillPay i in reserveOld.Bills)
                    {
                        if (!reserve.Bills.Exists(c => c.Id == i.Id))
                        {
                            ctx.ExecutaQuery("DELETE FROM BillPay WHERE Id = " + i.Id);
                        }
                    }

                #endregion

                #region Update

                ////lista de updates
                //List<BillReceive> updateBills = package.Bills.FindAll(i => i.Id > 0);

                //updateBills.ToList().ForEach(x =>
                //{
                //    StringBuilder sqlU = new StringBuilder();

                //    sqlU.Append("UPDATE BillReceive ");
                //    sqlU.Append("SET ");
                //    sqlU.Append("Amount = @Amount, ");
                //    sqlU.Append("AmountReceived = @AmountReceived, ");
                //    sqlU.Append("Concerning = @Concerning, ");
                //    sqlU.Append("DueDate = @DueDate, ");
                //    sqlU.Append("PayDay = @PayDay, ");
                //    sqlU.Append("Comments = @Comments ");
                //    sqlU.Append("WHERE Id = @Id;");

                //    MySqlCommand cmmU = new MySqlCommand(sqlU.ToString());

                //    cmmU.Parameters.Add("@Amount", MySqlDbType.Decimal).Value = x.Amount;
                //    cmmU.Parameters.Add("@AmountReceived", MySqlDbType.Decimal).Value = x.AmountReceived;
                //    cmmU.Parameters.Add("@Concerning", MySqlDbType.Text).Value = x.Concerning;
                //    cmmU.Parameters.Add("@DueDate", MySqlDbType.Date).Value = x.DueDate;
                //    cmmU.Parameters.Add("@PayDay", MySqlDbType.Date).Value = x.PayDay;
                //    cmmU.Parameters.Add("@Comments", MySqlDbType.Text).Value = x.Comments;
                //    cmmU.Parameters.Add("@Id", MySqlDbType.Int32).Value = x.Id;

                //    ctx.ExecutaQuery(cmmU);
                //});

                #endregion

                #region Insert

                List<BillPay> insertBills = reserve.Bills.FindAll(i => i.Id == 0);

                insertBills.ToList().ForEach(x =>
                {
                    StringBuilder sqlI = new StringBuilder();

                    sqlI.Append("INSERT INTO BillPay (");
                    sqlI.Append("ReservationId, ");
                    sqlI.Append("Amount, ");
                    sqlI.Append("AmountPaid, ");
                    sqlI.Append("Concerning, ");
                    sqlI.Append("CreateDate, ");
                    sqlI.Append("DueDate, ");
                    sqlI.Append("PayDay, ");
                    sqlI.Append("Comments) ");
                    sqlI.Append("VALUES (");
                    sqlI.Append("@ReservationId, ");
                    sqlI.Append("@Amount, ");
                    sqlI.Append("@AmountPaid, ");
                    sqlI.Append("@Concerning, ");
                    sqlI.Append("@CreateDate, ");
                    sqlI.Append("@DueDate, ");
                    sqlI.Append("@PayDay, ");
                    sqlI.Append("@Comments);");

                    MySqlCommand cmmI = new MySqlCommand(sqlI.ToString());

                    cmmI.Parameters.Add("@ReservationId", MySqlDbType.Int32).Value = reserveOld.Id;
                    cmmI.Parameters.Add("@Amount", MySqlDbType.Decimal).Value = x.Amount;
                    cmmI.Parameters.Add("@AmountReceived", MySqlDbType.Decimal).Value = x.AmountPaid;
                    cmmI.Parameters.Add("@Concerning", MySqlDbType.Text).Value = x.Concerning;
                    cmmI.Parameters.Add("@CreateDate", MySqlDbType.Date).Value = x.CreateDate;
                    cmmI.Parameters.Add("@DueDate", MySqlDbType.Date).Value = x.DueDate;
                    cmmI.Parameters.Add("@PayDay", MySqlDbType.Date).Value = x.PayDay;
                    cmmI.Parameters.Add("@Comments", MySqlDbType.Text).Value = x.Comments;

                    ctx.ExecutaQuery(cmmI);

                });

                #endregion

                #endregion

                ctx.CommitWork();
            }
            catch (MySqlException ex)
            {
                ctx.RollBack();
                throw ex;
            }

        }

        public void Delete(Reservation reserve)
        {
            _context.Reservations.Remove(reserve);
            _context.SaveChanges();
        }
        
        public Reservation Get(int id)
        {
            Reservation pk = _context.Reservations                                    
                                    .Include("Bills")
                                    .Include("Customer")
                                    .Where(x => x.Id == id).FirstOrDefault();
            return pk;
        }

        public List<Reservation> Get(string search)
        {

            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<Reservation> reserves = new List<Reservation>();
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT ");
            sql.Append("Reservation.Id, ");
            sql.Append("Reservation.CustomerId, ");
            sql.Append("Reservation.DateReservation, ");
            sql.Append("Reservation.QuantityTickets, ");
            sql.Append("Reservation.DeparturePlace, ");
            sql.Append("Reservation.Notification, ");
            sql.Append("Reservation.ContractNumber, ");
            sql.Append("Reservation.Status, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM Reservation ");
            sql.Append("INNER JOIN Customer ON Reservation.CustomerId = Customer.Id ");

            if (!string.IsNullOrEmpty(search))
            {
                sql.Append("WHERE Customer.Name LIKE @name ");
                cmm.Parameters.Add("@name", MySqlDbType.VarChar).Value = "%" + search + "%";
            }

            sql.Append("ORDER BY Reservation.DateReservation DESC;");

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {
                reserves.Add(new Reservation(
                    (int)dr["Id"],
                    new Customer(
                        0,
                        0,
                        (string)dr["Name"],
                        "",
                        "",
                        "",
                        "",
                        DateTime.Today,
                        ""),
                    0,
                    (DateTime)dr["DateReservation"],
                    (int)dr["QuantityTickets"],
                    (string)dr["DeparturePlace"],
                    dr.IsDBNull(dr.GetOrdinal("Notification")) ? "" : (string)dr["Notification"],
                    (string)dr["ContractNumber"],
                    (string)dr["Status"],
                    null));
            }

            dr.Close();

            return reserves;

        }

        public List<Reservation> Get(int skip, int take)
        {
            throw new NotImplementedException();
        }        

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
