using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Models.Customers;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface ICustomerRepository
    {
        List<Customer> Get(string search);
        Customer Get(int id);
        List<Customer> Get(int skip, int take);

        void Create(Customer user);
        void Update(Customer user);
        void Delete(Customer user);
    }
}
