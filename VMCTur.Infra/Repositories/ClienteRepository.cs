﻿using System;
using System.Collections.Generic;
using System.Linq;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Customers;
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

        public void Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _context.Entry<Customer>(customer).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Customer customer)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        public Customer Get(int id)
        {
            return _context.Customers.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Customer> Get(string search)
        {
            return _context.Customers.Where(x => x.Nome == search).ToList();
        }

        public List<Customer> Get(int skip, int take)
        {
            return _context.Customers.OrderBy(x => x.Nome).Skip(skip).Take(take).ToList();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
