using System;
using System.Collections.Generic;
using PrimeOrdinance.Boundary;
using PrimeOrdinance.Event;

namespace PrimeOrdinance.UseCase
{
    public class ViewEconomy
    {
        private readonly Events _events;
        private readonly TimeProvider _timeProvider;

        public ViewEconomy(Events events, TimeProvider timeProvider)
        {
            _events = events;
            _timeProvider = timeProvider;
        }

        public PresentableEconomy Execute(string playerId)
        {
            var matterRate = 0;
            long matter = 0;
            
            var destroyed = new Dictionary<Guid, long>();
            
            foreach (var @event in _events.AllInReverse)
            {
                if (@event.Is(Game.EventDestroyUnit))
                {
                    destroyed.Add((Guid)@event.Data, @event.Time);
                    matterRate -= 10;
                }

                if (!@event.Is(Game.EventBuildUnit) || !(@event.Data is BuildUnit unitEvent)) continue;
                if (unitEvent.Type != "matter-originator") continue;
                
                matterRate += 10;
                var endTime = destroyed.ContainsKey(@event.Id) ? destroyed[@event.Id] : _timeProvider.GetTime();
                var startTime = @event.Time;
                        
                matter = (endTime - startTime) / 100;
            }
            
            return new PresentableEconomy { MatterRate = matterRate, Matter = matter };
        }
    }
}