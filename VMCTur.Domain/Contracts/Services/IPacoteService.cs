using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Pacotes;
using VMCTur.Domain.Entities.Passeios;

namespace VMCTur.Domain.Contracts.Services
{
    public interface IPacoteService : IDisposable
    {
        void Create(int empresaId, int clienteId, List<Participantes> participantes, List<Passeio> passeio,
                    DateTime datahoraPartida, string hotelHospedagem, string quantidadeBilhetes,
                    int veiculoUtilizadoId, int guiaPasseioId, double valorTotal, DateTime dataPgamentoSinal,
                    double valorPagamentoSinal, string condicãoPagamentoRestante,
                    string reservasAdicionais, string observações);

        void Update(int id, int empresaId, int clienteId, List<Participantes> participantes, List<Passeio> passeio,
                    DateTime datahoraPartida, string hotelHospedagem, string quantidadeBilhetes,
                    int veiculoUtilizadoId, int guiaPasseioId, double valorTotal, DateTime dataPgamentoSinal,
                    double valorPagamentoSinal, string condicãoPagamentoRestante,
                    string reservasAdicionais, string observações);

        void Delete(int id);

        List<Pacote> GetByRange(int skip, int take);

        List<Pacote> GetBySearch(string search);

        Pacote GetById(int id);

    }
}
