using System;
using System.Diagnostics;
using System.Web;

namespace Appceptive.Agent.Web
{
    public class AgentHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;
        }

        protected void BeginRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var request = context.Request;
            var url = request.Url.ToString();
            var name = url;
	        var stopwatch = Stopwatch.StartNew();

            context.Items["Appceptive.Stopwatch"] = stopwatch;

            Core.Appceptive.BeginActivity(name);
            Core.Appceptive.AddActivityProperty("Url", url);
            Core.Appceptive.AddActivityProperty("UserAgent", request.UserAgent);
        }

        protected void EndRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
	        var stopwatch = (Stopwatch) context.Items["Appceptive.Stopwatch"];
            stopwatch.Stop();

			Core.Appceptive.EndCurrentActivity(stopwatch.ElapsedMilliseconds);
        }
        
        public void Dispose()
        {
        }
    }
}