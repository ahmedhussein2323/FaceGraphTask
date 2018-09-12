using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FaceGraphTask.Core.Entities;
using FaceGraphTask.Infrastructure.DbContext;
using Microsoft.Owin.Security.OAuth;
using Utilities.Security;

namespace FaceGraphTask.FBWeb.Models.AuthOptions
{
    public class MyOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity("otc");
            var id = context.OwinContext.Get<Guid>("otc:id");
            var email = context.OwinContext.Get<string>("otc:email");

            var currUser = new DocumentDbRepository<User>("User").GetItemAsync(id, "");

            identity.AddClaim(new Claim(ClaimTypes.Email, email));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Role, currUser.Role));
            context.Validated(identity);
            return Task.FromResult(0);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            try
            {
                var email = context.Parameters["email"];
                var password = context.Parameters["password"];
                var userEmail = new DocumentDbRepository<User>("User").GetItemsAsync(e => e.Email.ToLower() == email.ToLower());
                var enumerable = userEmail as IList<User> ?? userEmail.ToList();
                if (userEmail != null && enumerable.Any())
                {
                    var user = enumerable.ToArray()[0];
                    if (PasswordHash.ValidatePassword(password, user.Password))
                    {
                        context.OwinContext.Set("otc:email", email);
                        context.OwinContext.Set("otc:id", user.Id);
                        context.Validated();
                        return Task.FromResult(0);
                    }
                }
                else
                {
                    context.SetError("Wrong Email");
                    context.Rejected();
                }
            }
            catch (Exception ex)
            {
                context.SetError("Server error " + ex);
                context.Rejected();
            }
            context.SetError("Wrong Password");
            context.Rejected();
            return Task.FromResult(0);
        }
    }
}