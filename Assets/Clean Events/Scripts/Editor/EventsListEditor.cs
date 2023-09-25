using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CleanEvents
{
    [CustomEditor(typeof(EventsList))]
    public class EventsListEditor : Editor
    {
        EventsList eventsList;
        private void OnEnable()
        {
            eventsList = (EventsList)serializedObject.targetObject;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Names must start with:\r\n\t► Letters\r\n\t► Underscore\r\n\r\nand only can contain:\r\n\t► Letters\r\n\t► Numbers\r\n\t► Underscores", MessageType.Info);

            base.OnInspectorGUI();

            EditorGUI.BeginDisabledGroup(ValidateNames() == false);
            {
                EditorGUILayout.Space();
                if (GUILayout.Button("Generate Enum"))
                {
                    eventsList.GenerateEnumFile();
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private bool ValidateNames()
        {
            if (eventsList.EventNames.Count == 0)
            {
                ShowMessaegBox("Events list cannot be empty.", new List<string>(), MessageType.Error);

                return false;
            }

            HashSet<string> uniques = new HashSet<string>();
            List<string> validNames = new List<string>();
            List<string> duplicates = new List<string>();
            List<string> invalidNames = new List<string>();

            foreach (var item in eventsList.EventNames)
                if (EventsList.IsValidName(item))
                {
                    if (uniques.Contains(item))
                    {
                        duplicates.Add(item);
                    }
                    else
                    {
                        validNames.Add(item);
                        uniques.Add(item);
                    }
                }
                else
                {
                    invalidNames.Add(item);
                }

            if (duplicates.Count > 0)
            {
                ShowMessaegBox("Following name(s) are already declared:", duplicates, MessageType.Error);

                return false;
            }

            if (invalidNames.Count > 0)
            {
                ShowMessaegBox("Following name(s) are invalid:", invalidNames, MessageType.Error);

                return false;
            }

            return true;
        }

        private void ShowMessaegBox(string firstLine, List<string> list, MessageType type)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(firstLine);
            sb.AppendLine(string.Join("\r\n", list));

            EditorGUILayout.HelpBox(sb.ToString(), type);
        }
    }
}
