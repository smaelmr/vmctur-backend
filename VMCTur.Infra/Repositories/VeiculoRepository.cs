using System;
using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Vehicles;
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

        public void Create(Vehicle veiculo)
        {
            _context.Vehicles.Add(veiculo);
            _context.SaveChanges();
        }

        public void Update(Vehicle veiculo)
        {
            _context.Entry<Vehicle>(veiculo).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Vehicle veiculo)
        {
            _context.Vehicles.Remove(veiculo);
            _context.SaveChanges();
        }

        public Vehicle Get(int id)
        {
            return _context.Vehicles.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Vehicle> Get(string search)
        {
            return _context.Vehicles.Where(x => x.Placa == search).ToList();
        }

        public List<Vehicle> Get(int skip, int take)
        {
            return _context.Vehicles.OrderBy(x => x.Placa).Skip(skip).Take(take).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
