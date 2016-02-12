using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Passeios;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IPasseioService : IDisposable
    {
        void Create(int empresaId, string nome, string roteiro, string horarioAbertura,
                       string horarioFechamento, bool inativo, string obs);

        void Update(int id, int empresaId, string nome, string roteiro, string horarioAbertura,
                       string horarioFechamento, bool inativo, string obs);

        void Delete(int id);

        List<Passeio> GetByRange(int skip, int take);

        List<Passeio> GetBySearch(string search);

        Passeio GetById(int id);
    }
}
