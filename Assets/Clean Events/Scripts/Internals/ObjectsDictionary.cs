using System.Collections.Generic;
using System.Reflection;

namespace CleanEvents
{
    internal class ObjectsDictionary : Dictionary<object, EventMethodDictionary>
    {
        public bool IsRegistered(EventName eventName, object targetObject, MethodInfo method)
        {
            if (ContainsKey(targetObject) == false)
                return false;

            if (this[targetObject].ContainsKey(eventName) == false)
                return false;

            return this[targetObject].Contains(eventName, method);
        }

        public void Add(EventName eventName, object targetObject, MethodInfo method)
        {
            if (ContainsKey(targetObject) == false)
            {
                Add(targetObject, new EventMethodDictionary(eventName));
            }

            this[targetObject].Add(eventName, method);
        }

        public bool Contains(EventName eventName, object obj)
        {
            return ContainsKey(obj) && this[obj].ContainsKey(eventName);
        }

        public List<MethodInfo> GetMethods(EventName eventName, object obj)
        {
            return this[obj][eventName];
        }
    }
}
