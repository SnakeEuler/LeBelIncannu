using UnityEngine;
using UnityEngine.Events;

namespace CleanEvents
{
    public class EventListenerComponent : MonoBehaviour
    {
        public EventName EventName;

        public UnityEvent<EventModel> OnEventRaised;

        private void Awake()
        {
            EventsManager.AddListener(EventName, this, Listen);
        }

        public void Listen(EventModel model)
        {
            OnEventRaised.Invoke(model);
        }

        private void OnDestroy()
        {
            EventsManager.RemoveListener(EventName, this, Listen);
        }
    }
}
