using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Api.Models.Guias
{
    public class UpdateGuiaModel
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string BondType { get; set; }
        public string Comments { get; set; }
    }
}