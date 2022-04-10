using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CustomEditor(typeof(BasicAttackSkill))]
    public class BasicAttackSkillEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            BasicAttackSkill _target = target as BasicAttackSkill;

            GUI.enabled = false;
            SerializedProperty scr = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(scr, true, new GUILayoutOption[0]);
            GUI.enabled = true;

            DrawPropertiesExcluding(serializedObject, "m_Script", "castAtExactRange", "skillManager");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
