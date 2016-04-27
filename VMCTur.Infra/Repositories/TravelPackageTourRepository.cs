using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class TravelPackageTourRepository : ITravelPackageTourRepository
    {
        private AppDataContext _context;

        public TravelPackageTourRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(TravelPackageTour tour)
        {
            _context.TravelPackageTours.Add(tour);
            _context.SaveChanges();
        }

        public void Update(TravelPackageTour tour)
        {            
            _context.Entry<TravelPackageTour>(tour).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();            
        }

        public void Delete(TravelPackageTour tour)
        {
            _context.TravelPackageTours.Remove(tour);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
