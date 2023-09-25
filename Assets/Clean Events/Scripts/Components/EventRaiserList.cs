using System.Collections.Generic;

namespace CleanEvents
{
    public class EventRaiserList : EventRaiser
    {
        public List<EventName> EventNames;

        public override void Raise()
        {
            foreach (var evt in EventNames)
            {
                EventsManager.RaiseEvent(evt, this, Delay);
            }
        }
    }
}
