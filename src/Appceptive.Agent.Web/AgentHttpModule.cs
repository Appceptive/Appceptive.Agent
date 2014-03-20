using System;
using System.Web;
using Appceptive.Agent.Core;

namespace Appceptive.Agent.Web
{
    public class AgentHttpModule : IHttpModule
    {
        private static bool _agentStarted;
        private static readonly object _agentStartedLock = new object();

        public void Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
            context.EndRequest += EndRequest;

            StartAppceptiveAgent();
        }

        private static void StartAppceptiveAgent()
        {
            if (_agentStarted)
                return;

            lock (_agentStartedLock)
            {
                if (_agentStarted)
                    return;

                Core.Appceptive.Start();
                _agentStarted = true;
            }
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