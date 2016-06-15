using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Tours;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class TourRepository : ITourRepository
    {
        private AppDataContext _context;

        public TourRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(Tour passeio)
        {
            _context.Tours.Add(passeio);
            _context.SaveChanges();
        }

        public void Update(Tour passeios)
        {
            _context.Entry<Tour>(passeios).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Tour passeios)
        {
            _context.Tours.Remove(passeios);
            _context.SaveChanges();
        }

        public Tour Get(int id)
        {
            return _context.Tours.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Tour> Get(string search)
        {
            List<Tour> tours;

            if (!string.IsNullOrEmpty(search))
                tours = _context.Tours.Where(x => x.Name.Contains(search)).OrderBy(o => o.Name).ToList();
            else
                tours = _context.Tours.OrderBy(o => o.Name).ToList();

            return tours == null ? new List<Tour>() : tours;
        }

        public List<Tour> Get(int skip, int take)
        {
            return _context.Tours.OrderBy(x => x.Name).Skip(skip).Take(take).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
