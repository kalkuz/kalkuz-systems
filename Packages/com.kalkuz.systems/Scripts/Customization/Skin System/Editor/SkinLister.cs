using System.Collections.Generic;
using System.Linq;
using KalkuzSystems.DataStructures.Pooling;
using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Customization.SkinSystem.Editor
{
    public class SkinLister : EditorWindow
    {
        private Vector2 scrollPosition;
        
        List<SkinData> objList = new List<SkinData>();

        [MenuItem("Kalkuz Systems/Customization/Check Skins")]
        static void CheckPoolObjects()
        {
            var window = GetWindow<SkinLister>();
            window.titleContent = new GUIContent("Skin Lister");
            window.minSize = new Vector2(600, 600);
            window.Show();

            window.FindObjects();
        }

        void FindObjects()
        {
            objList.Clear();
            string[] GUIDs = AssetDatabase.FindAssets("t:GameObject");
            foreach (string GUID in GUIDs)
            {
                SkinData skinData = AssetDatabase.LoadAssetAtPath<SkinData>(AssetDatabase.GUIDToAssetPath(GUID));
                if (skinData != null) objList.Add(skinData);
            }

            objList = objList.OrderBy(x => x.SkinID).ToList();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Refresh")) FindObjects();
            EditorGUILayout.Space();

            GUIStyle centeredLabel = new GUIStyle(GUI.skin.label);
            centeredLabel.alignment = TextAnchor.UpperCenter;
            centeredLabel.fontStyle = FontStyle.Bold;
            centeredLabel.richText = true;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("ID", centeredLabel);
            EditorGUILayout.LabelField("Asset", centeredLabel);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            centeredLabel.fontStyle = FontStyle.Normal;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (SkinData item in objList)
            {
                bool hasDuplicate = objList.Find((i) => i != item && i.SkinID == item.SkinID);
                string fontColor = hasDuplicate ? "#ff7777" : "#77ff77";
                
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"<color={fontColor}>{item.SkinID}</color>", centeredLabel);

                GUI.enabled = false;
                EditorGUILayout.ObjectField(item, typeof(GameObject), false);
                GUI.enabled = true;

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}