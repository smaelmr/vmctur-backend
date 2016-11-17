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
                    c.IncludeXmlComments(GetXmlCommentsPath());
                })
            .EnableSwaggerUi(c =>
                {
					
                });
        }

        protected static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\VMCTur.Api.XML", System.AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}