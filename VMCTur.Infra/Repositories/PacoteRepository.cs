using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Pacotes;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class PacoteRepository : IPacoteRepository
    {
        private AppDataContext _context;

        public PacoteRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(Pacote pacote)
        {
            _context.Pacotes.Add(pacote);
            _context.SaveChanges();
        }

        public void Update(Pacote pacote)
        {
            _context.Entry<Pacote>(pacote).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Pacote pacote)
        {
            _context.Pacotes.Remove(pacote);
            _context.SaveChanges();
        }

        public Pacote Get(int id)
        {
            return _context.Pacotes.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Pacote> Get(string search)
        {
            return _context.Pacotes.Where(x => x.Observacoes == search).ToList();
        }

        public List<Pacote> Get(int skip, int take)
        {
            return _context.Pacotes.OrderBy(x => x.DatahoraPartida).Skip(skip).Take(take).ToList();
        }               

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
