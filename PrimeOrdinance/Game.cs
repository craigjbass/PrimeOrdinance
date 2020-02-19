using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeOrdinance
{
    public class Game
    {
        private readonly List<Event> _events = new List<Event>();

        public class PresentableLobby
        {
            public string[] Players;
        }

        public class PresentableUnits
        {
            public string[] Units;
        }

        private class Event
        {
            public Guid Id;
            public string Type;
            public object Data;
        }

        private Guid EmitGameEvent(string eventType, object data)
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

        public PresentableLobby ShowLobby()
        {
            return new PresentableLobby
            {
                Players = _events
                    .Where(e => e.Type == "add-player")
                    .Select(e => (string) e.Data)
                    .ToArray()
            };
        }

        public void AddPlayer(string playerName)
        {
            EmitGameEvent("add-player", playerName);
        }

        public string BuildUnit(string unitType)
        {
            var id = EmitGameEvent("build-unit", unitType);
            return id.ToString();
        }

        public void DestroyUnit(string id)
        {
            EmitGameEvent("destroy-unit", Guid.Parse(id));
        }

        public PresentableUnits ViewUnits()
        {
            var units = new List<string>();
            var destroyed = new List<Guid>();
            
            foreach (var @event in _events.AsReadOnly().Reverse())
            {
                if (@event.Type == "destroy-unit")
                {
                    destroyed.Add((Guid) @event.Data);
                    continue;
                }

                if (destroyed.Contains(@event.Id))
                    continue;

                if (@event.Type == "build-unit") units.Add((string) @event.Data);
            }
            
            return new PresentableUnits()
            {
                Units = units.AsReadOnly().Reverse().ToArray()
            };
        }
    }

}