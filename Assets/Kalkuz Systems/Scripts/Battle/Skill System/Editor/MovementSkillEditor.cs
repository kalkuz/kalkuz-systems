using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CustomEditor(typeof(MovementSkill))]
    [CanEditMultipleObjects]
    public class MovementSkillEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            MovementSkill _target = (target as MovementSkill);

            GUI.enabled = false;
            SerializedProperty scr = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(scr, true, new GUILayoutOption[0]);
            GUI.enabled = true;

            if (_target.movesOverObstacles) DrawPropertiesExcluding(serializedObject, "m_Script", "immovableSurfaces");
            else DrawPropertiesExcluding(serializedObject, "m_Script");

            serializedObject.ApplyModifiedProperties();
        }
    }
}
