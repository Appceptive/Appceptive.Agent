using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Appceptive.Agent.Core.Json;
using Newtonsoft.Json;

namespace Appceptive.Agent.Core
{
    public class Activity
	{
		private readonly IDictionary<string, object> _properties = new Dictionary<string, object>(); 
        private readonly IList<Event> _events = new List<Event>(); 
 
	    public string Name { get; private set; }
		public string Server { get; private set; }
		public long Duration { get; private set; }
        public bool Important { get; private set; }
        public DateTime Timestamp { get; private set; }
        public int DispatchAttempts { get; private set; }

        [JsonConverter(typeof(DictionaryKeysAreNotPropertyNamesJsonConverter))]
        public IDictionary<string, object> Properties
        {
            get { return new ReadOnlyDictionary<string, object>(_properties); }
        } 

        public IEnumerable<Event> Events
        {
            get { return _events.ToList().AsReadOnly(); }
        } 

		public Activity(string name)
		{
		    Timestamp = DateTime.UtcNow;
		    Name = name;
			Server = Environment.MachineName;
		}

        public void FlagImportant()
        {
            Important = true;
        }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void AddProperty(string name, object value)
        {
            _properties[name] = value;
        }

        public void AddEvent(Event @event)
        {
            if (@event.Important)
            {
                FlagImportant();
            }

            _events.Add(@event);
        }

        public void DispatchFailed()
        {
            DispatchAttempts++;
        }

	    public void End(long duration)
	    {
		    Duration = duration;
	    }
	}
}
