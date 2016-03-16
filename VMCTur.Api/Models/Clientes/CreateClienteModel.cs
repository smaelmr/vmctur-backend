using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Api.Models.Clientes
{
    public class CreateClienteModel
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public string Comments { get; set; }
    }
}