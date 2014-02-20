using System;
using System.Collections.Generic;
using System.Configuration;
using Appceptive.Agent.Core.Logging;

namespace Appceptive.Agent.Core
{
    public class Configuration
    {
		public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
		public TimeSpan ActivityDispatchInterval { get; set; }
        public int ActivityBatchSize { get; set; }
		public string ApplicationName { get; set; }
        public int ActivityDispatchAttempts { get; set; }
        public ILogger Logger { get; private set; }
        public IList<Predicate<Activity>> Filters { get; private set; }

	    public Configuration()
	    {
			ApiUrl = ConfigurationManager.AppSettings["Appceptive.ApiUrl"] ?? "http://api.appceptive.com";
			ApiKey = ConfigurationManager.AppSettings["Appceptive.ApiKey"];
		    ApplicationName = ConfigurationManager.AppSettings["Appceptive.ApplicationName"];
		    ActivityDispatchInterval = TimeSpan.FromSeconds(60);
		    ActivityBatchSize = 500;
	        ActivityDispatchAttempts = 5;
	        Logger = new NullLogger();
            Filters = new List<Predicate<Activity>>();
	    }

        public void UseLogger(ILogger logger)
        {
            Logger = logger;
        }

        public void Filter(Predicate<Activity> filter)
        {
            Filters.Add(filter);
        }
    }
}