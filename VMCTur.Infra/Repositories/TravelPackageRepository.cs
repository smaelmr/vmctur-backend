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

            Document doc = new Document(PageSize.A4);
            doc.SetMargins(40, 40, 40, 80);
            doc.AddCreationDate();
            ;
            string caminho = @System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "PRE_RESERVA.pdf";
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));

            doc.Open();

            string dados = "";
            Paragraph paragrafo = new Paragraph(dados, new Font(Font.NORMAL, 14));

            paragrafo.Alignment = Element.ALIGN_JUSTIFIED;

            paragrafo.Add("A");
            paragrafo.Add(package.Customer.Name);

            paragrafo.Add("Segue abaixo DADOS PARA EFETUAÇÃO DE RESERVA:");

            paragrafo.Add("Serviços a serem prestados:");

            foreach (TravelPackageTour t in package.Tours)
            {
                paragrafo.Add(t.DateStart.ToShortDateString() + " - " + t.Tour.Name);
            }

            decimal amountPerPerson = package.TotalAmount / package.QuantityTickets;

            paragrafo.Add("Valor total por pessoa R$ " + Math.Round(amountPerPerson, 2, MidpointRounding.AwayFromZero) + "(base " + package.QuantityTickets + " pessoas)");

            paragrafo.Add("Valor total da reserva: R$ " + Math.Round(package.TotalAmount, 2, MidpointRounding.AwayFromZero) + " (Um mil oitocentos e setenta e seis reais)");

            paragrafo.Add("Formas de Pagamento: ");
            paragrafo.Add("Solicitamos depósito antecipado de 30% do total na conta da empresa até a data de 15 / 02 / 016 - para confirmação da reserva, e restante pagamento em Gramado no dia da chegada(04 / 04).");

            paragrafo.Add("Dados para depósito");
            paragrafo.Add("Cristiane Rosa:");
            paragrafo.Add("Banco Itaú");
            paragrafo.Add("Agência:");
            paragrafo.Add("1606");
            paragrafo.Add("Conta Corrente: 01802 - 4");
            paragrafo.Add("CNPJ: 09.396.197 / 0001 - 02");
            paragrafo.Add("Favorecido: Cristiane Lúcio da Rosa");

            paragrafo.Add("Ou");

            paragrafo.Add("Banco do Brasil");
            paragrafo.Add("Agencia 0575 - 4");
            paragrafo.Add("Conta corrente 115850 - 3");
            paragrafo.Add("CNPJ: 09.396.197 / 0001 - 02");
            paragrafo.Add("Favorecido:");
            paragrafo.Add("VMC TURISMO");

            paragrafo.Add("Obs * Enviar comprovante via FAX(54 3286 1209, ou via email: vmcturismo@gramadosite.com.br");

            paragrafo.Add("OBS *");
            paragrafo.Add("01) No traslado de retorno, saída de Gramado com no mínimo 04 horas de antecedência em relação ao voo, para evitar possíveis atrasos;");
            paragrafo.Add("02)	Os passeios poderão sofrer alteração no roteiro ou cancelamentos devido a condições climáticas ou fator de força maior que impeça aquela atividade;");
            paragrafo.Add("03)	Os horários dos passeios e transfers serão informados pela agência operadora sempre no dia que antecede o passeio ou o transfer, geralmente à tarde;");
            paragrafo.Add("04)	Todos os passeios e transfers envolvem planejamento e alocação de pessoas e equipamentos;");
            paragrafo.Add("05)	A agência reserva o direito de utilizar ônibus, micro-ônibus, vans e carros;");
            paragrafo.Add("06)	A ordem da execução dos roteiros poderá ser alterada, dependendo da data de chegada;");
            paragrafo.Add("07)	A agência não se responsabiliza por pertences deixados nos veículos;");
            paragrafo.Add("08)	A não realização de passeios e/ ou traslados não dá direito a restituição de valor, troca ou remuneração para outra data;");
            paragrafo.Add("09)	Não haverá tolerância no horário marcado para saída dos passeios e / ou traslados, por isso observar o horário para sua saída do hotel;");
            paragrafo.Add("10) O local de encontro é sempre na recepção do hotel e quando não encontrado, a agência se reserva o direito por respeito aos demais clientes, de dar continuidade ao passeio ou transfer;");
            paragrafo.Add("11)	Os passeios e ou/ traslados acima é em base regular, ou seja, o valor é correspondente ao serviço realizado em grupo;");
            paragrafo.Add("12)	O carro da agência buscará e deixará o passageiro no hotel onde o mesmo se encontra hospedado em Gramado ou Canela;");
            paragrafo.Add("13)	Atividade sujeita à alteração de datas, em função da programação interna da agência;");
            paragrafo.Add("14)	Referente a cancelamentos:");
            paragrafo.Add("Antecedência de 05(cinco) a 20(vinte) dias da viagem, multa de 50 % sobre o valor dos serviços de receptivo;");
            paragrafo.Add("Antecedência de 04(quatro) ou menos dias da viagem, multa de 100 % sobre o valor dos serviços de receptivo;");
            paragrafo.Add("Para os períodos compreendidos entre Junho, Julho, Natal Luz, Feriados, Congressos e demais eventos, a multa poderá ser de até 100 % sobre o valor total da reserva, independente de prazo de antecedência.");
            paragrafo.Add("15)	O contratante é obrigado a comunicar no ato da contratação de quaisquer serviços se ha passageiro(s)com necessidades especiais;");
            paragrafo.Add("16)	Poderá haver substituição de veículos e Guia / Motorista nos traslados e / ou passeios;");
            paragrafo.Add("17)	Os traslados de ida e volta serão realizados com todos juntos conforme contrato, caso houver alteração no voo de 01 ou mais passageiros não haverá reembolso de valores, o(s)passageiro(s) poderá(ão) optar por um ou mais traslados extras com valores à parte;");
            paragrafo.Add("18)	Valores com base em 07 pessoas, caso venha diminuir ou aumentar, consultar valores com a agência;");
            paragrafo.Add("19)	É expressamente proibido o consumo de bebidas alcoólicas dentro do veículo;");
            paragrafo.Add("20) É OBRIGATÓRIO PORTAR DOCUMENTO DE IDENTIDADE NOS TRASLADOS E / OU PASSEIOS.CRIANÇAS QUE NÃO POSSUÍREM CARTEIRA DE IDENTIDADE DEVERÃO PORTAR SUA CERTIDÃO DE NASCIMENTO.EM CASO DE NÃO ESTAREM PORTANDO SUA IDENTIDADE E O VEÍCULO FOR NOTIFICADO PELO ÓRGÃO DE TRÂNSITO O CONTRATANTE FICA RESPONSÁVEL POR QUAISQUER MULTAS QUE VIREM A SER APLICADAS PELO NÃO CUMPRIMENTO DESTA OBRIGATORIEDADE.");


            paragrafo.Add("Atenciosamente");
            paragrafo.Add("VMC Turismo");
            paragrafo.Add("Cristiane Rosa");

            doc.Add(paragrafo);

            doc.Close();

            

            return caminho;
        }
    

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
