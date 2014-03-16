using System;
using System.Threading.Tasks;
using System.Timers;
using Appceptive.Agent.Core.Logging;

namespace Appceptive.Agent.Core
{
    public class ActivityDispatcherService
    {
        private readonly string _applicationName;
        private readonly ActivityQueue _queue;
	    private readonly ApiClient _apiClient;
        private readonly Timer _timer;
        private readonly TimeSpan _activityDispatchInterval;
        private readonly int _activityBatchSize;
        private readonly int _activityDispatchAttempts;

	    public ActivityDispatcherService(Configuration configuration, ActivityQueue queue, ApiClient apiClient)
        {
	        _applicationName = configuration.ApplicationName;
	        _activityDispatchInterval = configuration.ActivityDispatchInterval;
	        _activityBatchSize = configuration.ActivityBatchSize;
	        _activityDispatchAttempts = configuration.ActivityDispatchAttempts;
            _queue = queue;
		    _apiClient = apiClient;

	        _timer = new Timer {AutoReset = false};
            _timer.Elapsed += DispatchQueuedActivities;
        }

        private void DispatchQueuedActivities(object sender, ElapsedEventArgs e)
        {
	        Task.Factory.StartNew(async () =>
		    {
                var activities = _queue.GetQueuedActivities(_activityBatchSize);

                Logger.Information("Attempting to upload {0} activities to Appceptive.", activities.Count);

			    foreach (var activity in activities)
			    {
			        try
			        {
			            await _apiClient.CreateActivity(_applicationName, activity);
			        }
			        catch(Exception ex)
			        {
			            activity.DispatchFailed();

                        if (activity.DispatchAttempts < _activityDispatchAttempts)
			            {
			                _queue.QueueActivity(activity);
			            }

                        Logger.Error(ex, "Failed to upload activity to Appceptive for application {0}.", _applicationName);
			        }
			    }

                _timer.Start();
		    });
        }

        public void Start()
        {
            Logger.Information("Activity dispatcher starting.");

            _timer.Interval = _activityDispatchInterval.TotalMilliseconds;
			_timer.Start();
        }

        public void Stop()
        {
            Logger.Information("Activity dispatcher shutting down.");
            
            _timer.Dispose();
        }
    }
}