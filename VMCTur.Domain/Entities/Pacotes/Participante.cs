using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Common.Resources;
using VMCTur.Common.Validation;

namespace VMCTur.Domain.Entities.Pacotes
{
    public class Participante
    {
        #region Properties

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string NroDocumento { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public int PacoteId { get; private set; }
        public Pacote Pacote { get; private set; }

        #endregion

        #region Ctor

        protected Participante()
        { }

        public Participante(int id, string nome, string nroDocumento, DateTime dataNascimento, int pacoteId)
        {
            Id = id;
            Nome = nome;
            NroDocumento = nroDocumento;
            DataNascimento = dataNascimento;
            PacoteId = pacoteId;
        }

        #endregion

        #region Methods

        public void Validate()
        {
            AssertionConcern.AssertArgumentLength(this.Nome, 3, 100, Errors.InvalidName);
            BirthdayAssertionConcern.AssertIsValid(this.DataNascimento);
            AssertionConcern.AssertArgumentNotEmpty(this.NroDocumento, "Número do documento deve ser informado.");
        }
    


        #endregion
    }
}
