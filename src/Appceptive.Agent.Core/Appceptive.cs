using System;
using Appceptive.Agent.Core.Logging;

namespace Appceptive.Agent.Core
{
    public class Appceptive
    {
        private static AppceptiveAgent _instance;
	    private static IActivityStorage _activityStorage = new DefaultActivityStorage();
        
        private static Activity CurrentActivity
        {
            get { return _activityStorage.CurrentActivity; }
            set { _activityStorage.CurrentActivity = value; }
        }

	    public static ILogger Logger { get; private set; }

	    public static void SetActivityStorage(IActivityStorage activityStorage)
        {
            _activityStorage = activityStorage;
        }

        public static void Start(Action<Configuration> configure)
        {
			if(_instance != null) 
				throw new InvalidOperationException("Appceptive agent has already been started.");

			var configuration = new Configuration();
			configure(configuration);

			var apiClient = new ApiClient(configuration.ApiUrl, configuration.ApiKey);
			var activityQueue = new ActivityQueue(configuration.Filters);
            var activityDispatcherService = new ActivityDispatcherService(configuration.ApplicationName, activityQueue, apiClient)
            {
                ActivityBatchSize = configuration.ActivityBatchSize,
                ActivityDispatchAttempts = configuration.ActivityDispatchAttempts,
                ActivityDispatchInterval = configuration.ActivityDispatchInterval
            };

            Logger = configuration.Logger;

			_instance = new AppceptiveAgent(activityDispatcherService, activityQueue);
            _instance.Start();
        }

        public static void Shutdown()
        {
            EnsureStarted();

            _instance.Shutdown();
        }

        public static void BeginActivity(string name)
        {
            EnsureStarted();

            if (CurrentActivity != null)
                throw new InvalidOperationException("Unable to create a new activity while one is already running.");

            Logger.Information("Beginning Activity: {0}", name);
            CurrentActivity = new Activity(name);
        }

        public static void EndCurrentActivity(long duration)
        {
            EnsureStarted();

            if (CurrentActivity == null)
                return;

			Logger.Information("Ending Activity: {0}, took {1}.", CurrentActivity.Name, duration);
	        CurrentActivity.End(duration);

            _instance.QueueActivity(CurrentActivity);
            CurrentActivity = null;
        }

        public static void AddActivityProperty(string name, object value)
        {
            EnsureStarted();

            if (CurrentActivity == null)
                return;

            CurrentActivity.AddProperty(name, value);
        }

        public static void AddActivityEvent(Event @event)
        {
            EnsureStarted();

            if (CurrentActivity == null)
                return;

            CurrentActivity.AddEvent(@event);
        }

        public static void SetActivityName(string name)
        {
            EnsureStarted();

            if (CurrentActivity == null)
                return;

            CurrentActivity.ChangeName(name);
        }

        public static void FlagActivityImportant()
        {
            EnsureStarted();

            if (CurrentActivity == null)
                return;

            CurrentActivity.FlagImportant();
        }

        private static void EnsureStarted()
        {
            if (_instance == null)
				throw new InvalidOperationException("Agent has not been started, ensure you have started the agent first by calling AppceptiveAgent.Start().");
        }
    }
}