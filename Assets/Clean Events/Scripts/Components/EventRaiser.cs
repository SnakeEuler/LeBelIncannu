using UnityEngine;

namespace CleanEvents
{
    public abstract class EventRaiser : MonoBehaviour
    {
        public float Delay = 0;
        public bool RaiseOnEnable = false;
        public bool RaiseOnStart = false;

        private void OnEnable()
        {
            if (RaiseOnEnable)
                Raise();
        }

        private void Start()
        {
            if (RaiseOnStart)
                Raise();
        }

        public virtual void Raise()
        {

        }
    }
}
