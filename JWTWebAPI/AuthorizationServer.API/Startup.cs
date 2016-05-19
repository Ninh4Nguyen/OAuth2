using AuthorizationServer.API.Formats;
using AuthorizationServer.API.Providers;
using AuthorizationServer.API.Repository;
using AuthorizationServer.API.Services;
using Autofac;
using Autofac.Configuration;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Http;

[assembly: OwinStartup(typeof(AuthorizationServer.API.Startup))]
namespace AuthorizationServer.API
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
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<UserServices>().As<IUserServices>().InstancePerRequest();
            builder.RegisterType<AudienceServices>().As<IAudienceServices>().InstancePerRequest();
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            ConfigurationOAuth(app);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
        public void ConfigurationOAuth(IAppBuilder app)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            UserServices userServices = new UserServices(unitOfWork);
            AudienceServices audienceServices = new AudienceServices(unitOfWork);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(5),
                Provider = new CustomOAuthProvider(unitOfWork, audienceServices, userServices),
                AccessTokenFormat = new CustomJwtFormat(audienceServices, "http://localhost:45495")
            };
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }
    }
}