namespace CleanEvents
{
    public class EventModel
    {
        public EventName EventName;
        public object Sender;
        public float Delay = 0;
        public bool IsHandled = false;
        public object Payload;

        public EventModel(EventName eventName, object sender = null, float delay = 0, object payload = null)
        {
            if (payload == null)
                payload = new object();

            EventName = eventName;
            Sender = sender;
            Delay = delay;
            Payload = payload;
            IsHandled = false;
        }

        public override string ToString()
        {
            return EventName.ToString();
        }
    }
}
