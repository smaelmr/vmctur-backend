using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Api.Models.Guias
{
    public class CreateGuiaModel
    {
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Vinculo { get; set; }
        public string Obs { get; set; }
    }
}