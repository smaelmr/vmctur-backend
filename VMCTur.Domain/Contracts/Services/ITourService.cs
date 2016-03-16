using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Tours;

namespace VMCTur.Domain.Contracts.Services
{
    public interface ITourService : IDisposable
    {
        void Create(int empresaId, string nome, string roteiro, string horarioAbertura,
                       string horarioFechamento, bool inativo, string obs);

        void Update(int id, int empresaId, string nome, string roteiro, string horarioAbertura,
                       string horarioFechamento, bool inativo, string obs);

        void Delete(int id);

        List<Tour> GetByRange(int skip, int take);

        List<Tour> GetBySearch(string search);

        Tour GetById(int id);
    }
}
