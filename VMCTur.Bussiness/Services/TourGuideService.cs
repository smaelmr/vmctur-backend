﻿using System;
using System.Collections.Generic;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Enums;
using VMCTur.Domain.Entities.TourGuides;

namespace VMCTur.Bussiness.Services
{
    public class TourGuideService : IGuideService
    {
        private IGuideRepository _repository;

        public TourGuideService(IGuideRepository repository)
        {
            _repository = repository;
        }

        public void Create(int companyId, string nome, string cpf, string tipoVinculo, string obs)
        {
            TypeBondGuide vinculo = (TypeBondGuide)Enum.Parse(typeof(TypeBondGuide), tipoVinculo);

            var guia = new TourGuide(0, companyId, nome, cpf, vinculo, true, obs);
            guia.Validate();

            _repository.Create(guia);
        }

        public void Update(int id, int empresaId, string nome, string cpf, string tipoVinculo, string obs)
        {
            TypeBondGuide vinculo = (TypeBondGuide)Enum.Parse(typeof(TypeBondGuide), tipoVinculo);

            var guia = new TourGuide(id, empresaId, nome, cpf, vinculo, true, obs);
            guia.Validate();

            _repository.Update(guia);
        }

        public void Delete(int id)
        {
            var guia = _repository.Get(id);

            _repository.Delete(guia);
        }

        public TourGuide GetById(int id)
        {
            var guia = _repository.Get(id);

            return guia;
        }

        public List<TourGuide> GetByRange(int skip, int take)
        {
            var customers = _repository.Get(skip, take);

            return customers;
        }

        public List<TourGuide> GetBySearch(string search)
        {
            var customers = _repository.Get(search);

            return customers;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }       
    }
}
