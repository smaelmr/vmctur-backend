using System;
using VMCTur.Common.Standard;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Enums;

namespace VMCTur.Domain.Entities.Vehicles
{
    public class Vehicle
    {
        #region Properties

        public int Id { get; private set; }
        public int CompanyId { get; private set; }
        public string Plate { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public TypeAcquisition TypeAcquisition { get; private set; }
        public int NumberOfPassengers { get; private set; }
        public bool Inactive { get; private set; }        
        public string Comments { get; private set; }

        public string TypeAcquisitionDisplay
        {
            get
            {
                return Standard.ObterDescricaoEnum(TypeAcquisition);
            }
        }

        #endregion

        #region Ctor

        protected Vehicle()
        { }

        public Vehicle(int id, int empresaId, string placa, int ano, string modelo, int capacidadePassageiros, 
                       bool inativo, TypeAcquisition tipoAquisicao, string obs)
        {
            Id = id;
            CompanyId = empresaId;
            Plate = placa;
            Year = ano;
            Model = modelo;
            NumberOfPassengers = capacidadePassageiros;
            TypeAcquisition = tipoAquisicao;
            Inactive = inativo;
            Comments = obs;
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertArgumentNotEmpty(this.Plate, "A placa do veículo deve ser informada.");
            AssertionConcern.AssertArgumentLength(this.Plate, 8, 8, "Placa inválida.");
            AssertionConcern.AssertArgumentNotEmpty(this.Model, "A modelo do veículo deve ser informada.");
            AssertionConcern.AssertArgumentRange(this.Year, DateTime.Today.Year - 50, DateTime.Today.Year + 1, "O ano do veículo é inválido.");
            AssertionConcern.AssertArgumentRange(this.NumberOfPassengers, 1, 100, "A capacidade de passageiros é inválido!.");
        }
    
        #endregion
    }
}
