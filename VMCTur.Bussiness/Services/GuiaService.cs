using System;
using System.Collections.Generic;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Enums;
using VMCTur.Domain.Entities.TourGuides;

namespace VMCTur.Bussiness.Services
{
    public class GuiaService : IGuiaService
    {
        private IGuiaRepository _repository;

        public GuiaService(IGuiaRepository customerRepository)
        {
            _repository = customerRepository;
        }

        public void Create(int empresaId, string nome, string cpf, string tipoVinculo, string obs)
        {
            TipoVinculoGuia vinculo = (TipoVinculoGuia)Enum.Parse(typeof(TipoVinculoGuia), tipoVinculo);

            var guia = new TourGuide(0, empresaId, nome, cpf, vinculo, obs);
            guia.Validate();

            _repository.Create(guia);
        }

        public void Update(int id, int empresaId, string nome, string cpf, string tipoVinculo, string obs)
        {
            TipoVinculoGuia vinculo = (TipoVinculoGuia)Enum.Parse(typeof(TipoVinculoGuia), tipoVinculo);

            var guia = new TourGuide(id, empresaId, nome, cpf, vinculo, obs);
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
