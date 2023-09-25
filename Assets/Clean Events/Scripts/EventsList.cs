using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace CleanEvents
{
    [CreateAssetMenu(fileName = "Events List", menuName = "Events List")]
    public class EventsList : ScriptableObject
    {
        public List<string> EventNames = new List<string>()
        {
            "None",
            "GameStart",
            "GamePause",
            "GameContinue",
            "GameQuit",
            "LevelStart",
            "LevelFailed",
            "LevelPassed",
            "LevelEnd",
            "ShowMainMenu",
            "ShowPauseMenu",
            "ShowSettingsMenu",
            "ShowInventory",
            "ShowHUD",
            "LoadScene"
        };

#if UNITY_EDITOR
        public void GenerateEnumFile()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(
@"///////////////////////////////////////////
// This is a code-generated file, do not edit it, otherwise your changes will be lost!
///////////////////////////////////////////

namespace CleanEvents
{
    public enum EventName
    {");

            foreach (var evt in EventNames)
            {
                builder.AppendLine($"\t\t{evt},");
            }
            builder.Remove(builder.Length - 3, 1);
            builder.AppendLine(@"   }
}");

            CreateFile("Clean Events\\Scripts", "EventName.cs", builder.ToString());

            Debug.Log("EventName.cs Generated");

            AssetDatabase.Refresh();
        }

        private void CreateFile(string folder, string file, string content)
        {
            folder = Path.Combine(Application.dataPath, folder);

            if (Directory.Exists(folder) == false)
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, file);

            if (File.Exists(path) == false)
                File.CreateText(path).Close(); ;

            File.WriteAllText(path, content);
        }

        public static bool IsValidName(string name)
        {
            return new Regex("^([A-Za-z_])+([A-Za-z_\\d])*$").IsMatch(name);
        }
#endif
    }
}
