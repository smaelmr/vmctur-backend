﻿using System;
using System.Collections.Generic;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Enums;
using VMCTur.Domain.Entities.Vehicles;

namespace VMCTur.Bussiness.Services
{
    public class VeiculoService : IVeiculoService
    {
        private IVeiculoRepository _repository;

        public VeiculoService(IVeiculoRepository repository)
        {
            _repository = repository;
        }

        public void Create(int empresaId, string placa, int ano, string modelo, int capacidadePassageiros,
                       bool inativo, string tipoVinculo, string obs)
        {
            TipoVinculoVeiculo vinculo = (TipoVinculoVeiculo)Enum.Parse(typeof(TipoVinculoVeiculo), tipoVinculo);

            var veiculo = new Vehicle(0, empresaId, placa, ano, modelo, capacidadePassageiros, inativo, vinculo, obs);

            veiculo.Validate();

            _repository.Create(veiculo);
        }

        public void Update(int id, int empresaId, string placa, int ano, string modelo, int capacidadePassageiros,
                       bool inativo, string tipoVinculo, string obs)
        {
            TipoVinculoVeiculo vinculo = (TipoVinculoVeiculo)Enum.Parse(typeof(TipoVinculoVeiculo), tipoVinculo);

            var veiculo = new Vehicle(id, empresaId, placa, ano, modelo, capacidadePassageiros, inativo, vinculo, obs);
            veiculo.Validate();

            _repository.Update(veiculo);
        }

        public void Delete(int id)
        {
            var veiculo = _repository.Get(id);

            _repository.Delete(veiculo);
        }

        public Vehicle GetById(int id)
        {
            var veiculo = _repository.Get(id);

            return veiculo;
        }

        public List<Vehicle> GetByRange(int skip, int take)
        {
            var veiculo = _repository.Get(skip, take);

            return veiculo;
        }

        public List<Vehicle> GetBySearch(string search)
        {
            var veiculo = _repository.Get(search);

            return veiculo;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }     
    }
}
