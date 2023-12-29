using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication7
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.RouteExistingFiles = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
    name: "DeletePost",
    url: "Home/DeletePost/{postId}",
    defaults: new { controller = "Home", action = "DeletePost" }
);

            routes.MapRoute(
                name: "UserProfile",
                url: "Home/UserProfile/{userId}",
                defaults: new { controller = "Home", action = "UserProfile" }
            );


            //routes.MapRoute(
            //    name: "UserProfile",
            //    url: "Profile/{userId}",
            //    defaults: new { controller = "Home", action = "Profile" }
            //);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
