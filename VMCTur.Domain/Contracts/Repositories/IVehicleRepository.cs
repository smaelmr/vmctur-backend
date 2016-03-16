using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Vehicles;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IVehicleRepository : IDisposable
    {
        List<Vehicle> Get(string search);
        Vehicle Get(int id);
        List<Vehicle> Get(int skip, int take);

        void Create(Vehicle veiculo);
        void Update(Vehicle veiculo);
        void Delete(Vehicle veiculo);
    }
}
