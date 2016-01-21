using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Api.Models.Clientes
{
    public class UpdateClienteModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public string Comments { get; set; }
    }
}