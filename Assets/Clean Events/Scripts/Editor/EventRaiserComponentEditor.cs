using UnityEditor;
using UnityEngine;

namespace CleanEvents
{
    public class EventRaiserEditor<T> : Editor where T : MonoBehaviour
    {
        protected T raiser;

        private void OnEnable()
        {
            raiser = serializedObject.targetObject as T;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(Application.isPlaying == false);
            {
                if (GUILayout.Button("Raise Events"))
                {
                    OnButtonPressed();
                }
            }
            if (Application.isPlaying == false)
            {
                EditorGUILayout.LabelField("This button will be active during play mode", new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Italic
                });
            }
            EditorGUI.EndDisabledGroup();
        }

        protected virtual void OnButtonPressed()
        {
        }
    }

    [CustomEditor(typeof(EventRaiserSingle))]
    public class EventRaiserSingleEditor : EventRaiserEditor<EventRaiserSingle>
    {
        protected override void OnButtonPressed()
        {
            raiser.Raise();
        }
    }

    [CustomEditor(typeof(EventRaiserList))]
    public class EventRaiserListEditor : EventRaiserEditor<EventRaiserList>
    {
        protected override void OnButtonPressed()
        {
            raiser.Raise();
        }
    }
}
