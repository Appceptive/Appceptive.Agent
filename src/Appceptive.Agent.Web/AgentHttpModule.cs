using System;
using System.Web;
using Appceptive.Agent.Core;

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

            Core.Appceptive.BeginActivityScope(name);
            Core.Appceptive.AddActivityProperty("Url", url);
            Core.Appceptive.AddActivityProperty("UserAgent", request.UserAgent);
        }

        protected void EndRequest(object sender, EventArgs e)
        {
            var scope = ActivityScope.Current;
            if (scope == null)
                return;

            scope.Dispose();
        }
        
        public void Dispose()
        {
        }
    }
}