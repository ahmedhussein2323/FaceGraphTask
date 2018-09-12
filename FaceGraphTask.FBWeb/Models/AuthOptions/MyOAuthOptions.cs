using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace FaceGraphTask.FBWeb.Models.AuthOptions
{
    public class MyOAuthOptions : OAuthAuthorizationServerOptions
    {
        public MyOAuthOptions()
        {
            TokenEndpointPath = new PathString("/api/auth/login/getToken");
            AccessTokenExpireTimeSpan = TimeSpan.FromDays(600);
            AccessTokenFormat = new MyJwtFormat(this);
            Provider = new MyOAuthProvider();
            AllowInsecureHttp = true;
        }
    }
}