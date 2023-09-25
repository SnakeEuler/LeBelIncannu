using UnityEngine;

namespace CleanEvents
{
    public abstract class EventBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            EventsManager.Register(this);
        }

        protected virtual void OnDestroy()
        {
            EventsManager.RemoveListener(this);
        }

        protected virtual void RaiseEvent(EventName eventName)
        {
            EventsManager.RaiseEvent(eventName);
        }
    }
}