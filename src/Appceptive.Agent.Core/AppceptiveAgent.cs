namespace Appceptive.Agent.Core
{
    public class AppceptiveAgent
    {
        private readonly ActivityDispatcherService _activityDispatcherService;
        private readonly ActivityQueue _activityQueue;

        public AppceptiveAgent(ActivityDispatcherService activityDispatcherService, ActivityQueue activityQueue)
        {
            _activityDispatcherService = activityDispatcherService;
            _activityQueue = activityQueue;
        }

        public void Start()
        {
            Appceptive.Logger.Information("Appceptive agent starting.");

            _activityDispatcherService.Start();
        }

        public void Shutdown()
        {
            Appceptive.Logger.Information("Appceptive agent shutting down.");

            _activityDispatcherService.Stop();
        }

        public void QueueActivity(Activity activity)
        {
            _activityQueue.QueueActivity(activity);
        }
    }
}