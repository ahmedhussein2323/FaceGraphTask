using System.Configuration;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;

namespace FaceGraphTask.FBWeb.Models.AuthOptions
{
    public class MyJwtOptions : JwtBearerAuthenticationOptions
    {
        public MyJwtOptions()
        {
            var issuer = "localhost";
            var audience = "all";
            var key = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["secret"]);

            AllowedAudiences = new[] { audience };
            IssuerSecurityTokenProviders = new[]
            {
            new SymmetricKeyIssuerSecurityTokenProvider(issuer, key)
            };
        }
    }

}