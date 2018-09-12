using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using FaceGraphTask.FBWeb;
using FaceGraphTask.FBWeb.Models.AuthOptions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof(Startup))]
namespace FaceGraphTask.FBWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            App.Init();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseAutofacMiddleware(App.Container);
            app.UseAutofacMvc();
            app.UseOAuthAuthorizationServer(new MyOAuthOptions());
            app.UseJwtBearerAuthentication(new MyJwtOptions());
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                SlidingExpiration = true,
                ExpireTimeSpan = new TimeSpan(0, 300, 0),
                CookieName = "faceGraphCookie",
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnResponseSignedIn = ctx =>
                    {
                        ctx.Options.SlidingExpiration = true;
                        ctx.Options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    },
                    OnApplyRedirect = ctx =>
                    {
                        if (IsAjaxRequest(ctx.Request))
                            return;
                        ctx.Response.Redirect(HttpUtility.HtmlDecode($"{ctx.RedirectUri}"));
                    },
                    OnValidateIdentity = MyCustomValidateIdentity
                }
            });
            DependencyResolver.SetResolver(new AutofacDependencyResolver(App.Container));
            //webApi
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            config.DependencyResolver = App.WebApiResolver;
            config.EnableSwagger(x => x.SingleApiVersion("v1", "FaceGraphTask.Web.Api")).EnableSwaggerUi();

            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            app.UseWebApi(config);
        }
        private static bool IsAjaxRequest(IOwinRequest request)
        {
            //var query = request.Query;
            //if ((query != null) && (query["X-Requested-With"] == "XMLHttpRequest"))
            if (request.Uri.ToString().Contains("/api"))
            {
                return true;
            }
            var headers = request.Headers;
            return ((headers != null) && (headers["X-Requested-With"] == "XMLHttpRequest"));
        }

        private static Task MyCustomValidateIdentity(CookieValidateIdentityContext context)
        {
            // validate security stamp for 'sign out everywhere'
            // here I want to verify the security stamp in every 100 seconds.
            // but I choose not to regenerate the identity cookie, so I passed in NULL 
            //var stampValidator = SecurityStampValidator.OnValidateIdentity<MyUserManager<Myuser>.MyUser>(TimeSpan.FromSeconds(100), null);
            //stampValidator.Invoke(context);

            // here we get the cookie expiry time
            var expireUtc = context.Properties.ExpiresUtc;

            // add the expiry time back to cookie as one of the claims, called 'myExpireUtc'
            // to ensure that the claim has latest value, we must keep only one claim
            // otherwise we will be having multiple claims with same type but different values
            var claimType = "myExpireUtc";
            var identity = context.Identity;
            if (identity.HasClaim(c => c.Type == claimType))
            {
                var existingClaim = identity.FindFirst(claimType);
                identity.RemoveClaim(existingClaim);
            }
            if (expireUtc != null)
            {
                var newClaim = new Claim(claimType, expireUtc.Value.UtcTicks.ToString());
                context.Identity.AddClaim(newClaim);
            }

            return Task.FromResult(0);
        }
    }
}