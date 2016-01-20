using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VMCTur.Api.Attributes;
using VMCTur.Api.Models.Customer;
using VMCTur.Domain.Contracts.Services;
using WebApi.OutputCache.V2;

namespace VMCTur.Api.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        private ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Create a new customer.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("")]        
        public Task<HttpResponseMessage> Post(CreateCustomerModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Create(model.CompanyId, model.Name, model.Email, model.PhoneNumber, model.Rg, model.Cpf, model.BirthDate, model.Comments);
                response = Request.CreateResponse(HttpStatusCode.OK, new { name = model.Name, email = model.Email });
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
        /// Update customer's data.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route("")]
        public Task<HttpResponseMessage> Put(UpdateCustomerModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Update(model.Id, model.CompanyId, model.Name, model.Email, model.PhoneNumber, model.Rg, model.Cpf, model.BirthDate, model.Comments);
                response = Request.CreateResponse(HttpStatusCode.OK, new { name = model.Name });
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
        /// Get customers added.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getbyid")]
        [DeflateCompression]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
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
        /// Get customers by range.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getbyrange/{skip:int:min(0)}/{take:int:min(1)}")]
        [DeflateCompression]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
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
        /// Get customers by search value.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getbysearch")]
        [DeflateCompression]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
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

        protected override void Dispose(bool disposing)
        {
            _service.Dispose();
        }
    }
}