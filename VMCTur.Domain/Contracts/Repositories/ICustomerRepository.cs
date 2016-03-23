using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Customers;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface ICustomerRepository : IDisposable
    {
        List<Customer> Get(string search);
        Customer Get(int id);
        List<Customer> Get(int skip, int take);

        List<Customer> GetBithDayOfMonth();
        List<Customer> GetBithDayOfWeek();
        List<Customer> GetBithDayOfDay();

        void Create(Customer cliente);
        void Update(Customer cliente);
        void Delete(Customer cliente);
    }
}
