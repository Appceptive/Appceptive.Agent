using System;
using System.Threading;
using System.Web.Mvc;
using Serilog;

namespace Appceptive.Agent.Samples.MVC.Controllers
{
    public class SerilogController : Controller
    {

        public ActionResult LogError()
        {
            var ex = new Exception("Foo");
            Log.Error(ex, "LogError logged");

            return Content("LogError Logged");
        }

        public ActionResult LogInfo()
        {
            var person = new { Name = "Bob", Age = 22 };

            Log.Information("Information logged at {Prop} for {@Person}", DateTime.UtcNow, person);
            Core.Appceptive.AddActivityProperty("Person", person);

            return Content("Info logged");
        }

        public ActionResult LogTwentyErrors()
        {
            var ex = new Exception("Twenty Errors");

            for (var i = 1; i <= 20; i++)
            {
                Thread.Sleep(100);
                Log.Error(ex, string.Format("Error {0}", i));
            }

            return Content("Logged twenty errors");
        }
    }
}