﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Probet247
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{sportid}/{leagueid}/{eventcode}/{pagenlo}",
                defaults: new { controller = "exchange", action = "Login", sportid = UrlParameter.Optional, leagueid = UrlParameter.Optional, eventcode = UrlParameter.Optional, pagenlo = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default1",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "exchange", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}
