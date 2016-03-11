using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Resources;
using VMCTur.Common.Validation;

namespace VMCTur.Domain.Entities.Tours

{
    public class Tour
    {
        #region Properties

        public int Id { get; private set; }
        public int EmpresaId { get; private set; }
        public string Nome { get; private set; }
        public string Roteiro { get; private set; }
        public TimeSpan HorarioAbertura { get; private set; }
        public TimeSpan HorarioFechamento { get; private set; }
        public bool Inativo { get; private set; }
        public string Obs { get; private set; }

        #endregion

        #region Ctor

        protected Tour()
        { }

        public Tour(int id, int empresaId, string nome, string roteiro, TimeSpan horarioAbertura,
                       TimeSpan horarioFechamento, bool inativo, string obs)
        {
            Id = id;
            EmpresaId = empresaId;
            Nome = nome;
            Roteiro = roteiro;
            HorarioAbertura = horarioAbertura;
            HorarioFechamento = horarioFechamento;
            Inativo = inativo;
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
