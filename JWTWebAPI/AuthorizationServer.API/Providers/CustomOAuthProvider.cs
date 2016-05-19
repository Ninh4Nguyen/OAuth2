using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security;
using AuthorizationServer.API.Services;
using AuthorizationServer.API.Models;
using AuthorizationServer.API.Repository;

namespace AuthorizationServer.API.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private IUnitOfWork _unitOfWork;
        private IAudienceServices _audienceServices;
        private IUserServices _userServices;
        public CustomOAuthProvider(IUnitOfWork unitOfWork, IAudienceServices audienceServices, IUserServices userServices)
        {
            _unitOfWork = unitOfWork;
            _audienceServices = audienceServices;
            _userServices = userServices;
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            if(!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            if(context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_id is not set");
                return Task.FromResult<object>(null);
            }

            var audience = _audienceServices.GetById(context.ClientId);
            if(audience == null)
            {
                context.SetError("invalid_clientId", string.Format("Invalid ClientId: {0}", context.ClientId));
                return Task.FromResult<object>(null);
            }
            context.Validated();
            return Task.FromResult<object>(null);
        }
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            //Validate User
            User user = _userServices.FindUser(context.UserName, context.Password);
            if(user == null)
            {
                context.SetError("invalid_grant", "The user or password is incorrect!");
                return Task.FromResult<object>(null);
            }
            else
            {
                if(user.Audience != null)
                {
                    if(user.Audience.ClientId != context.ClientId)
                    {
                        context.SetError("invalid_client", "this client does not have this user!");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            string role = string.Empty;
            if(user.Role != null)
            {
                role = user.Role.Name;
            }
            identity.AddClaim(new Claim(ClaimTypes.Role, role));

            var property = new AuthenticationProperties(new Dictionary<string, string>
            {
                {
                    "audience", (context.ClientId ?? string.Empty)
                }
            });

            var ticket = new AuthenticationTicket(identity, property);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }
    }
}