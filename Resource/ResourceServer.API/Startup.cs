using Autofac;
using Autofac.Integration.WebApi;
using BusinessServices.Repositories;
using BusinessServices.Services;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Reflection;
using System.Web.Http;

[assembly: OwinStartup(typeof(ResourceServer.API.Startup))]
namespace ResourceServer.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            //Autofac
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ProductServices>().As<IProductServices>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            ConfigurationOAuth(app);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        public void ConfigurationOAuth(IAppBuilder app)
        {
            var issuer = "http://localhost:45495";
            var audience = "099153c2625149bc8ecb3e85e03f0022";
            var secret = TextEncodings.Base64Url.Decode("IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw");

            app.UseJwtBearerAuthentication(
                new Microsoft.Owin.Security.Jwt.JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                    AllowedAudiences = new[] { audience},
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[] 
                    { 
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, secret)
                    }
                });
        }
    }
}