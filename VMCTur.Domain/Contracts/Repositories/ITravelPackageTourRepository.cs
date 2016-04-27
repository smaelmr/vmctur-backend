using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface ITravelPackageTourRepository : IDisposable
    {
        void Create(TravelPackageTour tour);
        void Update(TravelPackageTour tour);
        void Delete(TravelPackageTour tour);
    }
}
