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

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
