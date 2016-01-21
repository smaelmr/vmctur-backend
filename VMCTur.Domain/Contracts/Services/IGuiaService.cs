using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Guias;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IGuiaService : IDisposable
    {
        void Create(int empresaId, string cpf, string nome, string tipoVinculo, string obs);

        void Update(int id, int empresaId, string nome, string cpf, string tipoVinculo, string obs);

        void Delete(int id);

        List<Guia> GetByRange(int skip, int take);

        List<Guia> GetBySearch(string search);

        Guia GetById(int id);
    }
}
