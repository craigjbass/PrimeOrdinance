using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeOrdinance
{
    public class Events
    {
        private readonly List<Event> _events = new List<Event>();
        public class Event
        {
            public Guid Id;
            public string Type;
            public object Data;
        }

        public IEnumerable<Event> All => _events.AsReadOnly().AsEnumerable();

        public IEnumerable<Event> AllInReverse => _events.AsReadOnly().Reverse();

        public Guid EmitGameEvent(string eventType, object data)
        {
            var id = Guid.NewGuid();
            var @event = new Event
            {
                Id = id,
                Type = eventType,
                Data = data
            };
            _events.Add(@event);
            return id;
        }
    }
}