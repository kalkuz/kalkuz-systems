using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CustomEditor(typeof(ProjectileSkill), true)]
    [CanEditMultipleObjects]
    public class ProjectileSkillEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            List<string> arr = new List<string>();
            serializedObject.Update();

            ProjectileSkill _target = target as ProjectileSkill;

            #region "Excluding Fields"
            arr.Add("m_Script");

            if (_target.frontProjectileCount <= 1) arr.Add("projectilePositionOffset");
            if (_target.diagonalProjectileCount <= 0) arr.Add("maxProjectileSpreadAngle");

            #endregion

            GUI.enabled = false;
            SerializedProperty scr = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(scr, true, new GUILayoutOption[0]);
            GUI.enabled = true;

            DrawPropertiesExcluding(serializedObject, arr.ToArray());

            serializedObject.ApplyModifiedProperties();
        }
    }
}

