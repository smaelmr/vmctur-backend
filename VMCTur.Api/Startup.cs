using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using System.Web.Http.Cors;
using VMCTur.Api.Helpers;
using VMCTur.Api.Security;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Startup;

namespace VMCTur.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            HttpConfiguration config = new HttpConfiguration();

            
            // Configure Dependency Injection
            var container = new UnityContainer();
            DependencyResolver.Resolve(container);
            config.DependencyResolver = new UnityResolver(container);

            // Swagger
            SwaggerConfig.Register(config);

            // Habilitando CORS com politica global
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            ConfigureWebApi(config);

            // Habiliando o cors do OWIN
            app.UseCors(CorsOptions.AllowAll);

            ConfigureOAuth(app, container.Resolve<IUserService>());

            app.UseWebApi(config);
        }

        private void ConfigureCors(IAppBuilder app)
        {
            var politica = new CorsPolicy();

            politica.AllowAnyHeader = true;
            politica.AllowAnyMethod = true;
            politica.AllowAnyOrigin = true;

            politica.Origins.Add("http://app.ontur.com.br");
            politica.Origins.Add("http://app-homologacao.ontur.com.br");

            politica.Methods.Add("GET");
            politica.Methods.Add("POST");

            var corsOptions = new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context => Task.FromResult(politica)                    
                }
            };

            app.UseCors(corsOptions);
        }

        public static void ConfigureWebApi(HttpConfiguration config)
        {            

            // Remove o XML
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            // Modifica a identação
            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Modifica a serialização
            formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public void ConfigureOAuth(IAppBuilder app, IUserService service)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/security/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider(service),                               
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}