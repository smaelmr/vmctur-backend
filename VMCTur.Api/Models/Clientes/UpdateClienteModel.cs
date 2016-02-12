using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Api.Models.Clientes
{
    public class UpdateClienteModel
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Fone { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Obs { get; set; }
    }
}