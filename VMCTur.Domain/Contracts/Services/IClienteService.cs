using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Clientes;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IClienteService : IDisposable
    {
        void Create(int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs);

        void Update(int id, int empresaId, string nome, string email, string fone, string rg, string cpf, DateTime dataNascimento, string obs);

        void Delete(int id);

        List<Cliente> GetByRange(int skip, int take);

        List<Cliente> GetBySearch(string search);

        Cliente GetById(int id);

    }
}
