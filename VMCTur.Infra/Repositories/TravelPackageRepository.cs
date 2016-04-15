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
            TravelPackage pk =  _context.TravelPackages
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
