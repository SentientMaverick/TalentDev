using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TalentAcquisition
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
             name: "Search",
             url: "Home/Job/search",
             defaults: new { controller = "Job", action = "search" }
         );
            routes.MapRoute(
               name: "JobLink",
               url: "Job/{id}/{name}",
               defaults: new { controller = "Job", action = "Details", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "SearchForm",
               url: "Applicant/Job/search",
               defaults: new { controller = "Job", action = "search"}
           );
          
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
