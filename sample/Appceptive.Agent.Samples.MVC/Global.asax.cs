using System;
using System.Web.Mvc;
using System.Web.Routing;
using Appceptive.Agent.Serilog;
using Serilog;

namespace Appceptive.Agent.Samples.MVC
{
	public class MvcApplication : System.Web.HttpApplication
	{
        protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			Core.Appceptive.Start(c =>
			{
				c.ActivityDispatchInterval = TimeSpan.FromSeconds(5);

                c.Filter(activity =>
                {
                    var url = (string) activity.Properties["Url"];

                    return url.Contains("/Images/");
                });
			});

            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .WriteTo.Appceptive()
                .CreateLogger();
		}
	}
}