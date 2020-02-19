using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeOrdinance
{
    public class Game
    {
        private const string EventAddPlayer = "add-player";
        private const string EventBuildUnit = "build-unit";
        private const string EventDestroyUnit = "destroy-unit";
        
        private Events _events;

        public Game()
        {
            _events = new Events();
        }

        public PresentableLobby ShowLobby()
        {
            return new PresentableLobby
            {
                Players = _events.All
                    .Where(e => e.Type == EventAddPlayer)
                    .Select(e => (string) e.Data)
                    .ToArray()
            };
        }

        public void AddPlayer(string playerName)
        {
            _events.EmitGameEvent(EventAddPlayer, playerName);
        }

        public string BuildUnit(string unitType)
        {
            var id = _events.EmitGameEvent(EventBuildUnit, unitType);
            return id.ToString();
        }

        public void DestroyUnit(string id)
        {
            _events.EmitGameEvent(EventDestroyUnit, Guid.Parse(id));
        }

        public PresentableUnits ViewUnits()
        {
            var units = new List<string>();
            var destroyed = new List<Guid>();

            foreach (var @event in _events.AllInReverse)
            {
                if (@event.Type == EventDestroyUnit)
                {
                    destroyed.Add((Guid) @event.Data);
                    continue;
                }

                if (destroyed.Contains(@event.Id))
                    continue;

                if (@event.Type == EventBuildUnit) units.Add((string) @event.Data);
            }

            return new PresentableUnits()
            {
                Units = units.AsReadOnly().Reverse().ToArray()
            };
        }

        public class PresentableLobby
        {
            public string[] Players;
        }

        public class PresentableUnits
        {
            public string[] Units;
        }
    }
}