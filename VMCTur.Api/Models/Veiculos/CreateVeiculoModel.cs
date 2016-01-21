using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Api.Models.Veiculos
{
    public class CreateVeiculoModel
    {
        public int EmpresaId { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public int CapacidadePassageiros { get; set; }
        public bool Inativo { get; set; }
        public string Vinculo { get; set; }
        public string Obs { get; set; }
    }
}