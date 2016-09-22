using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VMCTur.Api.Attributes;
using VMCTur.Domain.Contracts.Services;
using WebApi.OutputCache.V2;

namespace VMCTur.Api.Controllers
{
    [RoutePrefix("api/agenda")]
    public class TourScheduleController : ApiController
    {
        private ITourScheduleService _service;

        public TourScheduleController(ITourScheduleService service)
        {
            _service = service;
        }

        /// <summary>
        /// lista os passeios dos próximos sete dias.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getnextsevendays")]
        [DeflateCompression]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> GetNextSevenDays()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetNextSevenDays();
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
        /// lista os passeios dos próximos quinze dias.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getnextfifteenDays")]
        [DeflateCompression]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> GetNextFifteenDays()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetNextFifteenDays();
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
        /// lista os passeios dos próximos trinta dias.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getNextthirtydays")]
        [DeflateCompression]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> GetNextThirtyDays()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetNextThirtyDays();
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
        /// lista os passeios dos próximos trinta dias.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getbyperiod")]
        [DeflateCompression]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> Get(DateTime startPeriod, DateTime finishPeriod)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.Get(startPeriod, finishPeriod);
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
        /// lista os passeios dos próximos trinta dias.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getall")]
        [DeflateCompression]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> GetAll()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetAll();
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

        [HttpGet]
        [Route("exportexcel")]
        public Task<HttpResponseMessage> ExportExcel(DateTime startPeriod, DateTime finishPeriod)
        {
            string exportData = string.Empty;

            exportData = _service.ExportExcel(startPeriod, finishPeriod);

            

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(exportData, System.Text.Encoding.UTF8)
            };


            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("inline")
            {
                FileName = "Programcao.html",
            };

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(result);
            return tsc.Task;

        } 
    }
}
