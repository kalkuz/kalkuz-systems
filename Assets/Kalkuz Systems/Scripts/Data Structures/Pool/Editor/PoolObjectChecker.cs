using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace KalkuzSystems.DataStructures.Pooling
{
    public class PoolObjectChecker : EditorWindow
    {
        List<PoolObject> objList = new List<PoolObject>();

        [MenuItem("Kalkuz Systems/Pooling/Check Pool Objects")]
        static void CheckPoolObjects()
        {
            var window = GetWindow<PoolObjectChecker>();
            window.titleContent = new GUIContent("Pool Object Checker");
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

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("ID", centeredLabel, GUILayout.MaxWidth(50));
            EditorGUILayout.LabelField("GameObject", centeredLabel);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            centeredLabel.fontStyle = FontStyle.Normal;

            foreach (PoolObject item in objList)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(item.ID.ToString(), centeredLabel, GUILayout.MaxWidth(50));

                GUI.enabled = false;
                EditorGUILayout.ObjectField(item.gameObject, typeof(GameObject), allowSceneObjects: false);
                GUI.enabled = true;

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
