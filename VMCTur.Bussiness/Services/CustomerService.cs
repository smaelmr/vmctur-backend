using System;
using System.Collections.Generic;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Customers;

namespace VMCTur.Bussiness.Services
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void Create(int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs)
        {
            var customer = new Customer(0, empresaId, nome, email, fone, rg, cpf, dataNascimento, obs);
            customer.Validate();

            _customerRepository.Create(customer);
        }

        public void Update(int id, int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs)
        {
            var customer = new Customer(id, empresaId, nome, email, fone, rg, cpf, dataNascimento, obs);
            customer.Validate();

            _customerRepository.Update(customer);
        }

        public void Delete(int id)
        {
            var customer = _customerRepository.Get(id);

            _customerRepository.Delete(customer);
        }

        public Customer GetById(int id)
        {
            var customer = _customerRepository.Get(id);

            return customer;
        }

        public List<Customer> GetByRange(int skip, int take)
        {
            var customers = _customerRepository.Get(skip, take);

            return customers;
        }

        public List<Customer> GetBySearch(string search)
        {
            var customers = _customerRepository.Get(search);

            return customers;
        }

        public List<Customer> GetBithDayOfMonth()
        {
            return _customerRepository.GetBithDayOfMonth();
        }

        public List<Customer> GetBithDayOfDay()
        {
            return _customerRepository.GetBithDayOfDay();
        }

        public void Dispose()
        {
            _customerRepository.Dispose();
        }     
    }
}
