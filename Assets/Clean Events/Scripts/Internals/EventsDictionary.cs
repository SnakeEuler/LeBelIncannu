using System;
using System.Collections.Generic;
using System.Reflection;

namespace CleanEvents
{
    internal class EventsDictionary : Dictionary<EventName, SortedEventsList<ListenerModel>>
    {
        public EventsDictionary()
        {
            foreach (var item in Enum.GetValues(typeof(EventName)))
                Add((EventName)item, new SortedEventsList<ListenerModel>());
        }

        public void Add(EventName eventName, object targetObject, MethodInfo method, int priority)
        {
            this[eventName].Add(priority, new ListenerModel()
            {
                TargetObject = targetObject,
                Method = method
            });
        }

        public IEnumerable<ListenerModel> GetAllValues(EventName eventName)
        {
            return this[eventName].GetAllValues();
        }

        public void Remove(EventName eventName, object obj)
        {
            this[eventName].Remove((o) =>
            {
                return ReferenceEquals(obj, o.TargetObject);
            });
        }

        public void Remove(EventName eventName, object obj, MethodInfo listener)
        {
            this[eventName].Remove((o) =>
            {
                return ReferenceEquals(obj, o.TargetObject) && o.Method == listener;
            });
        }
    }
}
