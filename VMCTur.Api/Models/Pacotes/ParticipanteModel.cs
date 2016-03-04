using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Api.Models.Pacotes
{
    public class ParticipanteModel
    {
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string NroDocumento { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public int PacoteId { get; private set; }        
    }
}