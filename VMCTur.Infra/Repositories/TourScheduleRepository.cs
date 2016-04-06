using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            List<TourSchedule> schedules = (from it in _context.TravelPackageTours
                                            join travelPackage in _context.TravelPackages on it.TravelPackageId equals travelPackage.Id
                                            join customer in _context.Customers on travelPackage.CustomerId equals customer.Id
                                            join tourGuide in _context.TourGuides on travelPackage.GuideTourId equals tourGuide.Id
                                            join vehicle in _context.Vehicles on travelPackage.VehicleUsedId equals vehicle.Id
                                            join tour in _context.Tours on it.TourId equals tour.Id
                                            where it.DateHourStart >= startPeriod && it.DateHourStart <= finishPeriod
                                            select new TourSchedule()
                                            {
                                                DateHourTour = it.DateHourStart,
                                                CustomerName = customer.Name,
                                                QuantityParticipants = travelPackage.QuantityTickets,
                                                TourNamePasseio = tour.Name,
                                                TourGuidename = tourGuide.Name,
                                                VehicleModel = vehicle.Model
                                            }).ToList();

            return schedules;
        }

        public List<TourSchedule> Get(double days)
        {
            DateTime nextDate = DateTime.Now.AddDays(days);

            List<TourSchedule> schedules = (from it in _context.TravelPackageTours
                                            join travelPackage in _context.TravelPackages on it.TravelPackageId equals travelPackage.Id
                                            join customer in _context.Customers on travelPackage.CustomerId equals customer.Id
                                            join tourGuide in _context.TourGuides on travelPackage.GuideTourId equals tourGuide.Id
                                            join vehicle in _context.Vehicles on travelPackage.VehicleUsedId equals vehicle.Id
                                            join tour in _context.Tours on it.TourId equals tour.Id
                                            where it.DateHourStart >= DateTime.Now && it.DateHourStart <= nextDate
                                            select new TourSchedule()
                                            {
                                                DateHourTour = it.DateHourStart,
                                                CustomerName = customer.Name,
                                                QuantityParticipants = travelPackage.QuantityTickets,
                                                TourNamePasseio = tour.Name,
                                                TourGuidename = tourGuide.Name,
                                                VehicleModel = vehicle.Model
                                            }).ToList();

            return schedules;

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
