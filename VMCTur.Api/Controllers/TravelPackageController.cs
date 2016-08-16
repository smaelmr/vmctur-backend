using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using VMCTur.Api.Attributes;
using VMCTur.Domain.Commands.TravelPackageCommands.Create;
using VMCTur.Domain.Commands.TravelPackageCommands.Update;
using VMCTur.Domain.Contracts.Services;
//using WebApi.OutputCache.V2;

namespace VMCTur.Api.Controllers
{
    [RoutePrefix("api/pacote")]
    public class TravelPackageController : ApiController
    {
        private ITravelPackageService _service;

        public TravelPackageController(ITravelPackageService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Cria um novo Pacote.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("")]
        public Task<HttpResponseMessage> Post(CreateTravelPackageCommand package)   
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Create(package);

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
        /// Atualiza os dados do pacote.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("")]
        public Task<HttpResponseMessage> Put(UpdateTravelPackageCommand package)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Update(package);

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
        /// Deleta Pacote.
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
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
        public Task<HttpResponseMessage> GetById(int id)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetById(id);
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
        /// Busca os pacotes passando parametros de paginação.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getbyrange/{skip:int:min(0)}/{take:int:min(1)}")]
        [DeflateCompression]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
        public Task<HttpResponseMessage> GetByRange(int skip, int take)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetByRange(skip, take);
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
        /// Busca os pacotes passando campo para pesquisa, no momento somente pelo nome.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getbysearch")]
        [DeflateCompression]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
        public Task<HttpResponseMessage> Get(string search)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetBySearch(search);
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

        //[Authorize]
        [HttpGet]
        [Route("printprebooking")]
        //[DeflateCompression]
        public Task<HttpResponseMessage> PrintPreBooking(int id)
        {
            var pathRoot = HttpContext.Current.Server.MapPath("~");

            MemoryStream ms = _service.PrintPreBooking(id, pathRoot);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.GetBuffer())

            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
            {
                FileName = "PreReserva.pdf",
            };

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(result);
            return tsc.Task;
        }

        //[Authorize]
        [HttpGet]
        [Route("printbookingconfirmation")]
        //[DeflateCompression]
        public Task<HttpResponseMessage> PrintBookingConfirmation(int id)
        {
            var pathRoot = HttpContext.Current.Server.MapPath("~");

            MemoryStream ms = _service.PrintBookingConfirmation(id, pathRoot);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.GetBuffer())

            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
            {
                FileName = "ConfirmacaoReserva.pdf",
            };

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(result);
            return tsc.Task;
        }

        //[Authorize]
        [HttpGet]
        [Route("printvoucher")]
        //[DeflateCompression]
        public Task<HttpResponseMessage> PrintVoucher(int id)
        {
            var pathRoot = HttpContext.Current.Server.MapPath("~");

            MemoryStream ms = _service.PrintVoucher(id, pathRoot);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.GetBuffer())

            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
            {
                FileName = "Voucher.pdf",
            };

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(result);
            return tsc.Task;
        }

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
        }
    }
}