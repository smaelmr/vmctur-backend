using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Customers;

namespace VMCTur.Domain.Contracts.Services
{
    public interface ICustomerService : IDisposable
    {
        void Create(int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs);

        void Update(int id, int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs);

        void Delete(int id);

        List<Customer> GetBithDayOfMonth();
        List<Customer> GetBithDayOfDay();
        List<Customer> GetByRange(int skip, int take);
        List<Customer> GetBySearch(string search);

        Customer GetById(int id);

    }
}
