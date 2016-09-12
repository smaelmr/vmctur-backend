using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VMCTur.Api.Attributes;
using VMCTur.Domain.Commands.ReservationCommands;
using VMCTur.Domain.Contracts.Services;

namespace VMCTur.Api.Controllers
{
    [RoutePrefix("api/reserva")]
    public class ReservationController : ApiController
    {
        private IReservationService _service;

        public ReservationController(IReservationService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Cria uma nova reserva.
        /// </summary>
        /// <param name="reserve"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("")]
        public Task<HttpResponseMessage> Post(CreateReservationCommand reserve)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Create(reserve);

                response = Request.CreateResponse(HttpStatusCode.OK, new { message = "Pacote cadastrado com sucesso" });
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        /// <summary>
        /// Atualiza os dados da reserva.
        /// </summary>
        /// <param name="reserve"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("")]
        public Task<HttpResponseMessage> Put(UpdateReservationCommand reserve)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Update(reserve);

                response = Request.CreateResponse(HttpStatusCode.OK, new { message = "Pacote alterado com sucesso" });
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        /// <summary>
        /// Deleta a reserva.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route("")]
        public Task<HttpResponseMessage> Delete(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Delete(id);
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        /// <summary>
        /// Busca o pacote conforme id.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getbyid")]
        [DeflateCompression]
        public Task<HttpResponseMessage> GetById(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.Get(id);
                response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }

        /// <summary>
        /// Busca as reservas passando campo para pesquisa, no momento somente pelo nome.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getbysearch")]
        [DeflateCompression]
        public Task<HttpResponseMessage> Get(string search)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.Get(search);
                response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}