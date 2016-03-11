using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.TourGuides;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IGuiaService : IDisposable
    {
        void Create(int empresaId, string cpf, string nome, string tipoVinculo, string obs);

        void Update(int id, int empresaId, string nome, string cpf, string tipoVinculo, string obs);

        void Delete(int id);

        List<TourGuide> GetByRange(int skip, int take);

        List<TourGuide> GetBySearch(string search);

        TourGuide GetById(int id);
    }
}
