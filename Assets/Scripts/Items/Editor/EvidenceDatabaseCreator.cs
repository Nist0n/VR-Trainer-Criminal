using UnityEngine;
using UnityEditor;
using System.IO;

namespace Items.Editor
{
    public class EvidenceDatabaseCreator
    {
        [MenuItem("Tools/Create Evidence Database")]
        public static void CreateEvidenceDatabase()
        {
            // Убедимся, что папка Resources существует
            if (!Directory.Exists("Assets/Resources"))
            {
                Directory.CreateDirectory("Assets/Resources");
            }

            // Проверим, существует ли уже база данных
            var database = Resources.Load<EvidenceDatabase>("EvidenceDatabase");
            if (database == null)
            {
                // Создаем новую базу данных
                database = ScriptableObject.CreateInstance<EvidenceDatabase>();
                AssetDatabase.CreateAsset(database, "Assets/Resources/EvidenceDatabase.asset");
                AssetDatabase.SaveAssets();
                Debug.Log("EvidenceDatabase created in Resources folder");
            }
            else
            {
                Debug.Log("EvidenceDatabase already exists");
            }

            // Показываем базу данных в Project window
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = database;
        }
    }
} 