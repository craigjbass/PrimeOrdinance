using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeOrdinance
{
    public class Game
    {
        private readonly List<string> _players = new List<string>();
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

        public PresentableLobby ShowLobby() =>
            new PresentableLobby
            {
                Players = _players.ToArray()
            };

        public void AddPlayer(string playerName) => _players.Add(playerName);

        public string BuildUnit(string unitType)
        {
            var id = Guid.NewGuid();
            
            var @event = new Event
            {
                Id = id,
                Type = "build-unit",
                Data = unitType
            };
            
            _events.Add(@event);

            return id.ToString();
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
                {
                    continue;
                }
                
                units.Add((string) @event.Data);
            }
            
            return new PresentableUnits()
            {
                Units = units.AsReadOnly().Reverse().ToArray()
            };
        }

        public void DestroyUnit(string unitType)
        {
            _events.Add(new Event()
            {
                Id = Guid.NewGuid(),
                Type = "destroy-unit",
                Data = Guid.Parse(unitType)
            });
        }
    }

}