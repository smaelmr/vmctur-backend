using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMCTur.Api.Models.Pacotes
{
    public class UpdatePacoteModel
    {
        public int Id { get; private set; }
        public int EmpresaId { get; private set; }
        public int ClienteId { get; private set; }

        public List<ParticipanteModel> Participantes { get; private set; }
        public List<PasseioModel> Passeios { get; private set; }

        public DateTime DatahoraPartida { get; private set; }
        public string HotelHospedagem { get; private set; }
        public string QuantidadeBilhetes { get; private set; }

        public int VeiculoUtilizadoId { get; private set; }
        public int GuiaPasseioId { get; private set; }

        public double ValorTotal { get; private set; }
        public DateTime DataPagamentoSinal { get; private set; }
        public double ValorPagamentoSinal { get; private set; }
        public string CondicãoPagamentoRestante { get; private set; }
        public string ReservasAdicionais { get; private set; }
        public string Observacoes { get; private set; }
    }
}