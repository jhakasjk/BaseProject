using MvcGlobalisationSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BaseProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            //routes.MapRoute(
            //    name: "Joke-URL",
            //    url: "Joke/{id}/{title}",
            //    defaults: new { action = "Joke", controller = "Home" }, namespaces: new string[] { "BaseProject.Controllers" }
            //    );

            //routes.MapRoute(
            //   name: "Default",
            //   url: "{controller}/{action}/{id}",
            //   defaults: new { action = "Index", controller = "Home", id = UrlParameter.Optional }, namespaces: new string[] { "BaseProject.Controllers" }
            //   );
            const string defautlRouteUrl = "{controller}/{action}/{id}";

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteValueDictionary defaultRouteValueDictionary = new RouteValueDictionary(new
            {
                controller = "Home",
                action = "Index",
                id = UrlParameter.Optional
            });
            routes.Add("DefaultGlobalised", new GlobalisedRoute(defautlRouteUrl, defaultRouteValueDictionary));
            routes.Add("Default", new Route(defautlRouteUrl, defaultRouteValueDictionary, new MvcRouteHandler()));
        }
    }
}
