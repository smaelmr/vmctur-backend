using System;
using System.Collections.Generic;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Clientes;

namespace VMCTur.Bussiness.Services
{
    public class ClienteService : IClienteService
    {
        private IClienteRepository _customerRepository;

        public ClienteService(IClienteRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public void Create(int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs)
        {
            var customer = new Cliente(0, empresaId, nome, email, fone, rg, cpf, dataNascimento, obs);
            customer.Validate();

            _customerRepository.Create(customer);
        }

        public void Update(int id, int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs)
        {
            var customer = new Cliente(id, empresaId, nome, email, fone, rg, cpf, dataNascimento, obs);
            customer.Validate();

            _customerRepository.Update(customer);
        }

        public void Delete(int id)
        {
            var customer = _customerRepository.Get(id);

            _customerRepository.Delete(customer);
        }

        public Cliente GetById(int id)
        {
            var customer = _customerRepository.Get(id);

            return customer;
        }

        public List<Cliente> GetByRange(int skip, int take)
        {
            var customers = _customerRepository.Get(skip, take);

            return customers;
        }

        public List<Cliente> GetBySearch(string search)
        {
            var customers = _customerRepository.Get(search);

            return customers;
        }

        public void Dispose()
        {
            _customerRepository.Dispose();
        }     
    }
}
