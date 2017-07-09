using BestTickets.Infrastructure;
using System.Data.Entity;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;

namespace BestTickets
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);//Must be declared higher than RouteConfig

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<RouteRequestContext>(new CreateDatabaseIfNotExists<RouteRequestContext>());
        }
    }
}
