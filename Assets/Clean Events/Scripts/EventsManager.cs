using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace CleanEvents
{
    [DefaultExecutionOrder(-31750)]
    public class EventsManager : MonoBehaviour
    {
        public static EventsManager Instance;

        private static EventsDictionary EventActions = new EventsDictionary();
        private static ObjectsDictionary ObjectEvents = new ObjectsDictionary();

        public bool IsDebug = false;

        public void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            EventActions = new EventsDictionary();

            Instance = this;
            DontDestroyOnLoad(gameObject);

            FindAll();
        }

        private void FindAll()
        {
            MonoBehaviour[] all = FindObjectsOfType<MonoBehaviour>(true);
            foreach (MonoBehaviour item in all)
            {
                Register(item);
            }
        }

        public static void AddListener(EventName eventName, object obj, Action<EventModel> listener)
        {
            if (Instance.IsDebug)
                Debug.Log($"AddListener: {obj} is subscribed to {eventName}");

            RegisterModel(eventName, obj, listener.Method);
        }

        public static void Register(object obj)
        {
            MethodInfo[] methods = obj.GetType().GetMethods();
            foreach (MethodInfo method in methods)
            {
                var attributes = Attribute.GetCustomAttributes(method, typeof(ListenToAttribute));
                foreach (ListenToAttribute attribute in attributes)
                {
                    if (attribute == null)
                        continue;

                    if (ObjectEvents.IsRegistered(attribute.Event, obj, method))
                    {
                        if (Instance.IsDebug)
                            Debug.Log($"{obj}.{method.Name} is already bound to {attribute.Event}");

                        continue;
                    }

                    RegisterModel(attribute.Event, obj, method, attribute.Priority);

                    if (Instance.IsDebug)
                        Debug.Log($"{obj}.{method.Name} is bound to {attribute.Event}");
                }
            }
        }

        private static void RegisterModel(EventName eventName, object targetObject, MethodInfo method, int priority = 0)
        {
            EventActions.Add(eventName, targetObject, method, priority);

            ObjectEvents.Add(eventName, targetObject, method);
        }

        public static void RaiseEvent(EventName eventName, object sender = null, float delay = 0, object payload = null)
        {
            RaiseEvent(new EventModel(eventName, sender, delay, payload));
        }

        public static void RaiseEvent(EventModel eventData)
        {
            if (eventData.Delay > 0)
                Instance.StartCoroutine(RaiseEventDelayed(eventData));
            else
                RaiseImmidiately(eventData);
        }

        private static void RaiseImmidiately(EventModel eventData)
        {
            if (Instance.IsDebug)
                Debug.Log($"Event Raised: {eventData.EventName}");

            foreach (var item in EventActions.GetAllValues(eventData.EventName))
            {
                if (item.TargetObject == null)
                    // What about remove???
                    continue;

                item.Call(eventData);

                if (eventData.IsHandled)
                    break;
            }
        }

        private static IEnumerator RaiseEventDelayed(EventModel eventData)
        {
            yield return new WaitForSeconds(eventData.Delay);

            RaiseImmidiately(eventData);
        }

        public static void RemoveListener(object obj)
        {
            if (ObjectEvents.ContainsKey(obj) == false)
                return;

            foreach (var item in ObjectEvents[obj])
            {
                EventActions.Remove(item.Key, obj);
            }
        }

        public static void RemoveListener(EventName eventName, object obj, Action<EventModel> listener)
        {
            RemoveListener(eventName, obj, listener.Method);
        }

        public static void RemoveListener(EventName eventName, object obj, Action listener)
        {
            RemoveListener(eventName, obj, listener.Method);
        }

        private static void RemoveListener(EventName eventName, object obj, MethodInfo listener)
        {
            if (ObjectEvents.Contains(eventName, obj) == false)
                return;

            var listeners = ObjectEvents.GetMethods(eventName, obj);

            for (int j = 0; j < listeners.Count; j++)
            {
                if (listeners[j] == listener)
                {
                    listeners.RemoveAt(j);
                    break;
                }
            }

            EventActions.Remove(eventName, obj, listener);
        }

        private void OnDestroy()
        {
            EventActions.Clear();
            ObjectEvents.Clear();
        }
    }
}