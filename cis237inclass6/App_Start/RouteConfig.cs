using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace cis237inclass6
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}", // { } are placeholders in the url
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional } // UrlParameter.optional = means an ID doesn't have to be provided
            );

            //routes.MapRoute(
            //    name: "AnotherOne",
            //    url: "something/foo/bar",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }

    }
}
