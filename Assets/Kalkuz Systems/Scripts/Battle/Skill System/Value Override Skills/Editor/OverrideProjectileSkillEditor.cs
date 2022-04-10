using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CustomEditor(typeof(OverrideProjectileSkill))]
    public class OverrideProjectileSkillEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MovementSkill _target = (target as MovementSkill);

            GUI.enabled = false;
            SerializedProperty scr = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(scr, true, new GUILayoutOption[0]);
            GUI.enabled = true;

            DrawPropertiesExcluding(serializedObject, "m_Script", "damages", "buffsToApply", "defaultSkillPrefab", "modificationContainers",
                                     "skillManager", "accuracy", "critChance", "critMultiplier");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
