using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Customers;
using VMCTur.Domain.Entities.Financial.BillsReceive;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Infra.Conn;
using VMCTur.Infra.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace VMCTur.Infra.Repositories
{
    public class TravelPackageRepository : ITravelPackageRepository
    {
        private AppDataContext _context;

        public TravelPackageRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(TravelPackage package)
        {
            _context.TravelPackages.Add(package);
            _context.SaveChanges();
        }

        public void Update(TravelPackage package, TravelPackage packageOld)
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();

            sql.Append("UPDATE TravelPackage ");
            sql.Append("SET ");
            sql.Append("CustomerId = @CustomerId, ");
            sql.Append("Host = @Host, ");
            sql.Append("QuantityTickets = @QuantityTickets, ");            
            sql.Append("AddictionalReservs = @AddictionalReservs, ");
            sql.Append("Comments = @Comments, ");
            sql.Append("TotalAmount = @TotalAmount, ");
            sql.Append("QuantityChild = @QuantityChild, ");
            sql.Append("QuantityAdult = @QuantityAdult, ");
            sql.Append("QuantityEderly = @QuantityEderly, ");

            sql.Append("ArrivalDate = @ArrivalDate, ");
            sql.Append("LeaveDate = @LeaveDate, ");
            sql.Append("DescServices = @DescServices, ");
            sql.Append("PayForms = @PayForms, ");
            sql.Append("AmountForAdult = @AmountForAdult, ");
            sql.Append("AmountForEderly = @AmountForEderly, ");
            sql.Append("AmountForChild = @AmountForChild ");

            sql.Append("WHERE Id = @Id; ");

            MySqlCommand cmm = new MySqlCommand(sql.ToString());

            cmm.Parameters.Add("@CustomerId", MySqlDbType.Int32).Value = package.CustomerId;
            cmm.Parameters.Add("@Host", MySqlDbType.Text).Value = package.Host;
            cmm.Parameters.Add("@QuantityTickets", MySqlDbType.Int32).Value = package.QuantityTickets;            
            cmm.Parameters.Add("@AddictionalReservs", MySqlDbType.Text).Value = package.AddictionalReservs;
            cmm.Parameters.Add("@Comments", MySqlDbType.Text).Value = package.Comments;
            cmm.Parameters.Add("@TotalAmount", MySqlDbType.Decimal).Value = package.TotalAmount;

            cmm.Parameters.Add("@QuantityChild", MySqlDbType.Int32).Value = package.QuantityChild;
            cmm.Parameters.Add("@QuantityAdult", MySqlDbType.Int32).Value = package.QuantityAdult;
            cmm.Parameters.Add("@QuantityEderly", MySqlDbType.Int32).Value = package.QuantityEderly;

            if (package.ArrivalDate.HasValue)
                cmm.Parameters.Add("@ArrivalDate", MySqlDbType.DateTime).Value = package.ArrivalDate.Value;
            else
                cmm.Parameters.Add("@ArrivalDate", MySqlDbType.DateTime);

            if (package.LeaveDate.HasValue)
                cmm.Parameters.Add("@LeaveDate", MySqlDbType.DateTime).Value = package.LeaveDate.Value;
            else
                cmm.Parameters.Add("@LeaveDate", MySqlDbType.DateTime);

            cmm.Parameters.Add("@DescServices", MySqlDbType.Text).Value = package.DescServices;
            cmm.Parameters.Add("@PayForms", MySqlDbType.Text).Value = package.PayForms;
            cmm.Parameters.Add("@AmountForAdult", MySqlDbType.Decimal).Value = package.AmountForAdult;
            cmm.Parameters.Add("@AmountForEderly", MySqlDbType.Decimal).Value = package.AmountForEderly;
            cmm.Parameters.Add("AmountForChild", MySqlDbType.Decimal).Value = package.AmountForChild;

            cmm.Parameters.Add("@Id", MySqlDbType.Int32).Value = packageOld.Id;

            ctx.BeginWork();

            try
            {
                ctx.ExecutaQuery(cmm);

                #region Participants

                #region Delete

                ///Smael: para cada item na lista old que não existe na lista new, exclui-se.
                if (packageOld.Participants != null)
                    foreach (TravelPackageParticipant p in packageOld.Participants)
                    {
                        if (!package.Participants.Exists(c => c.Id == p.Id))
                        {
                            ctx.ExecutaQuery("DELETE FROM TravelPackageParticipant WHERE Id = " + p.Id);
                        }
                    }

                #endregion

                #region Update

                //lista de updates
                List<TravelPackageParticipant> updateParticipants = package.Participants.FindAll(i => i.Id > 0);

                updateParticipants.ToList().ForEach(x =>
                {
                    StringBuilder sqlU = new StringBuilder();

                    sqlU.Append("UPDATE TravelPackageParticipant ");
                    sqlU.Append("SET ");
                    sqlU.Append("Name = @Name, ");
                    sqlU.Append("NumberDocument = @NumberDocument, ");
                    sqlU.Append("BirthDate = @BirthDate ");
                    sqlU.Append("WHERE Id = @Id;");

                    MySqlCommand cmmU = new MySqlCommand(sqlU.ToString());

                    cmmU.Parameters.Add("@Name", MySqlDbType.VarChar).Value = x.Name;
                    cmmU.Parameters.Add("@NumberDocument", MySqlDbType.VarChar).Value = x.NumberDocument;
                    cmmU.Parameters.Add("@BirthDate", MySqlDbType.Date).Value = x.BirthDate;

                    cmmU.Parameters.Add("@Id", MySqlDbType.Int32).Value = x.Id;

                    ctx.ExecutaQuery(cmmU);
                });

                #endregion

                #region Insert

                List<TravelPackageParticipant> insertParticipants = package.Participants.FindAll(i => i.Id == 0);

                insertParticipants.ToList().ForEach(x =>
                {
                    StringBuilder sqlI = new StringBuilder();

                    sqlI.Append("INSERT INTO TravelPackageParticipant (");
                    sqlI.Append("TravelPackageId, ");
                    sqlI.Append("Name, ");
                    sqlI.Append("NumberDocument, ");
                    sqlI.Append("AgeGroupBelong, ");
                    sqlI.Append("Paying, ");
                    sqlI.Append("BirthDate) ");
                    sqlI.Append("VALUES (");
                    sqlI.Append("@TravelPackageId, ");
                    sqlI.Append("@Name, ");
                    sqlI.Append("@NumberDocument, ");
                    sqlI.Append("@AgeGroupBelong, ");
                    sqlI.Append("@Paying, ");
                    sqlI.Append("@BirthDate); ");

                    MySqlCommand cmmI = new MySqlCommand(sqlI.ToString());

                    cmmI.Parameters.Add("@TravelPackageId", MySqlDbType.Int32).Value = packageOld.Id;
                    cmmI.Parameters.Add("@Name", MySqlDbType.VarChar).Value = x.Name;
                    cmmI.Parameters.Add("@NumberDocument", MySqlDbType.VarChar).Value = x.NumberDocument;
                    cmmI.Parameters.Add("@AgeGroupBelong", MySqlDbType.Int32).Value = (int)x.AgeGroupBelong;
                    cmmI.Parameters.Add("@Paying", MySqlDbType.Int16).Value = x.Paying;
                    cmmI.Parameters.Add("@BirthDate", MySqlDbType.Date).Value = x.BirthDate;

                    ctx.ExecutaQuery(cmmI);

                });

                #endregion

                #endregion

                #region Tours

                #region Delete

                if (packageOld.Tours != null)
                    foreach (TravelPackageTour pi in packageOld.Tours)
                    {
                        if (!package.Tours.Exists(c => c.Id == pi.Id))
                        {
                            ctx.ExecutaQuery("DELETE FROM TravelPackageTour WHERE Id = " + pi.Id);
                        }
                    }

                #endregion

                #region Update

                //lista de updates
                List<TravelPackageTour> updateTours = package.Tours.FindAll(i => i.Id > 0);

                updateTours.ToList().ForEach(x =>
                {
                    StringBuilder sqlU = new StringBuilder();

                    sqlU.Append("UPDATE TravelPackageTour ");
                    sqlU.Append("SET ");
                    sqlU.Append("TourId = @TourId, ");
                    sqlU.Append("Shared = @Shared, ");
                    sqlU.Append("Comments = @Comments, ");
                    sqlU.Append("VehicleUsedId = @VehicleUsedId, ");
                    sqlU.Append("GuideTourId = @GuideTourId, ");
                    sqlU.Append("DateHourStart = @DateHourStart ");
                    sqlU.Append("WHERE Id = @Id");

                    MySqlCommand cmmU = new MySqlCommand(sqlU.ToString());

                    cmmU.Parameters.Add("@TourId", MySqlDbType.Int32).Value = x.TourId;
                    cmmU.Parameters.Add("@Shared", MySqlDbType.Int16).Value = x.Shared;
                    cmmU.Parameters.Add("@VehicleUsedId", MySqlDbType.Int32).Value = x.VehicleUsedId;
                    cmmU.Parameters.Add("@GuideTourId", MySqlDbType.Int32).Value = x.GuideTourId;
                    cmmU.Parameters.Add("@Comments", MySqlDbType.Text).Value = x.Comments;
                    cmmU.Parameters.Add("@DateHourStart", MySqlDbType.DateTime).Value = x.DateHourStart;

                    cmmU.Parameters.Add("@Id", MySqlDbType.Int32).Value = x.Id;

                    ctx.ExecutaQuery(cmmU);
                });

                #endregion

                #region Insert

                List<TravelPackageTour> insertTours = package.Tours.FindAll(i => i.Id == 0);

                insertTours.ToList().ForEach(x =>
                {
                    StringBuilder sqlI = new StringBuilder();

                    sqlI.Append("INSERT INTO TravelPackageTour (");
                    sqlI.Append("TourId, ");
                    sqlI.Append("TravelPackageId, ");
                    sqlI.Append("Shared, ");
                    sqlI.Append("Comments, ");
                    sqlI.Append("VehicleUsedId, ");
                    sqlI.Append("GuideTourId, ");
                    sqlI.Append("DateHourStart) ");
                    sqlI.Append("VALUES (");
                    sqlI.Append("@TourId, ");
                    sqlI.Append("@TravelPackageId, ");
                    sqlI.Append("@Shared, ");
                    sqlI.Append("@Comments, ");
                    sqlI.Append("@VehicleUsedId, ");
                    sqlI.Append("@GuideTourId, ");                    
                    sqlI.Append("@DateHourStart); ");

                    MySqlCommand cmmI = new MySqlCommand(sqlI.ToString());
                    
                    cmmI.Parameters.Add("@TourId", MySqlDbType.Int32).Value = x.TourId;
                    cmmI.Parameters.Add("@DateHourStart", MySqlDbType.DateTime).Value = x.DateHourStart;
                    cmmI.Parameters.Add("@Shared", MySqlDbType.Int16).Value = x.Shared;
                    cmmI.Parameters.Add("@VehicleUsedId", MySqlDbType.Int32).Value = x.VehicleUsedId;
                    cmmI.Parameters.Add("@GuideTourId", MySqlDbType.Int32).Value = x.GuideTourId;
                    cmmI.Parameters.Add("@Comments", MySqlDbType.Text).Value = x.Comments;

                    cmmI.Parameters.Add("@TravelPackageId", MySqlDbType.Int32).Value = x.TravelPackageId;

                    ctx.ExecutaQuery(cmmI);

                });

                #endregion

                #endregion

                #region Bills

                #region Delete

                ///Smael: para cada item na lista old que não existe na lista new, exclui-se.
                if (packageOld.Bills != null)
                foreach (BillReceive i in packageOld.Bills)
                {
                    if (!package.Bills.Exists(c => c.Id == i.Id))
                    {
                        ctx.ExecutaQuery("DELETE FROM BillReceive WHERE Id = " + i.Id);
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

                List<BillReceive> insertBills = package.Bills.FindAll(i => i.Id == 0);

                insertBills.ToList().ForEach(x =>
                {
                    StringBuilder sqlI = new StringBuilder();

                    sqlI.Append("INSERT INTO BillReceive (");
                    sqlI.Append("TravelPackageId, ");
                    sqlI.Append("Amount, ");
                    sqlI.Append("AmountReceived, ");
                    sqlI.Append("Concerning, ");
                    sqlI.Append("CreateDate, ");
                    sqlI.Append("DueDate, ");
                    sqlI.Append("PayDay, ");
                    sqlI.Append("Comments) ");
                    sqlI.Append("VALUES (");
                    sqlI.Append("@TravelPackageId, ");
                    sqlI.Append("@Amount, ");
                    sqlI.Append("@AmountReceived, ");
                    sqlI.Append("@Concerning, ");
                    sqlI.Append("@CreateDate, ");
                    sqlI.Append("@DueDate, ");
                    sqlI.Append("@PayDay, ");
                    sqlI.Append("@Comments);");

                    MySqlCommand cmmI = new MySqlCommand(sqlI.ToString());

                    cmmI.Parameters.Add("@TravelPackageId", MySqlDbType.Int32).Value = packageOld.Id;
                    cmmI.Parameters.Add("@Amount", MySqlDbType.Decimal).Value = x.Amount;
                    cmmI.Parameters.Add("@AmountReceived", MySqlDbType.Decimal).Value = x.AmountReceived;
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

        public void Delete(TravelPackage package)
        {
            _context.TravelPackages.Remove(package);
            _context.SaveChanges();
        }

        public TravelPackage Get(int id)
        {
            TravelPackage pk = _context.TravelPackages
                                    .Include("Participants")
                                    .Include("Bills")                                                                                                 
                                    .Include("Customer")
                                    .Where(x => x.Id == id).FirstOrDefault();


            List<TravelPackageTour> pkTours = _context.TravelPackageTours
                .Include("Tour")
                .Include("VehicleUsed")
                .Include("GuideTour")
                .Where(y => y.TravelPackageId == id).ToList();

            return pk;
        }

        public List<TravelPackage> Get(string search)
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<TravelPackage> packages = new List<TravelPackage>();
            MySqlCommand cmm = new MySqlCommand();

            sql.Append("SELECT TravelPackage.Id, ");
            sql.Append("TravelPackage.CompanyId, ");
            sql.Append("TravelPackage.CreationDate, ");
            sql.Append("TravelPackage.CustomerId, ");
            sql.Append("TravelPackage.Host, ");
            sql.Append("TravelPackage.QuantityTickets, ");            
            sql.Append("TravelPackage.AddictionalReservs, ");
            sql.Append("TravelPackage.Comments, ");
            sql.Append("TravelPackage.TotalAmount, ");
            sql.Append("TravelPackage.ArrivalDate, ");
            sql.Append("TravelPackage.LeaveDate, ");
            sql.Append("TravelPackage.DescServices, ");
            sql.Append("TravelPackage.PayForms, ");
            sql.Append("TravelPackage.AmountForAdult, ");
            sql.Append("TravelPackage.AmountForEderly, ");
            sql.Append("TravelPackage.AmountForChild, ");
            sql.Append("Customer.Name ");
            sql.Append("FROM TravelPackage ");
            sql.Append("INNER JOIN Customer ON TravelPackage.CustomerId = Customer.Id ");

            if (!string.IsNullOrEmpty(search))
            {
                sql.Append("WHERE Customer.Name LIKE @name ");
                cmm.Parameters.Add("@name", MySqlDbType.VarChar).Value = "%" + search + "%";
            }

            sql.Append("ORDER BY TravelPackage.CreationDate DESC;");

            cmm.CommandText = sql.ToString();

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {
                DateTime? auxArrivalDate = null;

                if (!dr.IsDBNull(dr.GetOrdinal("ArrivalDate")))
                    auxArrivalDate = (DateTime)dr["ArrivalDate"];

                DateTime? auxLeaveDate = null;

                if (!dr.IsDBNull(dr.GetOrdinal("LeaveDate")))
                    auxLeaveDate = (DateTime)dr["LeaveDate"];

                packages.Add(new TravelPackage(
                    (int)dr["Id"],
                    (int)dr["CompanyId"],
                    (DateTime)dr["CreationDate"],
                    (int)dr["CustomerId"],
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
                    new List<TravelPackageParticipant>(),
                    new List<TravelPackageTour>(),
                    new List<BillReceive>(),
                    dr.IsDBNull(dr.GetOrdinal("Host")) ? "" : (string)dr["Host"],
                    (int)dr["QuantityTickets"],                   
                    (decimal)dr["TotalAmount"],
                    dr.IsDBNull(dr.GetOrdinal("AddictionalReservs")) ? "" : (string)dr["AddictionalReservs"],
                    dr.IsDBNull(dr.GetOrdinal("Comments")) ? "" : (string)dr["Comments"],
                    auxArrivalDate,
                    auxLeaveDate,
                    (decimal)dr["AmountForAdult"],
                    (decimal)dr["AmountForEderly"],
                    (decimal)dr["AmountForChild"],
                    dr.IsDBNull(dr.GetOrdinal("DescServices")) ? "" : (string)dr["DescServices"],
                    dr.IsDBNull(dr.GetOrdinal("PayForms")) ? "" : (string)dr["PayForms"]));
            }   

            dr.Close();

            return packages;
        }

        public List<TravelPackage> Get(int skip, int take)
        {
            return _context.TravelPackages
                                .Include("Customer")
                                .OrderBy(x => x.CreationDate).Skip(skip).Take(take).ToList();
        }

        public MemoryStream PrintPreBooking(TravelPackage package, string url)
        {
            using (MemoryStream output = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                var pdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, output);
                document.Open();

                //FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                var fontCabecalho = FontFactory.GetFont("Times New Roman", 12);
                var fontNormal = FontFactory.GetFont("Times New Roman", 7);
                var fontObs = FontFactory.GetFont("Times New Roman", 6);

                float espacamentoNormal = 10;
                float espacamentoLinhaEmBranco = 16;

                // Figuras geométricas.
                var contentByte = pdfWriter.DirectContent;

                // Imagem.                
                var logo = Image.GetInstance(url + "Resources\\logo_vmc_pdf.jpg");
                logo.ScaleToFit(150, 75);
                logo.SetAbsolutePosition(20, 760);
                contentByte.AddImage(logo);

                var paragraph = new Paragraph("Fone: (54) 3286 1209 / (54) 8111 9986 (Tim - WhatsApp)", FontFactory.GetFont("Times New Roman", 12, BaseColor.BLUE));
                paragraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraph);

                paragraph = new Paragraph("Email: vmcturismo@gramadosite.com.br", fontCabecalho);
                paragraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraph);

                // cria um novo paragrafo para imprimir um traço e uma linha em branco
                var ph = new Paragraph();

                // cria um objeto sepatador (um traço)
                iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();

                // adiciona o separador ao paragravo
                ph.Add(seperator);

                // adiciona a linha em branco(enter) ao paragrafo
                ph.Add(new Chunk("\n"));

                // imprime o pagagrafo no documento
                document.Add(ph);

                paragraph = new Paragraph("Gramado, " + DateTime.Today.ToLongDateString(), fontNormal);
                paragraph.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "A", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                //Smael: nome do cliente.
                paragraph = new Paragraph(espacamentoNormal, package.Customer.Name, fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Segue abaixo DADOS PARA EFETUAÇÃO DE RESERVA: ", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);


                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Serviços a serem prestados: ", FontFactory.GetFont("Times New Roman", 8, BaseColor.RED));
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                if (package.Tours != null)
                {
                    foreach (TravelPackageTour t in package.Tours)
                    {
                        paragraph = new Paragraph(espacamentoNormal, t.DateStart.ToShortDateString() + " - " + t.Tour.Name, fontNormal);
                        paragraph.Alignment = Element.ALIGN_LEFT;
                        document.Add(paragraph);
                    }
                }

                decimal amountPerPerson = package.TotalAmount / package.QuantityTickets;

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Valor total por pessoa R$ " + Math.Round(amountPerPerson, 2, MidpointRounding.AwayFromZero) + "(base " + package.QuantityTickets + " pessoas)", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Valor total da reserva: R$ " + Math.Round(package.TotalAmount, 2, MidpointRounding.AwayFromZero) + " (Um mil oitocentos e setenta e seis reais)", FontFactory.GetFont("Calibri", 7, BaseColor.RED));
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Formas de Pagamento: ", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                //data de vencimento da primeira parcela.

                if (package.Bills != null)
                {
                    if (package.Bills.Count > 0)
                        paragraph = new Paragraph(espacamentoNormal, "Solicitamos depósito antecipado de 30% do total na conta da empresa até a data de " + package.Bills[0].DueDate.ToShortDateString() + " - para confirmação da reserva, e restante pagamento em Gramado no dia da chegada(04 / 04).", fontNormal);
                    else
                        paragraph = new Paragraph(espacamentoNormal, "Solicitamos depósito antecipado de 30% do total na conta da empresa para confirmação da reserva, e restante pagamento em Gramado no dia da chegada(04 / 04).", fontNormal);
                }

                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Dados para depósito", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoNormal, "Cristiane Rosa:", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoNormal, "Banco Itaú", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoNormal, "Agência:", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "1606", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Conta Corrente: 01802 - 4", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "CNPJ: 09.396.197 / 0001 - 02", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Favorecido: Cristiane Lúcio da Rosa", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Ou", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Banco do Brasil", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Agencia 0575 - 4", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Conta corrente 115850 - 3", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "CNPJ: 09.396.197 / 0001 - 02", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Favorecido: VMC TURISMO", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Obs: Enviar comprovante via FAX(54 3286 1209, ou via email: vmcturismo@gramadosite.com.br", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Observações Gerais", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "01) No traslado de retorno, saída de Gramado com no mínimo 04 horas de antecedência em relação ao voo, para evitar possíveis atrasos;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "02)	Os passeios poderão sofrer alteração no roteiro ou cancelamentos devido a condições climáticas ou fator de força maior que impeça aquela atividade;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "03)	Os horários dos passeios e transfers serão informados pela agência operadora sempre no dia que antecede o passeio ou o transfer, geralmente à tarde;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "04)	Todos os passeios e transfers envolvem planejamento e alocação de pessoas e equipamentos;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "05)	A agência reserva o direito de utilizar ônibus, micro-ônibus, vans e carros;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "06)	A ordem da execução dos roteiros poderá ser alterada, dependendo da data de chegada;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "07)	A agência não se responsabiliza por pertences deixados nos veículos;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "08)	A não realização de passeios e/ ou traslados não dá direito a restituição de valor, troca ou remuneração para outra data;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "09)	Não haverá tolerância no horário marcado para saída dos passeios e / ou traslados, por isso observar o horário para sua saída do hotel;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "10) O local de encontro é sempre na recepção do hotel e quando não encontrado, a agência se reserva o direito por respeito aos demais clientes, de dar continuidade ao passeio ou transfer;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "11)	Os passeios e ou/ traslados acima é em base regular, ou seja, o valor é correspondente ao serviço realizado em grupo;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "12)	O carro da agência buscará e deixará o passageiro no hotel onde o mesmo se encontra hospedado em Gramado ou Canela;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "13)	Atividade sujeita à alteração de datas, em função da programação interna da agência;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "14)	Referente a cancelamentos:", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "   - Antecedência de 05(cinco) a 20(vinte) dias da viagem, multa de 50 % sobre o valor dos serviços de receptivo;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "   - Antecedência de 04(quatro) ou menos dias da viagem, multa de 100 % sobre o valor dos serviços de receptivo;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "   - Para os períodos compreendidos entre Junho, Julho, Natal Luz, Feriados, Congressos e demais eventos, a multa poderá ser de até 100 % sobre o valor total da reserva, independente de prazo de antecedência.", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "15)	O contratante é obrigado a comunicar no ato da contratação de quaisquer serviços se ha passageiro(s)com necessidades especiais;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "16)	Poderá haver substituição de veículos e Guia / Motorista nos traslados e / ou passeios;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "17)	Os traslados de ida e volta serão realizados com todos juntos conforme contrato, caso houver alteração no voo de 01 ou mais passageiros não haverá reembolso de valores, o(s)passageiro(s) poderá(ão) optar por um ou mais traslados extras com valores à parte;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "18)	Valores com base em 07 pessoas, caso venha diminuir ou aumentar, consultar valores com a agência;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "19)	É expressamente proibido o consumo de bebidas alcoólicas dentro do veículo;", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "20) É OBRIGATÓRIO PORTAR DOCUMENTO DE IDENTIDADE NOS TRASLADOS E / OU PASSEIOS.CRIANÇAS QUE NÃO POSSUÍREM CARTEIRA DE IDENTIDADE DEVERÃO PORTAR SUA CERTIDÃO DE NASCIMENTO.EM CASO DE NÃO ESTAREM PORTANDO SUA IDENTIDADE E O VEÍCULO FOR NOTIFICADO PELO ÓRGÃO DE TRÂNSITO O CONTRATANTE FICA RESPONSÁVEL POR QUAISQUER MULTAS QUE VIREM A SER APLICADAS PELO NÃO CUMPRIMENTO DESTA OBRIGATORIEDADE.", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);


                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Atenciosamente", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "VMC Turismo", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Cristiane Rosa", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);


                document.Close();
                //System.Diagnostics.Process.Start("output.pdf");

                return output;
            }
        }

        public MemoryStream PrintBookingConfirmation(TravelPackage package, string url)
        {
            //using (var fileStream = new System.IO.FileStream("output.pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            //{
            using (MemoryStream output = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                var pdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, output);
                document.Open();

                //FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                var fontCabecalho = FontFactory.GetFont("Times New Roman", 12);
                var fontNormal = FontFactory.GetFont("Times New Roman", 7);
                var fontObs = FontFactory.GetFont("Times New Roman", 6);

                float espacamentoNormal = 10;
                float espacamentoLinhaEmBranco = 16;

                var contentByte = pdfWriter.DirectContent;

                // Imagem                
                var logo = Image.GetInstance(url + "Resources\\logo_vmc_pdf.jpg");
                logo.ScaleToFit(150, 75);
                logo.SetAbsolutePosition(20, 760);
                contentByte.AddImage(logo);

                var paragraph = new Paragraph("Fone: (54) 3286 1209 / (54) 8111 9986 (Tim - WhatsApp)", FontFactory.GetFont("Times New Roman", 12, BaseColor.BLUE));
                paragraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraph);

                paragraph = new Paragraph("Email: vmcturismo@gramadosite.com.br", fontCabecalho);
                paragraph.Alignment = Element.ALIGN_RIGHT;
                document.Add(paragraph);

                // cria um novo paragrafo para imprimir um traço e uma linha em branco
                var ph = new Paragraph();

                // cria um objeto sepatador (um traço)
                iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();

                // adiciona o separador ao paragravo
                ph.Add(seperator);

                // adiciona a linha em branco(enter) ao paragrafo
                ph.Add(new Chunk("\n"));

                // imprime o pagagrafo no documento
                document.Add(ph);

                paragraph = new Paragraph("Gramado, " + DateTime.Today.ToLongDateString(), fontNormal);
                paragraph.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "A", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                //Smael: nome do cliente.
                paragraph = new Paragraph(espacamentoNormal, package.Customer.Name, fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Segue abaixo DADOS PARA EFETUAÇÃO DE RESERVA: ", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);


                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Serviços a serem prestados: ", FontFactory.GetFont("Times New Roman", 8, BaseColor.RED));
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                if (package.Tours != null)
                {
                    foreach (TravelPackageTour t in package.Tours)
                    {
                        paragraph = new Paragraph(espacamentoNormal, t.DateStart.ToShortDateString() + " - " + t.Tour.Name, fontNormal);
                        paragraph.Alignment = Element.ALIGN_LEFT;
                        document.Add(paragraph);
                    }
                }

                decimal amountPerPerson = package.TotalAmount / package.QuantityTickets;

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Valor total por pessoa R$ " + Math.Round(amountPerPerson, 2, MidpointRounding.AwayFromZero) + "(base " + package.QuantityTickets + " pessoas)", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Valor total da reserva: R$ " + Math.Round(package.TotalAmount, 2, MidpointRounding.AwayFromZero) + " (Um mil oitocentos e setenta e seis reais)", FontFactory.GetFont("Calibri", 7, BaseColor.RED));
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Formas de Pagamento: ", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                if (package.Bills != null)
                {
                    foreach (BillReceive b in package.Bills)
                    {
                        paragraph = new Paragraph(espacamentoNormal, b.Concerning + "no valor de R$ " + Math.Round(b.Amount, 2, MidpointRounding.AwayFromZero) + " com vencimento em " + b.DueDate.ToShortDateString() + ". (" + b.Status + ")", fontNormal);
                        paragraph.Alignment = Element.ALIGN_LEFT;
                        document.Add(paragraph);
                    }
                }

                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Dados para depósito", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoNormal, "Cristiane Rosa:", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoNormal, "Banco Itaú", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoNormal, "Agência:", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "1606", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Conta Corrente: 01802 - 4", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "CNPJ: 09.396.197 / 0001 - 02", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Favorecido: Cristiane Lúcio da Rosa", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Ou", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Banco do Brasil", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Agencia 0575 - 4", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Conta corrente 115850 - 3", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "CNPJ: 09.396.197 / 0001 - 02", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Favorecido: VMC TURISMO", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Obs: Enviar comprovante via FAX(54 3286 1209, ou via email: vmcturismo@gramadosite.com.br", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Observações Gerais", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "01) No traslado de retorno, saída de Gramado com no mínimo 04 horas de antecedência em relação ao voo, para evitar possíveis atrasos;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "02)	Os passeios poderão sofrer alteração no roteiro ou cancelamentos devido a condições climáticas ou fator de força maior que impeça aquela atividade;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "03)	Os horários dos passeios e transfers serão informados pela agência operadora sempre no dia que antecede o passeio ou o transfer, geralmente à tarde;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "04)	Todos os passeios e transfers envolvem planejamento e alocação de pessoas e equipamentos;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "05)	A agência reserva o direito de utilizar ônibus, micro-ônibus, vans e carros;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "06)	A ordem da execução dos roteiros poderá ser alterada, dependendo da data de chegada;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "07)	A agência não se responsabiliza por pertences deixados nos veículos;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "08)	A não realização de passeios e/ ou traslados não dá direito a restituição de valor, troca ou remuneração para outra data;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "09)	Não haverá tolerância no horário marcado para saída dos passeios e / ou traslados, por isso observar o horário para sua saída do hotel;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "10) O local de encontro é sempre na recepção do hotel e quando não encontrado, a agência se reserva o direito por respeito aos demais clientes, de dar continuidade ao passeio ou transfer;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "11)	Os passeios e ou/ traslados acima é em base regular, ou seja, o valor é correspondente ao serviço realizado em grupo;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "12)	O carro da agência buscará e deixará o passageiro no hotel onde o mesmo se encontra hospedado em Gramado ou Canela;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "13)	Atividade sujeita à alteração de datas, em função da programação interna da agência;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "14)	Referente a cancelamentos:", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "   - Antecedência de 05(cinco) a 20(vinte) dias da viagem, multa de 50 % sobre o valor dos serviços de receptivo;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "   - Antecedência de 04(quatro) ou menos dias da viagem, multa de 100 % sobre o valor dos serviços de receptivo;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "   - Para os períodos compreendidos entre Junho, Julho, Natal Luz, Feriados, Congressos e demais eventos, a multa poderá ser de até 100 % sobre o valor total da reserva, independente de prazo de antecedência.", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "15)	O contratante é obrigado a comunicar no ato da contratação de quaisquer serviços se ha passageiro(s)com necessidades especiais;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "16)	Poderá haver substituição de veículos e Guia / Motorista nos traslados e / ou passeios;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "17)	Os traslados de ida e volta serão realizados com todos juntos conforme contrato, caso houver alteração no voo de 01 ou mais passageiros não haverá reembolso de valores, o(s)passageiro(s) poderá(ão) optar por um ou mais traslados extras com valores à parte;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "18)	Valores com base em 07 pessoas, caso venha diminuir ou aumentar, consultar valores com a agência;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "19)	É expressamente proibido o consumo de bebidas alcoólicas dentro do veículo;", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "20) É OBRIGATÓRIO PORTAR DOCUMENTO DE IDENTIDADE NOS TRASLADOS E / OU PASSEIOS.CRIANÇAS QUE NÃO POSSUÍREM CARTEIRA DE IDENTIDADE DEVERÃO PORTAR SUA CERTIDÃO DE NASCIMENTO.EM CASO DE NÃO ESTAREM PORTANDO SUA IDENTIDADE E O VEÍCULO FOR NOTIFICADO PELO ÓRGÃO DE TRÂNSITO O CONTRATANTE FICA RESPONSÁVEL POR QUAISQUER MULTAS QUE VIREM A SER APLICADAS PELO NÃO CUMPRIMENTO DESTA OBRIGATORIEDADE.", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);


                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Atenciosamente", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "VMC Turismo", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Cristiane Rosa", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);


                document.Close();
                //System.Diagnostics.Process.Start("output.pdf");

                return output;
            }
        }

        public MemoryStream PrintVoucher(TravelPackage package, string url)
        {

            //using (var fileStream = new System.IO.FileStream("output.pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            //{
            using (MemoryStream output = new MemoryStream())
            {
                var document = new iTextSharp.text.Document();
                var pdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, output);
                document.Open();

                //FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                var fontCabecalho = FontFactory.GetFont("Times New Roman", 12);
                var fontNormal = FontFactory.GetFont("Times New Roman", 8);
                var fontNegrito = FontFactory.GetFont(FontFactory.TIMES_BOLD, 8);
                var fontObs = FontFactory.GetFont("Times New Roman", 7);

                float espacamentoNormal = 10;
                float espacamentoLinhaEmBranco = 16;

                // Figuras geométricas.
                var contentByte = pdfWriter.DirectContent;
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                contentByte.SetFontAndSize(bf, 8);

                // Imagem.                
                
                var logo = Image.GetInstance(url + "/Resources/logo_vmc_pdf.jpg");
                logo.ScaleToFit(180, 80);
                logo.SetAbsolutePosition(20, 750);

                var slogam = Image.GetInstance(url + "/Resources/slogan_vmc_pdf.jpg");
                slogam.ScaleToFit(255, 100);
                slogam.SetAbsolutePosition(300, 750);
                
                contentByte.AddImage(logo);
                contentByte.AddImage(slogam);

                // cria um novo paragrafo para imprimir um traço e uma linha em branco
                var ph = new Paragraph();

                // adiciona a linha em branco(enter) ao paragrafo
                ph.Add(new Chunk("\n"));
                // adiciona a linha em branco(enter) ao paragrafo
                ph.Add(new Chunk("\n"));
                // adiciona a linha em branco(enter) ao paragrafo
                ph.Add(new Chunk("\n"));

                // cria um objeto sepatador (um traço)
                iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();

                // adiciona o separador ao paragravo
                ph.Add(seperator);

                // adiciona a linha em branco(enter) ao paragrafo
                ph.Add(new Chunk("\n"));

                // imprime o pagagrafo no documento
                document.Add(ph);

                var paragraph = new Paragraph("Voucher Serviços", FontFactory.GetFont("Times New Roman", 14));
                paragraph.Alignment = Element.ALIGN_CENTER;
                document.Add(paragraph);


                contentByte.Rectangle(35, 660, 260, 40);
                contentByte.Stroke();
                contentByte.Rectangle(295, 660, 260, 40);
                contentByte.Stroke();

                contentByte.Rectangle(35, 620, 260, 40);
                contentByte.Stroke();
                contentByte.Rectangle(295, 620, 260, 40);
                contentByte.Stroke();

                contentByte.BeginText();

                //Quadro 1
                contentByte.SetTextMatrix(37, 688);
                contentByte.ShowText("Nome: Fulano de tal.");

                contentByte.SetTextMatrix(37, 678);
                contentByte.ShowText("Fone: (51) 9696 9696.");

                contentByte.SetTextMatrix(37, 668);
                contentByte.ShowText("E-Mail: comercial@pillarturismo.com.");

                //Quadro 2

                contentByte.SetTextMatrix(37, 648);
                contentByte.ShowText("Destino: " + package + ".");

                contentByte.SetTextMatrix(37, 638);
                contentByte.ShowText("Nº Adultos: 3.");

                contentByte.SetTextMatrix(37, 628);
                contentByte.ShowText("Nº Crianças: 3.");

                //Quadro 3

                contentByte.SetTextMatrix(297, 688);
                contentByte.ShowText("Destino: Pousada Vale Verde - Gramado/RS.");

                contentByte.SetTextMatrix(297, 678);
                contentByte.ShowText("Nº Adultos: 3.");

                contentByte.SetTextMatrix(297, 668);
                contentByte.ShowText("Nº Crianças: 3.");

                //Quadro 4

                contentByte.SetTextMatrix(297, 648);
                contentByte.ShowText("Destino: Pousada Vale Verde - Gramado/RS.");

                contentByte.SetTextMatrix(297, 638);
                contentByte.ShowText("Nº Adultos: 3.");

                contentByte.SetTextMatrix(297, 628);
                contentByte.ShowText("Nº Crianças: 3.");

                contentByte.EndText();

                paragraph = new Paragraph(120, "Cordialmente fornecer os seguintes serviços mediante a apresentação deste comprovante.", fontNegrito);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "SERVIÇOS: ", fontNegrito);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                if (package.Tours != null)
                {
                    foreach (TravelPackageTour t in package.Tours)
                    {
                        paragraph = new Paragraph(espacamentoNormal, t.DateStart.ToShortDateString() + " - " + t.Tour.Name, fontNormal);
                        paragraph.Alignment = Element.ALIGN_LEFT;
                        document.Add(paragraph);
                    }
                }

                // cria um novo paragrafo para imprimir um traço e uma linha em branco
                ph = new Paragraph();
                seperator = new iTextSharp.text.pdf.draw.LineSeparator();
                ph.Add(seperator);
                ph.Add(new Chunk("\n"));
                document.Add(ph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "PASSAGEIROS: ", fontNegrito);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);

                if (package.Participants != null)
                {
                    foreach (TravelPackageParticipant p in package.Participants)
                    {
                        paragraph = new Paragraph(espacamentoNormal, p.Name, fontNormal);
                        paragraph.Alignment = Element.ALIGN_LEFT;
                        document.Add(paragraph);
                    }
                }

                // cria um novo paragrafo para imprimir um traço e uma linha em branco
                ph = new Paragraph();
                seperator = new iTextSharp.text.pdf.draw.LineSeparator();
                ph.Add(seperator);
                ph.Add(new Chunk("\n"));
                document.Add(ph);

                paragraph = new Paragraph(espacamentoLinhaEmBranco, "OBSERVAÇÕES GERAIS", fontNegrito);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "01) No traslado de retorno, saída de Gramado com no mínimo 04 horas de antecedência em relação ao voo, para evitar possíveis atrasos;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "02)	Os passeios poderão sofrer alteração no roteiro ou cancelamentos devido a condições climáticas ou fator de força maior que impeça aquela atividade;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "03)	Os horários dos passeios e transfers serão informados pela agência operadora sempre no dia que antecede o passeio ou o transfer, geralmente à tarde;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "04)	Todos os passeios e transfers envolvem planejamento e alocação de pessoas e equipamentos;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "05)	A agência reserva o direito de utilizar ônibus, micro-ônibus, vans e carros;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "06)	A ordem da execução dos roteiros poderá ser alterada, dependendo da data de chegada;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "07)	A agência não se responsabiliza por pertences deixados nos veículos;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "08)	A não realização de passeios e/ ou traslados não dá direito a restituição de valor, troca ou remuneração para outra data;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "09)	Não haverá tolerância no horário marcado para saída dos passeios e / ou traslados, por isso observar o horário para sua saída do hotel;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "10) O local de encontro é sempre na recepção do hotel e quando não encontrado, a agência se reserva o direito por respeito aos demais clientes, de dar continuidade ao passeio ou transfer;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "11)	Os passeios e ou/ traslados acima é em base regular, ou seja, o valor é correspondente ao serviço realizado em grupo;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "12)	O carro da agência buscará e deixará o passageiro no hotel onde o mesmo se encontra hospedado em Gramado ou Canela;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "13)	Atividade sujeita à alteração de datas, em função da programação interna da agência;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "14)	Referente a cancelamentos:", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "   - Antecedência de 05(cinco) a 20(vinte) dias da viagem, multa de 50 % sobre o valor dos serviços de receptivo;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "   - Antecedência de 04(quatro) ou menos dias da viagem, multa de 100 % sobre o valor dos serviços de receptivo;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "   - Para os períodos compreendidos entre Junho, Julho, Natal Luz, Feriados, Congressos e demais eventos, a multa poderá ser de até 100 % sobre o valor total da reserva, independente de prazo de antecedência.", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "15)	O contratante é obrigado a comunicar no ato da contratação de quaisquer serviços se ha passageiro(s)com necessidades especiais;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "16)	Poderá haver substituição de veículos e Guia / Motorista nos traslados e / ou passeios;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "17)	Os traslados de ida e volta serão realizados com todos juntos conforme contrato, caso houver alteração no voo de 01 ou mais passageiros não haverá reembolso de valores, o(s)passageiro(s) poderá(ão) optar por um ou mais traslados extras com valores à parte;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "18)	Valores com base em 07 pessoas, caso venha diminuir ou aumentar, consultar valores com a agência;", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "19)	É expressamente proibido o consumo de bebidas alcoólicas dentro do veículo;", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "20) É OBRIGATÓRIO PORTAR DOCUMENTO DE IDENTIDADE NOS TRASLADOS E / OU PASSEIOS.CRIANÇAS QUE NÃO POSSUÍREM CARTEIRA DE IDENTIDADE DEVERÃO PORTAR SUA CERTIDÃO DE NASCIMENTO.EM CASO DE NÃO ESTAREM PORTANDO SUA IDENTIDADE E O VEÍCULO FOR NOTIFICADO PELO ÓRGÃO DE TRÂNSITO O CONTRATANTE FICA RESPONSÁVEL POR QUAISQUER MULTAS QUE VIREM A SER APLICADAS PELO NÃO CUMPRIMENTO DESTA OBRIGATORIEDADE.", fontObs);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);


                paragraph = new Paragraph(espacamentoLinhaEmBranco, "Atenciosamente", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "VMC Turismo", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);
                paragraph = new Paragraph(espacamentoNormal, "Cristiane Rosa", fontNormal);
                paragraph.Alignment = Element.ALIGN_LEFT;
                document.Add(paragraph);


                document.Close();
                //System.Diagnostics.Process.Start("output.pdf");

                return output;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
