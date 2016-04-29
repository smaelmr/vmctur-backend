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
using iTextSharp;
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

            sql.Append("UPDATE travelpackage ");
            sql.Append("SET ");
            sql.Append("CustomerId = @CustomerId, ");
            sql.Append("Host = @Host, ");
            sql.Append("QuantityTickets = @QuantityTickets, ");
            sql.Append("VehicleUsedId = @VehicleUsedId, ");
            sql.Append("GuideTourId = @GuideTourId, ");
            sql.Append("AddictionalReservs = @AddictionalReservs, ");
            sql.Append("Comments = @Comments, ");
            sql.Append("TotalAmount = @TotalAmount ");
            sql.Append("WHERE Id = @Id; ");

            MySqlCommand cmm = new MySqlCommand(sql.ToString());

            cmm.Parameters.Add("@CustomerId", MySqlDbType.Int32).Value = package.CustomerId;
            cmm.Parameters.Add("@Host", MySqlDbType.Text).Value = package.Host;
            cmm.Parameters.Add("@QuantityTickets", MySqlDbType.Int32).Value = package.QuantityTickets;
            cmm.Parameters.Add("@VehicleUsedId", MySqlDbType.Int32).Value = package.VehicleUsedId;
            cmm.Parameters.Add("@GuideTourId", MySqlDbType.Int32).Value = package.GuideTourId;
            cmm.Parameters.Add("@AddictionalReservs", MySqlDbType.Text).Value = package.AddictionalReservs;
            cmm.Parameters.Add("@Comments", MySqlDbType.Text).Value = package.Comments;
            cmm.Parameters.Add("@TotalAmount", MySqlDbType.Decimal).Value = package.TotalAmount;

            cmm.Parameters.Add("@Id", MySqlDbType.Int32).Value = packageOld.Id;

            ctx.BeginWork();

            try
            {
                ctx.ExecutaQuery(cmm);

                #region Participants

                #region Delete

                ///Smael: para cada item na lista old que não existe na lista new, exclui-se.
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
                    sqlI.Append("BirthDate) ");
                    sqlI.Append("VALUES (");
                    sqlI.Append("@TravelPackageId, ");
                    sqlI.Append("@Name, ");
                    sqlI.Append("@NumberDocument, ");
                    sqlI.Append("@BirthDate); ");

                    MySqlCommand cmmI = new MySqlCommand(sqlI.ToString());

                    cmmI.Parameters.Add("@TravelPackageId", MySqlDbType.Int32).Value = packageOld.Id;
                    cmmI.Parameters.Add("@Name", MySqlDbType.VarChar).Value = x.Name;
                    cmmI.Parameters.Add("@NumberDocument", MySqlDbType.VarChar).Value = x.NumberDocument;
                    cmmI.Parameters.Add("@BirthDate", MySqlDbType.Date).Value = x.BirthDate;

                    ctx.ExecutaQuery(cmmI);

                });

                #endregion

                #endregion

                #region Tours

                #region Delete

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
                    sqlU.Append("DateHourStart = @DateHourStart ");
                    sqlU.Append("WHERE Id = @Id");

                    MySqlCommand cmmU = new MySqlCommand(sqlU.ToString());

                    cmmU.Parameters.Add("@DateHourStart", MySqlDbType.Date).Value = x.DateHourStart;

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
                    sqlI.Append("TravelPackageId,");
                    sqlI.Append("DateHourStart) ");
                    sqlI.Append("VALUES (");
                    sqlI.Append("@TourId, ");
                    sqlI.Append("@TravelPackageId,");
                    sqlI.Append("@DateHourStart); ");

                    MySqlCommand cmmI = new MySqlCommand(sqlI.ToString());

                    cmmI.Parameters.Add("@TravelPackageId", MySqlDbType.Int32).Value = x.TravelPackageId;
                    cmmI.Parameters.Add("@TourId", MySqlDbType.Int32).Value = x.TourId;
                    cmmI.Parameters.Add("@DateHourStart", MySqlDbType.DateTime).Value = x.DateHourStart;

                    ctx.ExecutaQuery(cmmI);

                });

                #endregion

                #endregion

                #region Bills

                #region Delete

                ///Smael: para cada item na lista old que não existe na lista new, exclui-se.
                foreach (BillReceive i in packageOld.Bills)
                {
                    if (!package.Bills.Exists(c => c.Id == i.Id))
                    {
                        ctx.ExecutaQuery("DELETE FROM BillReceive WHERE Id = " + i.Id);
                    }
                }

                #endregion

                #region Update

                //lista de updates
                List<BillReceive> updateBills = package.Bills.FindAll(i => i.Id > 0);

                updateBills.ToList().ForEach(x =>
                {
                    StringBuilder sqlU = new StringBuilder();

                    sqlU.Append("UPDATE BillReceive ");
                    sqlU.Append("SET ");
                    sqlU.Append("Amount = @Amount, ");
                    sqlU.Append("AmountReceived = @AmountReceived, ");
                    sqlU.Append("Concerning = @Concerning, ");
                    sqlU.Append("DueDate = @DueDate, ");
                    sqlU.Append("PayDay = @PayDay, ");
                    sqlU.Append("Comments = @Comments ");
                    sqlU.Append("WHERE Id = @Id;");

                    MySqlCommand cmmU = new MySqlCommand(sqlU.ToString());

                    cmmU.Parameters.Add("@Amount", MySqlDbType.Decimal).Value = x.Amount;
                    cmmU.Parameters.Add("@AmountReceived", MySqlDbType.Decimal).Value = x.AmountReceived;
                    cmmU.Parameters.Add("@Concerning", MySqlDbType.Text).Value = x.Concerning;
                    cmmU.Parameters.Add("@DueDate", MySqlDbType.Date).Value = x.DueDate;
                    cmmU.Parameters.Add("@PayDay", MySqlDbType.Date).Value = x.PayDay;
                    cmmU.Parameters.Add("@Comments", MySqlDbType.Text).Value = x.Comments;
                    cmmU.Parameters.Add("@Id", MySqlDbType.Int32).Value = x.Id;

                    ctx.ExecutaQuery(cmmU);
                });

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
                                    //.Include("Tours").Include("Tours.Tour")                               
                                    .Include("VehicleUsed")
                                    .Include("GuideTour")
                                    .Include("Customer")
                                    .Where(x => x.Id == id).FirstOrDefault();


            List<TravelPackageTour> pkTours = _context.TravelPackageTours
                .Include("Tour")
                .Where(y => y.TravelPackageId == id).ToList();

            return pk;
        }

        public List<TravelPackage> Get(string search)
        {
            StringBuilder sql = new StringBuilder();
            MySqlConn ctx = MySqlConn.GetInstancia();
            List<TravelPackage> packages = new List<TravelPackage>();

            sql.Append("SELECT travelpackage.Id, ");
            sql.Append("travelpackage.CompanyId, ");
            sql.Append("travelpackage.CreationDate, ");
            sql.Append("travelpackage.CustomerId, ");
            sql.Append("travelpackage.Host, ");
            sql.Append("travelpackage.QuantityTickets, ");
            sql.Append("travelpackage.VehicleUsedId, ");
            sql.Append("travelpackage.GuideTourId, ");
            sql.Append("travelpackage.AddictionalReservs, ");
            sql.Append("travelpackage.Comments, ");
            sql.Append("travelpackage.TotalAmount, ");
            sql.Append("customer.Name ");
            sql.Append("FROM travelpackage ");
            sql.Append("INNER JOIN customer ON travelpackage.CustomerId = customer.id ");
            sql.Append("WHERE customer.Name LIKE @name;");

            MySqlCommand cmm = new MySqlCommand(sql.ToString());

            cmm.Parameters.Add("@name", MySqlDbType.VarChar).Value = "%" + search + "%";

            MySqlDataReader dr = ctx.ExecutaQueryComLeitura(cmm);

            while (dr.Read())
            {
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
                    (int)dr["VehicleUsedId"],
                    null,
                    (int)dr["GuideTourId"],
                    null,
                    (decimal)dr["TotalAmount"],
                    dr.IsDBNull(dr.GetOrdinal("AddictionalReservs")) ? "" : (string)dr["AddictionalReservs"],
                    dr.IsDBNull(dr.GetOrdinal("Comments")) ? "" : (string)dr["Comments"]));
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

        public string PreBooking(int id)
        {
            //Smael: carrega o pacote a ser impresso.
            TravelPackage package = Get(id);

            using (var fileStream = new System.IO.FileStream("output.pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                var document = new iTextSharp.text.Document();
                var pdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fileStream);
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
                var image = iTextSharp.text.Image.GetInstance("Resources\\logo_vmc_pdf.jpg");
                image.ScaleToFit(150, 75);
                image.SetAbsolutePosition(20, 760);
                contentByte.AddImage(image);

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

                foreach (TravelPackageTour t in package.Tours)
                {
                    paragraph = new Paragraph(espacamentoNormal, t.DateStart.ToShortDateString() + " - " + t.Tour.Name, fontNormal);
                    paragraph.Alignment = Element.ALIGN_LEFT;
                    document.Add(paragraph);
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

                if (package.Bills.Count > 0)
                    paragraph = new Paragraph(espacamentoNormal, "Solicitamos depósito antecipado de 30% do total na conta da empresa até a data de " + package.Bills[0].DueDate.ToShortDateString() + " - para confirmação da reserva, e restante pagamento em Gramado no dia da chegada(04 / 04).", fontNormal);
                else
                    paragraph = new Paragraph(espacamentoNormal, "Solicitamos depósito antecipado de 30% do total na conta da empresa para confirmação da reserva, e restante pagamento em Gramado no dia da chegada(04 / 04).", fontNormal);

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
                System.Diagnostics.Process.Start("output.pdf");
            }

            return "";
        }

        public string BookingConfirmation(int id)
        {
            //Smael: carrega o pacote a ser impresso.
            TravelPackage package = Get(id);

            using (var fileStream = new System.IO.FileStream("output.pdf", System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                var document = new iTextSharp.text.Document();
                var pdfWriter = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fileStream);
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
                var image = iTextSharp.text.Image.GetInstance("Resources\\logo_vmc_pdf.jpg");
                image.ScaleToFit(150, 75);
                image.SetAbsolutePosition(20, 760);
                contentByte.AddImage(image);

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

                foreach (TravelPackageTour t in package.Tours)
                {
                    paragraph = new Paragraph(espacamentoNormal, t.DateStart.ToShortDateString() + " - " + t.Tour.Name, fontNormal);
                    paragraph.Alignment = Element.ALIGN_LEFT;
                    document.Add(paragraph);
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

                foreach (BillReceive b in package.Bills)
                {
                    paragraph = new Paragraph(espacamentoNormal, b.Concerning + "no valor de R$ " + Math.Round(b.Amount, 2, MidpointRounding.AwayFromZero) + " com vencimento em " + b.DueDate.ToShortDateString() + ". (" + b.Status + ")", fontNormal);
                    paragraph.Alignment = Element.ALIGN_LEFT;
                    document.Add(paragraph);
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
                System.Diagnostics.Process.Start("output.pdf");
            }

            return "";
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
