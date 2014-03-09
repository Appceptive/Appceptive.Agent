using System;
using Appceptive.Agent.Core.Logging;

namespace Appceptive.Agent.Core
{
    public class Appceptive
    {
        private static AppceptiveAgent _instance;
	    
	    public static void Start(Action<Configuration> configure = null)
        {
			if(_instance != null) 
				throw new InvalidOperationException("Appceptive agent has already been started.");

			var configuration = new Configuration();
            
			if(configure != null)
			{
				configure(configuration);
			}

            Logger.Current = configuration.Logger;

			var apiClient = new ApiClient(configuration.ApiUrl, configuration.ApiKey);
			var activityQueue = new ActivityQueue(configuration.Filters);
	        var activityDispatcherService = new ActivityDispatcherService(configuration, activityQueue, apiClient);

            _instance = new AppceptiveAgent(activityDispatcherService, activityQueue);
            _instance.Start();
        }

        public static void Shutdown()
        {
            EnsureStarted();

            _instance.Shutdown();
        }

        public static ActivityScope BeginActivityScope(string name)
        {
            EnsureStarted();

            var scope = ActivityScope.Current;
            if (scope != null)
                throw new InvalidOperationException("Unable to create a new activity scope as there is already an active scope.");

            Logger.Information("Beginning Activity: {0}.", name);

            var activity = new Activity(name);
            return ActivityScope.Create(_instance, activity);
        }

        public static void AddActivityProperty(string name, object value)
        {
            EnsureStarted();

            var scope = ActivityScope.Current;
            if(scope == null)
                return;

            scope.Activity.AddProperty(name, value);
        }

        public static void AddActivityEvent(Event @event)
        {
            EnsureStarted();

            var scope = ActivityScope.Current;
            if (scope == null)
                return;

            scope.Activity.AddEvent(@event);
        }

        public static void SetActivityName(string name)
        {
            EnsureStarted();

            var scope = ActivityScope.Current;
            if (scope == null)
                return;

            scope.Activity.ChangeName(name);
        }

        public static void FlagActivityImportant()
        {
            EnsureStarted();

            var scope = ActivityScope.Current;
            if (scope == null)
                return;

            scope.Activity.FlagImportant();
        }

        private static void EnsureStarted()
        {
            if (_instance == null)
				throw new InvalidOperationException("The Appceptive agent has not been started, ensure you have started the agent first by calling Appceptive.Start().");
        }
    }
}