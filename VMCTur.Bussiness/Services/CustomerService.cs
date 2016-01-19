using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Models.Customers;

namespace VMCTur.Bussiness.Services
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void Create(int companyId, string name, string email, string phoneNumber, string rg, string cpf, DateTime birthDate, string comments)
        {
            var customer = new Customer(0, companyId, name, email, phoneNumber, rg, cpf, birthDate, comments);
            customer.Validate();

            _customerRepository.Create(customer);
        }

        public void Update(int id, int companyId, string name, string email, string phoneNumber, string rg, string cpf, DateTime birthDate, string comments)
        {
            var customer = new Customer(id, companyId, name, email, phoneNumber, rg, cpf, birthDate, comments);
            customer.Validate();

            _customerRepository.Update(customer);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Customer GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetByRange(int skip, int take)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetBySearch(string search)
        {
            throw new NotImplementedException();
        }
    }
}
