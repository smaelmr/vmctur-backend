using Microsoft.Owin.Security.OAuth;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Common.Resources;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace VMCTur.Api.Security
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserService _service;

        public AuthorizationServerProvider(IUserService service)
        {
            _service = service;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                var user = _service.Authenticate(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", Errors.InvalidCredentials);
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.Email));
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

                GenericPrincipal principal = new GenericPrincipal(identity, null);
                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch(Exception ex)
            {
                context.SetError("invalid_grant", Errors.InvalidCredentials);
            }
        }
    }
}