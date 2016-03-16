using System;
using System.Collections.Generic;
using VMCTur.Domain.Commands.VehicleCommands;
using VMCTur.Domain.Entities.Vehicles;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IVehicleService : IDisposable
    {
        void Create(CreateVehicleCommand vehicle);

        void Update(UpdateVehicleCommand vehicle);

        void Delete(int id);

        List<Vehicle> GetByRange(int skip, int take);

        List<Vehicle> GetBySearch(string search);

        Vehicle GetById(int id);
    }
}
