using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeOrdinance
{
    public class Events
    {
        private readonly Game.TimeProvider _timeProvider;
        private readonly List<Event> _events = new List<Event>();

        public Events(Game.TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public class Event
        {
            public Guid Id;
            public string Type;
            public object Data;
            public long Time;

            public bool Is(string type) => Type == type;
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
                Data = data,
                Time = _timeProvider.GetTime()
            };
            _events.Add(@event);
            return id;
        }
    }
}