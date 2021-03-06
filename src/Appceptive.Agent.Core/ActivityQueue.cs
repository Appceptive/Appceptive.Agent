﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Appceptive.Agent.Core
{
    public class ActivityQueue
    {
        private readonly ConcurrentQueue<Activity> _activities;

        public ActivityQueue()
        {
            _activities = new ConcurrentQueue<Activity>();
        }

        public void QueueActivity(Activity activity)
        {
            _activities.Enqueue(activity);
        }

        public IList<Activity> GetQueuedActivities(int count)
        {
            var activities = new List<Activity>();

            for (var i = 0; i < _activities.Count && activities.Count < count; i++)
            {
                Activity activity;
                _activities.TryDequeue(out activity);

                if (activity == null || Appceptive.Configuration.Filters.Any(filter => filter(activity)))
                    continue;

                activities.Add(activity);
            }

            return activities;
        }
    }
}