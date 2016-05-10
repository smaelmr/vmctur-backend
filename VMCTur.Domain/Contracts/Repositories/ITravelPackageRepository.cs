using System;
using System.Collections.Generic;
using System.IO;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface ITravelPackageRepository : IDisposable
    {
        List<TravelPackage> Get(string search);
        TravelPackage Get(int id);
        List<TravelPackage> Get(int skip, int take);

        void Create(TravelPackage package);
        void Update(TravelPackage package, TravelPackage packageOld);
        void Delete(TravelPackage package);

        MemoryStream PrintPreBooking(TravelPackage package, string url);
        MemoryStream PrintBookingConfirmation(TravelPackage package, string url);
        MemoryStream PrintVoucher(TravelPackage package, string url);
    }
}
