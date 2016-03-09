using VMCTur.Common.Resources;
using VMCTur.Common.Validation;
using VMCTur.Domain.Entities.Enum;

namespace VMCTur.Domain.Entities.Guias
{
    /// <summary>
    /// Smael: guia também pode ser o motorista.
    /// </summary>
    public class Guia
    {
        #region Properties

        public int Id { get; private set; }
        public int EmpresaId { get; private set; }
        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public TipoVinculoGuia TipoVinculo { get; private set; }
        public string Obs { get; private set; }

        #endregion

        #region Ctor

        protected Guia()
        { }

        public Guia(int id, int empresaId, string nome, string cpf, TipoVinculoGuia tipoVinculo, string obs)
        {
            Id = id;
            EmpresaId = empresaId;
            Nome = nome;
            Cpf = cpf;
            TipoVinculo = tipoVinculo;
            Obs = obs;
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertArgumentLength(this.Nome, 3, 100, Errors.InvalidName);            
        }

        #endregion
    }
}
