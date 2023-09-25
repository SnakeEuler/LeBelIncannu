using UnityEngine;

namespace CleanEvents
{
    public class SamplePocoCode
    {
        [ListenTo(EventName.LevelStart)]
        [ListenTo(EventName.LevelEnd)]
        public void OnLevelStarted()
        {
            Debug.Log("On Level Start/Finish called on POCO");
        }
    }
}