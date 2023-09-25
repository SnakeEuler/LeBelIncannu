using System;

namespace CleanEvents
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ListenToAttribute : Attribute
    {
        public EventName Event { get; private set; }
        public int Priority { get; private set; }

        public ListenToAttribute(EventName evt)
        {
            Event = evt;
            Priority = 0;
        }

        public ListenToAttribute(EventName evt, int priority)
        {
            Event = evt;
            Priority = priority;
        }
    }
}