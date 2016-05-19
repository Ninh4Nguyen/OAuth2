using AuthorizationServer.API.Models;
using AuthorizationServer.API.Services;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens;

namespace AuthorizationServer.API.Formats
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private const string AudiencePropertyKey = "audience";
        private readonly string _issuer = string.Empty;
        private IAudienceServices _audienceServices;
        public CustomJwtFormat(IAudienceServices audienceServices, string issuer)
        {
            _audienceServices = audienceServices;
            _issuer = issuer;
        }
        public string Protect(AuthenticationTicket data)
        {
            if(data == null)
            {
                throw new ArgumentNullException("data");
            }

            string audienceId = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey) ? data.Properties.Dictionary[AudiencePropertyKey] : null;
            if(audienceId == null)
            {
                throw new InvalidOperationException(string.Format("AuthenticationTicket.Properties doesn't include {0}", AudiencePropertyKey));
            }

            Audience audience = _audienceServices.GetById(audienceId);
            string symetricKeyAsBase64 = audience.Base64Secret;
            var keyByteArray = TextEncodings.Base64Url.Decode(symetricKeyAsBase64);
            var signingKey = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
            return jwt;
        }
        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException(protectedText);
        }
    }
}