﻿using System;
using System.Collections.Generic;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Enum;
using VMCTur.Domain.Entities.Passeios;
using VMCTur.Domain.Entities.Veiculos;

namespace VMCTur.Bussiness.Services
{
    public class PasseioService : IPasseioService
    {
        private IPasseioRepository _repository;

        public PasseioService(IPasseioRepository repository)
        {
            _repository = repository;
        }

        public void Create(int empresaId, string nome, string roteiro, string horarioAbertura,
                       string horarioFechamento, bool inativo, string obs)
        {
            TimeSpan horarioAb = TimeSpan.Parse(horarioAbertura);
            TimeSpan horarioFe = TimeSpan.Parse(horarioFechamento);

            var paseio = new Passeio(0, empresaId, nome, roteiro, horarioAb, horarioFe, inativo, obs);
            paseio.Validate();

            _repository.Create(paseio);
        }

        public void Update(int id, int empresaId, string nome, string roteiro, string horarioAbertura,
                       string horarioFechamento, bool inativo, string obs)
        {
            TimeSpan horarioAb = TimeSpan.Parse(horarioAbertura);
            TimeSpan horarioFe = TimeSpan.Parse(horarioFechamento);

            var passeio = new Passeio(id, empresaId, nome, roteiro, horarioAb, horarioFe, inativo, obs);
            passeio.Validate();

            _repository.Update(passeio);
        }

        public void Delete(int id)
        {
            var passeio = _repository.Get(id);

            _repository.Delete(passeio);
        }

        public Passeio GetById(int id)
        {
            var passeio = _repository.Get(id);

            return passeio;
        }

        public List<Passeio> GetByRange(int skip, int take)
        {
            var passeio = _repository.Get(skip, take);

            return passeio;
        }

        public List<Passeio> GetBySearch(string search)
        {
            var passeio = _repository.Get(search);

            return passeio;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }     
    }
}