using System;
using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Infra.Data;

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

        public void Update(TravelPackage package)
        {
            _context.Entry<TravelPackage>(package).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(TravelPackage package)
        {
            _context.TravelPackages.Remove(package);
            _context.SaveChanges();
        }

        public TravelPackage Get(int id)
        {
            return _context.TravelPackages
                                .Include("Participants")
                                .Include("Bills")
                                .Include("Tours").Include("Tours.Tour")
                                .Include("VehicleUsed")
                                .Include("GuideTour")
                                .Include("Customer")
                                .Where(x => x.Id == id).FirstOrDefault();

            //List<TravelPackage> pack = (from it in _context.TravelPackages
            // join participant in _context.TravelPackageParticipants on it.Id equals participant.TravelPackageId
            // join bill in _context.BillReceives on it.Id equals bill.TravelPackageId
            // join tour in _context.TravelPackageTours on it.Id equals tour.TravelPackageId
            // join vehicle in _context.Vehicles on it.VehicleUsedId equals vehicle.Id
            // join guideTour in _context.TourGuides on it.GuideTourId equals guideTour.Id
            // join customer in _context.Customers on it.CustomerId equals customer.Id
            // where it.Id == id
            // select new TravelPackage(
            //     it.Id,
            //     it.CompanyId,
            //     it.CreationDate,
            //     it.CustomerId,
            //     it.Customer,
            //     it.Participants,
            //     it.Tours,
            //     it.Bills,
            //     it.Host,
            //     it.QuantityTickets,
            //     it.VehicleUsedId,
            //     it.VehicleUsed,
            //     it.GuideTourId,
            //     it.GuideTour,
            //     it.TotalAmount,
            //     it.AddictionalReservs,
            //     it.Comments
            //)).ToList();

            //return pack[0];
        }

        public List<TravelPackage> Get(string search)
        {
            return _context.TravelPackages.Where(x => x.Comments.Contains(search)).ToList();
        }

        public List<TravelPackage> Get(int skip, int take)
        {
            return _context.TravelPackages                                
                                .OrderBy(x => x.CreationDate).Skip(skip).Take(take).ToList();
        }

        

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
