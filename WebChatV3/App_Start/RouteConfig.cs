using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebChatV3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "@me",
                url: "channels/@me",
                defaults: new { controller = "Chating", action = "Index"}
            );
            routes.MapRoute(
                name: "private",
                url: "channels/@me/{room}",
                defaults: new { controller = "Chating", action = "Index"},
                new { room = @"^[0-9a-f]{8}[-][0-9a-f]{4}[-][0-9a-f]{4}[-][0-9a-f]{4}[-][0-9a-f]{12}$" }
            );
            routes.MapRoute(
                name: "server",
                url: "channels/{server}",
                defaults: new { controller = "Chating", action = "Index"},
                new { server = @"^[0-9a-f]{8}[-][0-9a-f]{4}[-][0-9a-f]{4}[-][0-9a-f]{4}[-][0-9a-f]{12}$" }
            );
            routes.MapRoute(
                name: "channel",
                url: "channels/{server}/{room}",
                defaults: new { controller = "Chating", action = "Index"},
                new { server = @"^[0-9a-f]{8}[-][0-9a-f]{4}[-][0-9a-f]{4}[-][0-9a-f]{4}[-][0-9a-f]{12}$", room = @"^[0-9a-f]{8}[-][0-9a-f]{4}[-][0-9a-f]{4}[-][0-9a-f]{4}[-][0-9a-f]{12}$" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "UserRegister", action = "Login"}
            );
        }
    }
}
