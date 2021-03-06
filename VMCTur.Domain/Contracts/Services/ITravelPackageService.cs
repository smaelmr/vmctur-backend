﻿using System;
using System.Collections.Generic;
using System.IO;
using VMCTur.Domain.Commands.TravelPackageCommands.Create;
using VMCTur.Domain.Commands.TravelPackageCommands.Update;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Domain.Contracts.Services
{
    public interface ITravelPackageService : IDisposable
    {
        void Create(CreateTravelPackageCommand travelPackage);

        void Update(UpdateTravelPackageCommand travelPackage);

        void Delete(int id);

        List<TravelPackage> GetByRange(int skip, int take);

        List<TravelPackage> GetBySearch(string search);

        TravelPackage GetById(int id);

        MemoryStream PrintPreBooking(int id, string url);
        MemoryStream PrintBookingConfirmation(int id, string url);
        MemoryStream PrintVoucher(int id, string url);

    }
}
