using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Clientes;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IClienteService : IDisposable
    {
        void Create(int companyId, string name, string email, string phoneNumber, string rg, string cpf, DateTime birthDate, string comments);

        void Update(int id, int companyId, string name, string email, string phoneNumber, string rg, string cpf, DateTime birthDate, string comments);

        void Delete(int id);

        List<Cliente> GetByRange(int skip, int take);

        List<Cliente> GetBySearch(string search);

        Cliente GetById(int id);

    }
}
