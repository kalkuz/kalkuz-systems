using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CustomEditor(typeof(AoeSkill))]
    [CanEditMultipleObjects]
    public class AoeSkillEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            AoeSkill _target = target as AoeSkill;

            GUI.enabled = false;
            SerializedProperty scr = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(scr, true, new GUILayoutOption[0]);
            GUI.enabled = true;

            DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}


