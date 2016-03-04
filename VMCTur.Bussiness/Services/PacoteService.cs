using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Domain.Entities.Pacotes;
using VMCTur.Domain.Entities.Passeios;

namespace VMCTur.Bussiness.Services
{
    public class PacoteService : IPacoteService
    {
        private IPacoteRepository _pacoteRepository;

        public PacoteService(IPacoteRepository pacoteRepository)
        {
            _pacoteRepository = pacoteRepository;
        }

        public void Create(int empresaId, int clienteId, List<Participante> participantes, List<Passeio> passeios, DateTime datahoraPartida, string hotelHospedagem, 
                           string quantidadeBilhetes, int veiculoUtilizadoId, int guiaPasseioId, double valorTotal, DateTime dataPagamentoSinal, 
                           double valorPagamentoSinal, string condicaoPagamentoRestante, string reservasAdicionais, string observacoes)
        {
            var pacote = new Pacote(0, empresaId, clienteId, participantes, passeios, datahoraPartida, hotelHospedagem,
                                    quantidadeBilhetes, veiculoUtilizadoId, guiaPasseioId, valorTotal, dataPagamentoSinal,
                                    valorPagamentoSinal, condicaoPagamentoRestante, reservasAdicionais, observacoes);

            pacote.Validate();

            _pacoteRepository.Create(pacote);
        }

        public void Update(int id, int empresaId, int clienteId, List<Participante> participantes, List<Passeio> passeios, DateTime datahoraPartida, 
                           string hotelHospedagem, string quantidadeBilhetes, int veiculoUtilizadoId, int guiaPasseioId, double valorTotal, DateTime dataPagamentoSinal, 
                           double valorPagamentoSinal, string condicaoPagamentoRestante, string reservasAdicionais, string observacoes)
        {
            var pacote = new Pacote(id, empresaId, clienteId, participantes, passeios, datahoraPartida, hotelHospedagem,
                                    quantidadeBilhetes, veiculoUtilizadoId, guiaPasseioId, valorTotal, dataPagamentoSinal,
                                    valorPagamentoSinal, condicaoPagamentoRestante, reservasAdicionais, observacoes);

            pacote.Validate();

            _pacoteRepository.Create(pacote);
        }

        public void Delete(int id)
        {
            var pacote = _pacoteRepository.Get(id);

            _pacoteRepository.Delete(pacote);
        }                

        public Pacote GetById(int id)
        {
            var pacote = _pacoteRepository.Get(id);

            return pacote;
        }

        public List<Pacote> GetByRange(int skip, int take)
        {
            var pacote = _pacoteRepository.Get(skip, take);

            return pacote;
        }

        public List<Pacote> GetBySearch(string search)
        {
            var pacote = _pacoteRepository.Get(search);

            return pacote;
        }        

        public void Dispose()
        {
            _pacoteRepository.Dispose();
        }
    }
}
