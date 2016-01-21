using System;
using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Veiculos;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class VeiculoRepository : IVeiculoRepository
    {
        private AppDataContext _context;

        public VeiculoRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(Veiculo veiculo)
        {
            _context.Veiculos.Add(veiculo);
            _context.SaveChanges();
        }

        public void Update(Veiculo veiculo)
        {
            _context.Entry<Veiculo>(veiculo).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Veiculo veiculo)
        {
            _context.Veiculos.Remove(veiculo);
            _context.SaveChanges();
        }

        public Veiculo Get(int id)
        {
            return _context.Veiculos.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Veiculo> Get(string search)
        {
            return _context.Veiculos.Where(x => x.Placa == search).ToList();
        }

        public List<Veiculo> Get(int skip, int take)
        {
            return _context.Veiculos.OrderBy(x => x.Placa).Skip(skip).Take(take).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
