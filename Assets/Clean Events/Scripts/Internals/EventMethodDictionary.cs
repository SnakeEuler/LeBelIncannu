using System.Collections.Generic;
using System.Reflection;

namespace CleanEvents
{
    internal class EventMethodDictionary : Dictionary<EventName, List<MethodInfo>>
    {
        public EventMethodDictionary(EventName eventName)
        {
            Add(eventName, new List<MethodInfo>());
        }

        public void Add(EventName eventName, MethodInfo method)
        {
            if (ContainsKey(eventName) == false)
                Add(eventName, new List<MethodInfo>());

            this[eventName].Add(method);
        }

        public bool Contains(EventName eventName, MethodInfo method)
        {
            return this.ContainsKey(eventName) && this[eventName].Contains(method);
        }
    }
}
