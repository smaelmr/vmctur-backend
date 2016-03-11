using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Vehicles;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IVeiculoService : IDisposable
    {
        void Create(int empresaId, string placa, int ano, string modelo, int capacidadePassageiros,
                    bool inativo, string tipoVinculo, string obs);

        void Update(int id, int empresaId, string placa, int ano, string modelo, int capacidadePassageiros,
                    bool inativo, string tipoVinculo, string obs);

        void Delete(int id);

        List<Vehicle> GetByRange(int skip, int take);

        List<Vehicle> GetBySearch(string search);

        Vehicle GetById(int id);
    }
}
