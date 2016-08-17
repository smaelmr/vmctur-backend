using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VMCTur.Api.Attributes;
using VMCTur.Domain.Commands.BillCommands.BillReceiveCommands;
using VMCTur.Domain.Contracts.Services;
using WebApi.OutputCache.V2;

namespace VMCTur.Api.Controllers
{
    [RoutePrefix("api/conta-receber")]
    public class BillReceiveController : ApiController
    {
        private IBillReceiveService _service;

        public BillReceiveController(IBillReceiveService service)
        {
            this._service = service;
        }

        ///// <summary>
        ///// Cria uma nova conta a receber.
        ///// </summary>
        ///// <param name="command"></param>
        ///// <returns></returns>
        //[Authorize]
        //[HttpPost]
        //[Route("")]
        //public Task<HttpResponseMessage> Post(CreateBillReceiveCommand command)
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();

        //    try
        //    {
        //        _service.Create(command);
        //        response = Request.CreateResponse(HttpStatusCode.OK, new { name = "Conta inserida com sucesso." });
        //    }
        //    catch (Exception ex)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
        //    }

        //    var tsc = new TaskCompletionSource<HttpResponseMessage>();
        //    tsc.SetResult(response);
        //    return tsc.Task;
        //}

        /// <summary>
        /// Atualiza os dados da conta.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("")]
        public Task<HttpResponseMessage> Put(UpdateBillReceiveCommand command)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Update(command);
                response = Request.CreateResponse(HttpStatusCode.OK, new { name = "Dados alterados com sucesso." });
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
        /// Delete conta.
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
        /// Faz o recebimento da conta.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("receipt")]
        public Task<HttpResponseMessage> Receipt(ReceiptBillReceiveCommand command)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Receipt(command);
                response = Request.CreateResponse(HttpStatusCode.OK, new { name = "Conta quitada com sucesso." });
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
        /// Busca a conta conforme id.
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
        /// Busca as contas passando parametros de paginação.
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
                var result = _service.Get(skip, take);
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
        /// Busca as contas passando um valor para pesquisa, pesquisando pelo status.
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

        /// <summary>
        /// Busca as contas recebidas passando um periodo por parametro, filtra pela data de recebimento.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getrecevedbills")]
        [DeflateCompression]
        public Task<HttpResponseMessage> GetRecevedBills(DateTime startPeriod, DateTime finishPeriod)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetReceivedBills(startPeriod, finishPeriod);
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
        /// Smael: lista todas as contas independente de status conforme o período informado.
        /// </summary>
        /// <param name="startPeriod"></param>
        /// <param name="finishPeriod"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getall")]
        [DeflateCompression]
        public Task<HttpResponseMessage> GetAll(DateTime startPeriod, DateTime finishPeriod)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetAll(startPeriod, finishPeriod);
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
        /// Smael: lista as contas vencendo hoje.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getwinningtodaybills")]
        [DeflateCompression]
        public Task<HttpResponseMessage> GetWinningTodayBills()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetWinningTodayBills();
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
        /// Smael: lista as contas a vencer.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("gettowinbills")]
        [DeflateCompression]
        public Task<HttpResponseMessage> GetToWinBills()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetToWinBills();
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
        /// Smael: lista as contas vencidas.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getoverduebills")]
        [DeflateCompression]
        public Task<HttpResponseMessage> GetOverdueBills()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetOverdueBills();
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
        /// Smael: listas as contas em aberto (que ainda não foram pagas independente de estarem vencidas ou não) conforme periodo informado.
        /// </summary>
        /// <param name="startPeriod"></param>
        /// <param name="finishPeriod"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getopenbills")]
        [DeflateCompression]
        public Task<HttpResponseMessage> GetOpenBills(DateTime startPeriod, DateTime finishPeriod)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetOpenBills(startPeriod, finishPeriod);
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

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
        }
    }
}