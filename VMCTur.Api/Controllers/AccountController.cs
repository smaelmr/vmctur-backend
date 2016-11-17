using VMCTur.Api.Models.Account;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Common.Resources;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VMCTur.Api.Attributes;
using WebApi.OutputCache.V2;

namespace VMCTur.Api.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private IUserService _service;

        public AccountController(IUserService service)
        {
            this._service = service;
        }

        /// <summary>
        /// Register a new user on application
        /// </summary>
        /// <param name="model">New user to register</param>
        /// <remarks>Adds new user to application and grant access</remarks>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("")]
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> Post(RegisterUserModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.Register(model.CompanyId, model.Name, model.Email, model.Password, model.ConfirmPassword);
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

        [Authorize]
        [HttpPut]
        [Route("")]
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> Put(ChangeInformationModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.ChangeInformation(User.Identity.Name, model.Name);
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
        /// Altera a senha do usuário.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("changepassword")]
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> ChangePassword(ChangePasswordModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                _service.ChangePassword(User.Identity.Name, model.Password, model.NewPassword, model.ConfirmNewPassword);
                response = Request.CreateResponse(HttpStatusCode.OK, Messages.PasswordSuccessfulyChanges);
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
        /// Recria a senha do usuário caso ele esqueça.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("resetpassword")]
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> ResetPassword(ResetPasswordModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var password = _service.ResetPassword(model.Email);
                response = Request.CreateResponse(HttpStatusCode.OK, String.Format(Messages.ResetPasswordEmailBody, password));
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
        /// Lista os usuários cadastros na aplicação.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("")]
        [DeflateCompression]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)] //Install-Package Strathweb.CacheOutput.WebApi2
        //[EnableCors(origins: "http://vmctur.azurewebsites.net", headers: "*", methods: "*")]
        public Task<HttpResponseMessage> Get()
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                var result = _service.GetByRange(0, 25);
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