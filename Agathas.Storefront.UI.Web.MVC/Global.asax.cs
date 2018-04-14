using System.Web.Mvc;
using System.Web.Routing;
using Agathas.Storefront.Controllers;
using Agathas.Storefront.Infrastructure.Configuration;
using Agathas.Storefront.Infrastructure.Email;
using Agathas.Storefront.Infrastructure.Logging;
using StructureMap;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System;
using Ninject;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;

namespace Agathas.Storefront.UI.Web.MVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });


            routes.MapHttpRoute(name: "DefaultApi",
routeTemplate: "api/{controller}/{action}/{id}",
defaults: new { id = RouteParameter.Optional });
            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);

            BootStrapper.ConfigureDependencies();
            //DependencyResolver.SetResolver(new NinjectDependencyResolver());

            Controllers.AutoMapperBootStrapper.ConfigureAutoMapper();
            Services.AutoMapperBootStrapper.ConfigureAutoMapper();


            ApplicationSettingsFactory.InitializeApplicationSettingsFactory
                                  (ObjectFactory.GetInstance<IApplicationSettings>());

            LoggingFactory.InitializeLogFactory(ObjectFactory.GetInstance<ILogger>());

            EmailServiceFactory.InitializeEmailServiceFactory
                                  (ObjectFactory.GetInstance<IEmailService>());
            ControllerBuilder.Current.SetControllerFactory(new IoCControllerFactory());


            LoggingFactory.GetLogger().Log("Application Started");
            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());


        }
    }
    public class AllowCrossSiteJsonAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null)
            {
               // var origin = actionExecutedContext.Request.Headers["Origin"];
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS, PUT");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");                
                actionExecutedContext.Response.Headers.Add("SupportsCredentials", "true");
               // actionExecutedContext.Response.Headers.Add("Content-Type", "Set-Cookie");
                //actionExecutedContext.Response.Headers.
            }

            base.OnActionExecuted(actionExecutedContext);
        }
    }


}