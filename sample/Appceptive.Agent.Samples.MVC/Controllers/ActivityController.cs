using System.Threading;
using System.Web.Mvc;

namespace Appceptive.Agent.Samples.MVC.Controllers
{
    public class ActivityController : Controller
	{
		public ActionResult Index()
		{
			return Content("Normal Action");
		}

		public ActionResult Slow()
		{
			Thread.Sleep(2000);

			return Content("Slow Action");
		}
	}
} 