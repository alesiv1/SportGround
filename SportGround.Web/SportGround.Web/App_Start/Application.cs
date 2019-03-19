using Sitecore.DependencyInjection.ContainerContexts;
using Sitecore.DependencyInjection.DependencyResolvers;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportGround.Web
{
    public class Application : Sitecore.Web.Application
    {
        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapRoute(
            //    name: "route example",
            //    url: "api/{controller}/{action}/{id}",
            //    defaults: new { controller = "RouteExample", action = "Index", id = UrlParameter.Optional });
        }
    }
}