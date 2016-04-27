using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface ITravelPackageParticipantRepository : IDisposable
    {
        void Create(TravelPackageParticipant participant);
        void Update(TravelPackageParticipant participant);
        void Delete(TravelPackageParticipant participant);
    }
}
