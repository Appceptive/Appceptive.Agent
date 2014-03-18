using System.Web;
using System.Web.Mvc;

namespace Appceptive.Agent.Samples.MVC
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}