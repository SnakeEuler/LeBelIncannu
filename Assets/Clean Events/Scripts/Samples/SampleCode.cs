using UnityEngine;

namespace CleanEvents.Samples
{
    public class SampleCode : EventBehaviour
    {
        SamplePocoCode pocoObject;

        private void Start()
        {
            pocoObject = new SamplePocoCode();
            EventsManager.Register(pocoObject);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventsManager.RemoveListener(pocoObject);
        }


        [ContextMenu("Make")]
        public void Make()
        {
            var go = new GameObject();
            var component = go.AddComponent<SampleCode>();
            EventsManager.Register(component);
        }


        [ListenTo(EventName.LevelStart)]
        [ListenTo(EventName.LevelEnd)]
        public void OnLevelStartedOrEnded()
        {
            print("On Level Start/Finish called");
        }


        [ListenTo(EventName.LevelEnd)]
        private void OnLevelEnded(EventModel info)
        {
            // This is unreachable
        }


        [ListenTo(EventName.GameStart, 100)]
        public static void OnGameStart(EventModel info)
        {
            print("OnGameStart, this will be called before other listeners. (priority: 100)");
        }


        [ListenTo(EventName.GamePause)]
        private static void OnGamePause(EventModel info)
        {
            // This is unreachable
        }


        [ListenTo(EventName.GameQuit)]
        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }
    }
}
