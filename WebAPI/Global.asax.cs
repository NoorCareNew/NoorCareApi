using NoorCare.Web.App_Start;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NoorCare.WebAPI
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            Bootstrapper.Run();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //var config = GlobalConfiguration.Configuration;

            //AreaRegistration.RegisterAllAreas();
            //WebApiConfig.Register(config);
            //Bootstrapper.Run();
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //GlobalConfiguration.Configuration.EnsureInitialized();
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
