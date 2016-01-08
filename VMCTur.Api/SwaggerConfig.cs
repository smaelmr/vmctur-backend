using System.Web.Http;
using WebActivatorEx;
using VMCTur.Api;
using Swashbuckle.Application;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace VMCTur.Api
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "VmcTur.Api");
                })
            .EnableSwaggerUi(c =>
                {
                });
        }
    }
}