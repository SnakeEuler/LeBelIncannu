namespace CleanEvents
{
    public class EventRaiserSingle : EventRaiser
    {
        public EventName EventName;

        public override void Raise()
        {
            EventsManager.RaiseEvent(EventName, this, Delay);
        }
    }
}
