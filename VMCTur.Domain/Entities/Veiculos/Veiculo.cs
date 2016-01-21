using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Enum;

namespace VMCTur.Domain.Entities.Veiculos
{
    public class Veiculo
    {
        #region Properties

        public int Id { get; private set; }
        public int EmpresaId { get; private set; }
        public string Placa { get; private set; }
        public int Ano { get; private set; }
        public string Modelo { get; private set; }
        public int CapacidadePassageiros { get; private set; }
        public TipoVinculoVeiculo TipoAquisicao { get; private set; }
        public bool Inativo { get; private set; }
        public string Obs { get; private set; }

        #endregion

        #region Ctor

        public Veiculo(int id, int empresaId, string placa, int ano, string modelo, int capacidadePassageiros, 
                       bool inativo, TipoVinculoVeiculo tipoAquisicao, string obs)
        {
            Id = id;
            EmpresaId = empresaId;
            Placa = placa;
            Ano = ano;
            Modelo = modelo;
            CapacidadePassageiros = capacidadePassageiros;
            TipoAquisicao = tipoAquisicao;
            Inativo = inativo;
            Obs = obs;
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertArgumentNotEmpty(this.Placa, "A placa do veículo deve ser informada.");
            AssertionConcern.AssertArgumentLength(this.Placa, 7, 7, "Placa inválida.");
            AssertionConcern.AssertArgumentNotEmpty(this.Modelo, "A modelo do veículo deve ser informada.");
            AssertionConcern.AssertArgumentRange(this.Ano, DateTime.Today.Year - 50, DateTime.Today.Year + 1, "O ano do veículo é inválido.");
            AssertionConcern.AssertArgumentRange(this.CapacidadePassageiros, 1, 100, "O ano do veículo é inválido.");
        }
    
        #endregion
    }
}
