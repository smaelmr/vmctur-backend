using System;
using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Clientes;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private AppDataContext _context;

        public ClienteRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void Create(Cliente customer)
        {
            _context.Clientes.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Cliente customer)
        {
            _context.Entry<Cliente>(customer).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Cliente customer)
        {
            _context.Clientes.Remove(customer);
            _context.SaveChanges();
        }

        public Cliente Get(int id)
        {
            return _context.Clientes.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Cliente> Get(string search)
        {
            return _context.Clientes.Where(x => x.Nome == search).ToList();
        }

        public List<Cliente> Get(int skip, int take)
        {
            return _context.Clientes.OrderBy(x => x.Nome).Skip(skip).Take(take).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
