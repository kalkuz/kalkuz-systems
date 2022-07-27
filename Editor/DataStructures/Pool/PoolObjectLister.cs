using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace KalkuzSystems.DataStructures.Pooling
{
    public class PoolObjectLister : EditorWindow
    {
        private Vector2 scrollPosition;
        
        List<PoolObject> objList = new List<PoolObject>();

        [MenuItem("Kalkuz Systems/Pooling/Check Pool Objects")]
        static void CheckPoolObjects()
        {
            var window = GetWindow<PoolObjectLister>();
            window.titleContent = new GUIContent("Pool Object Lister");
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
                PoolObject poolObj = AssetDatabase.LoadAssetAtPath<PoolObject>(AssetDatabase.GUIDToAssetPath(GUID));
                if (poolObj != null) objList.Add(poolObj);
            }

            objList = objList.OrderBy(x => x.ID).ToList();
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
            EditorGUILayout.LabelField("GameObject", centeredLabel);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            centeredLabel.fontStyle = FontStyle.Normal;

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (PoolObject item in objList)
            {
                bool hasDuplicate = objList.Find((i) => i != item && i.ID == item.ID);
                string fontColor = hasDuplicate ? "#ff7777" : "#77ff77";
                
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"<color={fontColor}>{item.ID}</color>", centeredLabel);

                GUI.enabled = false;
                EditorGUILayout.ObjectField(item.gameObject, typeof(GameObject), false);
                GUI.enabled = true;

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
