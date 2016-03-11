using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface ITravelPackageRepository : IDisposable
    {
        List<TravelPackage> Get(string search);
        TravelPackage Get(int id);
        List<TravelPackage> Get(int skip, int take);

        void Create(TravelPackage package);
        void Update(TravelPackage package);
        void Delete(TravelPackage package);
    }
}
