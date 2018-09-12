﻿using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.OAuth;

namespace FaceGraphTask.FBWeb.Models.AuthOptions
{
    public class MyJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly OAuthAuthorizationServerOptions _options;

        public MyJwtFormat(OAuthAuthorizationServerOptions options)
        {
            _options = options;
        }

        public string SignatureAlgorithm => "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";

        public string DigestAlgorithm => "http://www.w3.org/2001/04/xmlenc#sha256";

        public string Protect(AuthenticationTicket data)
        {
            if (data == null) throw new ArgumentNullException("data");

            var issuer = "localhost";
            var audience = "all";
            var key = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["secret"]);
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_options.AccessTokenExpireTimeSpan.TotalMinutes);
            var signingCredentials = new System.IdentityModel.Tokens.SigningCredentials(
                                        new InMemorySymmetricSecurityKey(key),
                                        SignatureAlgorithm,
                                        DigestAlgorithm);
            var token = new JwtSecurityToken(issuer, audience, data.Identity.Claims,
                                             now, expires, signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }

}