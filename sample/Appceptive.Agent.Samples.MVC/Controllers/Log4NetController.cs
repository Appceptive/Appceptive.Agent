using System.Web.Mvc;
using log4net;
using log4net.Config;

namespace Appceptive.Agent.Samples.MVC.Controllers
{
    public class Log4NetController : Controller
    {
        public ActionResult LogError()
        {
            XmlConfigurator.Configure();
            var log = LogManager.GetLogger(typeof(Log4NetController));
            
            log.Error("This is a error from log4net.");
            return Content("Log4Net Error");
        }
    }
}