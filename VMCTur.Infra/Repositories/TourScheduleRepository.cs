using System;
using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class TourScheduleRepository : ITourScheduleRepository
    {
        private AppDataContext _context;

        public TourScheduleRepository(AppDataContext context)
        {
            this._context = context;
        }

        public List<TourSchedule> Get(DateTime startPeriod, DateTime finishPeriod)
        {
            DateTime start = new DateTime(startPeriod.Year, startPeriod.Month, startPeriod.Day, 0, 0, 0);
            DateTime finish = new DateTime(finishPeriod.Year, finishPeriod.Month, finishPeriod.Day, 23, 59, 59);

            List<TourSchedule> schedules = (from it in _context.TravelPackageTours
                                            join travelPackage in _context.TravelPackages on it.TravelPackageId equals travelPackage.Id
                                            join customer in _context.Customers on travelPackage.CustomerId equals customer.Id
                                            join tourGuide in _context.TourGuides on it.GuideTourId equals tourGuide.Id
                                            join vehicle in _context.Vehicles on it.VehicleUsedId equals vehicle.Id
                                            join tour in _context.Tours on it.TourId equals tour.Id
                                            where it.DateHourStart >= start && it.DateHourStart <= finish
                                            orderby it.DateHourStart
                                            select new TourSchedule()
                                            {
                                                DateHourTour = it.DateHourStart,
                                                CustomerName = customer.Name,                                                
                                                TourNamePasseio = tour.Name,
                                                TourGuidename = tourGuide.Name,
                                                VehicleModel = vehicle.Model,
                                                TourComments = it.Comments,
                                                Shared = it.Shared,
                                                QuantityAdult = travelPackage.QuantityAdult,
                                                QuantityChild = travelPackage.QuantityChild,
                                                QuantityElderly = travelPackage.QuantityElderly                                                

                                            }).ToList();

            return schedules;
        }

        public string ExportExcel(DateTime startPeriod, DateTime finishPeriod)
        {
            var items = Get(startPeriod, finishPeriod);

            System.Text.StringBuilder html = new System.Text.StringBuilder();

            html.AppendLine("<HTML>");

            html.AppendLine("<style>");
            html.AppendLine("table, td, th, tfoot {border: solid 1px #000; padding:5px;}");
            html.AppendLine("th {background-color:#999;}");
            html.AppendLine("caption {font-size:x-large;}");
            html.AppendLine("colgroup {background:#F60;}");
            html.AppendLine(".coluna1 {");
            html.AppendLine("background:#F66;}");
            html.AppendLine(".coluna2  {");
            html.AppendLine("background:#F33;}");
            html.AppendLine(".coluna3  {");
            html.AppendLine("background:#F99;}");
            html.AppendLine("</style>");


            html.AppendLine("<TABLE>");
            html.AppendLine("<caption>VMC TURISMO</caption>");
            html.AppendLine("<thead>");
            html.AppendFormat("<tr><th colspan=\"8\">PROGRAMACAO DE PASSEIOS DE {0} a {1} </th></tr>", startPeriod.ToShortDateString(), finishPeriod.ToShortDateString());
            html.AppendLine("<tr>");
            html.AppendLine("<th>Data/Hora</th>");
            html.AppendLine("<th>Cliente</th>");
            html.AppendLine("<th>Participantes</th>");
            html.AppendLine("<th>Passeio</th>");
            html.AppendLine("<th>Guia</th>");
            html.AppendLine("<th>Veículo</th>");
            html.AppendLine("<th>Tipo</th>");
            html.AppendLine("<th>Obs</th>");

            html.AppendLine("</tr>");
            html.AppendLine("</thead>");
            html.AppendLine("<tbody>");

            foreach (var it in items)
            {
                html.AppendFormat("<tr bgcolor=\"{0}\">", it.ColorOfDay);
                html.AppendFormat("<td>{0}</td>", it.DateHourTour);
                html.AppendFormat("<td>{0}</td>", it.CustomerName);
                html.AppendFormat("<td>{0}</td>", it.QuantityParticipantsDetails);
                html.AppendFormat("<td>{0}</td>", it.TourNamePasseio);
                html.AppendFormat("<td>{0}</td>", it.TourGuidename);
                html.AppendFormat("<td>{0}</td>", it.VehicleModel);
                html.AppendFormat("<td>{0}</td>", it.Shared ? "Compartilhado" : "Privado");
                html.AppendFormat("<td>{0}</td>", it.TourComments);
                html.AppendFormat("</tr>");
            }

            html.AppendLine("</tbody>");
            html.AppendLine("</TABLE>");
            html.AppendLine("</HTML>");
            
            return html.ToString();
        }

        /// <summary>
        /// Get just schedules this day on.
        /// </summary>
        /// <returns></returns>
        public List<TourSchedule> GetAll()
        {

            List<TourSchedule> schedules = (from it in _context.TravelPackageTours
                                            join travelPackage in _context.TravelPackages on it.TravelPackageId equals travelPackage.Id
                                            join customer in _context.Customers on travelPackage.CustomerId equals customer.Id
                                            join tourGuide in _context.TourGuides on it.GuideTourId equals tourGuide.Id
                                            join vehicle in _context.Vehicles on it.VehicleUsedId equals vehicle.Id
                                            join tour in _context.Tours on it.TourId equals tour.Id
                                            where it.DateHourStart >= DateTime.Today
                                            orderby it.DateHourStart
                                            select new TourSchedule()
                                            {
                                                DateHourTour = it.DateHourStart,
                                                CustomerName = customer.Name,                                                
                                                TourNamePasseio = tour.Name,
                                                TourGuidename = tourGuide.Name,
                                                VehicleModel = vehicle.Model,
                                                TourComments = it.Comments,
                                                Shared = it.Shared,
                                                QuantityAdult = travelPackage.QuantityAdult,
                                                QuantityChild = travelPackage.QuantityChild,
                                                QuantityElderly = travelPackage.QuantityElderly
                                            }).ToList();

            return schedules;

        }

        public List<TourSchedule> Get(double days)
        {
            DateTime nextDate = DateTime.Now.AddDays(days);

            List<TourSchedule> schedules = (from it in _context.TravelPackageTours
                                            join travelPackage in _context.TravelPackages on it.TravelPackageId equals travelPackage.Id
                                            join customer in _context.Customers on travelPackage.CustomerId equals customer.Id
                                            join tourGuide in _context.TourGuides on it.GuideTourId equals tourGuide.Id
                                            join vehicle in _context.Vehicles on it.VehicleUsedId equals vehicle.Id
                                            join tour in _context.Tours on it.TourId equals tour.Id                                            
                                            where it.DateHourStart >= DateTime.Now && it.DateHourStart <= nextDate
                                            orderby it.DateHourStart
                                            select new TourSchedule()
                                            {
                                                DateHourTour = it.DateHourStart,
                                                CustomerName = customer.Name,                                                
                                                TourNamePasseio = tour.Name,
                                                TourGuidename = tourGuide.Name,
                                                VehicleModel = vehicle.Model,
                                                TourComments = it.Comments,
                                                Shared = it.Shared,
                                                QuantityAdult = travelPackage.QuantityAdult,
                                                QuantityChild = travelPackage.QuantityChild,
                                                QuantityElderly = travelPackage.QuantityElderly
                                            }).ToList();

            return schedules;

        }

        public List<TourSchedule> GetToursByContractNumber(string contractNumber)
        {

            List<TourSchedule> schedules = (from it in _context.TravelPackageTours
                                            join travelPackage in _context.TravelPackages on it.TravelPackageId equals travelPackage.Id
                                            join customer in _context.Customers on travelPackage.CustomerId equals customer.Id
                                            join tourGuide in _context.TourGuides on it.GuideTourId equals tourGuide.Id
                                            join vehicle in _context.Vehicles on it.VehicleUsedId equals vehicle.Id
                                            join tour in _context.Tours on it.TourId equals tour.Id
                                            where it.ContractNumber == contractNumber
                                            orderby it.DateHourStart
                                            select new TourSchedule()
                                            {
                                                DateHourTour = it.DateHourStart,
                                                CustomerName = customer.Name,
                                                TourNamePasseio = tour.Name,
                                                TourGuidename = tourGuide.Name,
                                                VehicleModel = vehicle.Model,
                                                TourComments = it.Comments,
                                                Shared = it.Shared,
                                                QuantityTickets = it.QuantityTickets,
                                                QuantityAdult = travelPackage.QuantityAdult,
                                                QuantityChild = travelPackage.QuantityChild,
                                                QuantityElderly = travelPackage.QuantityElderly
                                            }).ToList();

            return schedules;

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
