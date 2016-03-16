using System;
using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Customers;
using VMCTur.Domain.Entities.TourGuides;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class GuiaRepository : IGuideRepository
    {
        private AppDataContext _context;

        public GuiaRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(TourGuide guia)
        {
            _context.TourGuides.Add(guia);
            _context.SaveChanges();
        }

        public void Update(TourGuide guia)
        {
            _context.Entry<TourGuide>(guia).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(TourGuide guia)
        {
            _context.TourGuides.Remove(guia);
            _context.SaveChanges();
        }

        public TourGuide Get(int id)
        {
            return _context.TourGuides.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<TourGuide> Get(string search)
        {
            return _context.TourGuides.Where(x => x.Nome.Contains(search)).ToList();
        }

        public List<TourGuide> Get(int skip, int take)
        {
            return _context.TourGuides.OrderBy(x => x.Nome).Skip(skip).Take(take).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
