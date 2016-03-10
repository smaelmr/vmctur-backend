using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IPacoteRepository : IDisposable
    {
        List<TravelPackage> Get(string search);
        TravelPackage Get(int id);
        List<TravelPackage> Get(int skip, int take);

        void Create(TravelPackage pacote);
        void Update(TravelPackage pacote);
        void Delete(TravelPackage pacote);
    }
}
