using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Appceptive.Agent.Core.Json;
using Newtonsoft.Json;

namespace Appceptive.Agent.Core
{
    public class Event
    {
        private readonly IDictionary<string, object> _properties = new Dictionary<string, object>(); 

        public string Type { get; private set; }
        public string Description { get; private set; }
        public bool Important { get; private set; }
        public DateTime Timestamp { get; private set; }

        public Event(string type, bool important)
        {
            Type = type;
            Important = important;
            Timestamp = DateTime.UtcNow;
        }

        public Event(string type, bool important, DateTime timestamp) 
            : this(type, important)
        {
            Timestamp = timestamp;
        }

        [JsonConverter(typeof(DictionaryKeysAreNotPropertyNamesJsonConverter))]
        public IReadOnlyDictionary<string, object> Properties
        {
            get { return new ReadOnlyDictionary<string, object>(_properties); }
        }

        public Event WithProperty(string name, object value)
        {
            _properties[name] = value;

            return this;
        }

        public Event WithDescription(string description)
        {
            Description = description;

            return this;
        }
    }
}