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
        private TimeProvider _timeProvider;

        public interface TimeProvider
        {
            long GetTime();
        }
        
        public Game(TimeProvider timeProvider)
        {
            _events = new Events(timeProvider);
            _timeProvider = timeProvider;
        }

        class BuildUnitEvent
        {
            public string Type;
            public Guid? OwnerId;
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

        public string AddPlayer(string playerName)
        {
            return _events.EmitGameEvent(EventAddPlayer, playerName).ToString();
        }

        public string BuildUnit(string unitType)
        {
            var id = _events.EmitGameEvent(EventBuildUnit, new BuildUnitEvent()
            {
                Type = unitType
            });
            return id.ToString();
        }

        public string BuildUnit(string unitType, string ownerId)
        {
            var id = _events.EmitGameEvent(EventBuildUnit, new BuildUnitEvent()
            {
                Type = unitType,
                OwnerId = ownerId.ToGuid()
            });
            return id.ToString();
        }

        public void DestroyUnit(string id)
        {
            _events.EmitGameEvent(EventDestroyUnit, id.ToGuid());
        }

        public PresentableUnits ViewUnits()
        {
            var units = new Stack<PresentableUnits.Unit>();
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

                if (@event.Is(EventBuildUnit) && @event.Data is BuildUnitEvent unitEvent)
                    units.Push(
                        new PresentableUnits.Unit
                        {
                            Id = @event.Id,
                            Type = unitEvent.Type,
                            OwnedBy = unitEvent.OwnerId,
                            State = "Constructing"
                        }
                    );
            }

            return new PresentableUnits
            {
                Units = units.ToArray()
            };
        }

        public class PresentableLobby
        {
            public string[] Players;
        }

        public class PresentableUnits
        {
            public Unit[] Units;

            public class Unit
            {
                public Guid Id;
                public string Type;
                public Guid? OwnedBy;
                public string State;
            }
        }

        public class PresentableEconomy
        {
            public int MatterRate;
            public long Matter;
        }

        public PresentableEconomy ViewEconomy(string playerId)
        {
            var matterRate = 0;
            long matter = 0;
            foreach (var @event in _events.AllInReverse)
            {
                if (@event.Is(EventBuildUnit) && @event.Data is BuildUnitEvent unitEvent)
                {
                    if (unitEvent.Type == "matter-originator")
                    {
                        matterRate += 10;
                        matter = (_timeProvider.GetTime() - @event.Time) / 100;
                    }
                }
            }
            
            return new PresentableEconomy { MatterRate = matterRate, Matter = matter };
        }
    }
}