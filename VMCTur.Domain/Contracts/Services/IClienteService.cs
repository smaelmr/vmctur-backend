using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Customers;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IClienteService : IDisposable
    {
        void Create(int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs);

        void Update(int id, int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs);

        void Delete(int id);

        List<Customer> GetByRange(int skip, int take);

        List<Customer> GetBySearch(string search);

        Customer GetById(int id);

    }
}
