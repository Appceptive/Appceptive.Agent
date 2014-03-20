using System;
using System.Threading.Tasks;
using System.Timers;
using Appceptive.Agent.Core.Logging;

namespace Appceptive.Agent.Core
{
    public class ActivityDispatcherService
    {
        private readonly ActivityQueue _queue;
	    private readonly ApiClient _apiClient;
        private readonly Timer _timer;

	    public ActivityDispatcherService(ActivityQueue queue, ApiClient apiClient)
        {
	        _queue = queue;
		    _apiClient = apiClient;

	        _timer = new Timer {AutoReset = false};
            _timer.Elapsed += DispatchQueuedActivities;
        }

        private void DispatchQueuedActivities(object sender, ElapsedEventArgs e)
        {
	        Task.Factory.StartNew(async () =>
		    {
                var activities = _queue.GetQueuedActivities(Appceptive.Configuration.ActivityBatchSize);

                Logger.Information("Attempting to upload {0} activities to Appceptive.", activities.Count);

			    foreach (var activity in activities)
			    {
			        try
			        {
			            await _apiClient.CreateActivity(Appceptive.Configuration.ApplicationName, activity);
			        }
			        catch(Exception ex)
			        {
			            activity.DispatchFailed();

                        if (activity.DispatchAttempts < Appceptive.Configuration.ActivityDispatchAttempts)
			            {
			                _queue.QueueActivity(activity);
			            }

                        Logger.Error(ex, "Failed to upload activity to Appceptive for application {0}.", Appceptive.Configuration.ApplicationName);
			        }
			    }

                _timer.Start();
		    });
        }

        public void Start()
        {
            Logger.Information("Activity dispatcher starting.");

            _timer.Interval = Appceptive.Configuration.ActivityDispatchInterval.TotalMilliseconds;
			_timer.Start();
        }

        public void Stop()
        {
            Logger.Information("Activity dispatcher shutting down.");
            
            _timer.Dispose();
        }
    }
}