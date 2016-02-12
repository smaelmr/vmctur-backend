using System;
using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Passeios;
using VMCTur.Domain.Entities.Veiculos;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class PasseioRepository : IPasseioRepository
    {
        private AppDataContext _context;

        public PasseioRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(Passeio passeio)
        {
            _context.Passeios.Add(passeio);
            _context.SaveChanges();
        }

        public void Update(Passeio passeios)
        {
            _context.Entry<Passeio>(passeios).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Passeio passeios)
        {
            _context.Passeios.Remove(passeios);
            _context.SaveChanges();
        }

        public Passeio Get(int id)
        {
            return _context.Passeios.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Passeio> Get(string search)
        {
            return _context.Passeios.Where(x => x.Nome == search).ToList();
        }

        public List<Passeio> Get(int skip, int take)
        {
            return _context.Passeios.OrderBy(x => x.Nome).Skip(skip).Take(take).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
