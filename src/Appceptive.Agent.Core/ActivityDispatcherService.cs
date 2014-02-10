using System;
using System.Threading.Tasks;
using System.Timers;

namespace Appceptive.Agent.Core
{
    public class ActivityDispatcherService
    {
        private readonly string _applicationName;
        private readonly ActivityQueue _queue;
	    private readonly ApiClient _apiClient;
        private readonly Timer _timer;

        internal TimeSpan ActivityDispatchInterval { get; set; }
        internal int ActivityBatchSize { get; set; }
        internal int ActivityDispatchAttempts { get; set; }

	    public ActivityDispatcherService(string applicationName, ActivityQueue queue, ApiClient apiClient)
        {
	        _applicationName = applicationName;
	        _queue = queue;
		    _apiClient = apiClient;

	        _timer = new Timer {AutoReset = false};
            _timer.Elapsed += DispatchQueuedActivities;
        }

        private void DispatchQueuedActivities(object sender, ElapsedEventArgs e)
        {
	        Task.Factory.StartNew(async () =>
		    {
			    var activities = _queue.GetQueuedActivities(ActivityBatchSize);

                Appceptive.Logger.Information("Attempting to upload {0} activities to Appceptive.", activities.Count);

			    foreach (var activity in activities)
			    {
			        try
			        {
			            await _apiClient.CreateActivity(_applicationName, activity);
			        }
			        catch(Exception ex)
			        {
			            activity.DispatchFailed();

			            if (activity.DispatchAttempts < ActivityDispatchAttempts)
			            {
			                _queue.QueueActivity(activity);
			            }

                        Appceptive.Logger.Error(ex, "Failed to upload activity to Appceptive for application {0}.", _applicationName);
			        }
			    }

                _timer.Start();
		    });
        }

        public void Start()
        {
            Appceptive.Logger.Information("Activity dispatcher starting.");

            _timer.Interval = ActivityDispatchInterval.TotalMilliseconds;
			_timer.Start();
        }

        public void Stop()
        {
            Appceptive.Logger.Information("Activity dispatcher shutting down.");
            
            _timer.Dispose();
        }
    }
}