using System;
using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Clientes;
using VMCTur.Domain.Entities.Guias;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class GuiaRepository : IGuiaRepository
    {
        private AppDataContext _context;

        public GuiaRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(Guia guia)
        {
            _context.Guias.Add(guia);
            _context.SaveChanges();
        }

        public void Update(Guia guia)
        {
            _context.Entry<Guia>(guia).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Guia guia)
        {
            _context.Guias.Remove(guia);
            _context.SaveChanges();
        }

        public Guia Get(int id)
        {
            return _context.Guias.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Guia> Get(string search)
        {
            return _context.Guias.Where(x => x.Nome == search).ToList();
        }

        public List<Guia> Get(int skip, int take)
        {
            return _context.Guias.OrderBy(x => x.Nome).Skip(skip).Take(take).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
