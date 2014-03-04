using System;
using System.Diagnostics;

namespace Appceptive.Agent.Core
{
    public class ActivityScope : IDisposable
    {
        private readonly AppceptiveAgent _agent;
        private readonly Activity _activity;
        private readonly Stopwatch _stopwatch;

        public Activity Activity
        {
            get { return _activity; }
        }

        public static ActivityScope Current
        {
            get { return ActivityScopeStorage.GetCurrentScope(); }
        }

        private ActivityScope(AppceptiveAgent agent, Activity activity)
        {
            _agent = agent;
            _activity = activity;
            _stopwatch = Stopwatch.StartNew();
        }

        public static ActivityScope Create(AppceptiveAgent agent, Activity activity)
        {
            if (Current != null)
                throw new Exception("Unable to begin another transaction scope when one already exists.");

            var scope = new ActivityScope(agent, activity);
            ActivityScopeStorage.SetCurrentScope(scope);

            return scope;
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            _activity.End(_stopwatch.ElapsedMilliseconds);
            _agent.QueueActivity(Activity);

            ActivityScopeStorage.SetCurrentScope(null);
        }
    }
}