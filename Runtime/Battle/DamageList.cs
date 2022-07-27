using System.Collections.Generic;
using System.Runtime.Serialization;
using KalkuzSystems.Battle;
using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using UnityEngine.Events;

namespace KalkuzSystems.Battle
{
    [System.Serializable]
    public sealed class DamageList
    {
        [SerializeField] private List<Damage> list;

        public IReadOnlyList<Damage> List => list;

        #region Constructors

        public DamageList()
        {
            list = new List<Damage>();
        }

        public DamageList(DamageList other)
        {
            list = new List<Damage>(other.List);
        }

        #endregion

        #region ListMethods

        public void Add(Damage damage)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].damageType == damage.damageType && list[i].damageApplicationType == damage.damageApplicationType)
                {
                    list[i].damageRange += damage.damageRange;
                    return;
                }
            }

            list.Add(damage);
        }

        public void UpdateDamageRange(int index, Vector2 newValue)
        {
            list[index].damageRange = newValue;
        }

        public void UpdateDamageRange(Damage damage, Vector2 newValue)
        {
            var element = list.Find(d => d == damage);
            if (element != null) element.damageRange = newValue;
        }

        public int IndexOf(Damage damage)
        {
            return list.IndexOf(damage);
        }

        public bool Remove(Damage damage)
        {
            return list.Remove(damage);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        #endregion

        #region Editor

#if UNITY_EDITOR

        [CustomPropertyDrawer(typeof(DamageList))]
        internal class DamageListDrawer : PropertyDrawer
        {
            private ReorderableList rList;
            private List<Damage> serializedList;
            private DamageList damageListObj;

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                var serializedObj = property.serializedObject;
                var targetObj = serializedObj.targetObject;

                damageListObj = targetObj.GetType().GetField(property.propertyPath).GetValue(property.serializedObject.targetObject) as DamageList;
                if (damageListObj == null) return;

                serializedList = damageListObj.list;

                if (rList == null)
                {
                    rList = new ReorderableList(serializedList, typeof(Damage), true, true, true, true)
                    {
                        elementHeight = EditorGUIUtility.singleLineHeight * 3f + 6
                    };

                    rList.drawHeaderCallback += (rect) => DrawHeaderCallback(rect, property.displayName);
                    rList.onAddCallback += (rList) => OnAddCallback(rList, property);
                    rList.onRemoveCallback += (rList) => OnRemoveCallback(rList, property);
                    rList.drawElementCallback += DrawElementCallback;
                }

                rList.DoList(EditorGUI.IndentedRect(position));
            }

            void OnAddCallback(ReorderableList reorderableList, SerializedProperty property)
            {
                EditorWindow.GetWindow<NewDamageWindow>().Init((damage) =>
                {
                    damageListObj.Add(damage);
                    
                    EditorUtility.SetDirty(property.serializedObject.targetObject);

                    property.serializedObject.Update();
                    property.serializedObject.ApplyModifiedProperties();
                    
                    AssetDatabase.SaveAssets();
                }).Show();
            }
            
            void OnRemoveCallback(ReorderableList reorderableList, SerializedProperty property)
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(reorderableList);
                
                EditorUtility.SetDirty(property.serializedObject.targetObject);
                
                property.serializedObject.Update();
                property.serializedObject.ApplyModifiedProperties();
                
                AssetDatabase.SaveAssets();
            }

            void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
            {
                Damage element = serializedList[index];

                rect = new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight);
                element.damageRange = EditorGUI.Vector2Field(rect, "Damage Range", element.damageRange);

                bool prevState = GUI.enabled;
                GUI.enabled = false;
                rect = new Rect(rect.x, rect.y + 2 + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight);
                element.damageType = (DamageType) EditorGUI.EnumPopup(rect, "Damage Type", element.damageType);

                rect = new Rect(rect.x, rect.y + 2 + EditorGUIUtility.singleLineHeight, rect.width, EditorGUIUtility.singleLineHeight);
                element.damageApplicationType = (DamageApplicationType) EditorGUI.EnumPopup(rect, "Damage Application Type", element.damageApplicationType);
                GUI.enabled = prevState;
            }

            void DrawHeaderCallback(Rect rect, string name)
            {
                EditorGUI.LabelField(rect, name);
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUIUtility.singleLineHeight * 0.5f + (rList == null ? 0f : rList.headerHeight + rList.footerHeight + rList.elementHeight * Mathf.Max(1f, rList.count));
            }
        }

        internal class NewDamageWindow : EditorWindow
        {
            Damage damage;
            UnityAction<Damage> onCreate;

            public EditorWindow Init(UnityAction<Damage> onCreateCallback)
            {
                damage = new Damage();
                onCreate = onCreateCallback;
                return this;
            }

            private void OnGUI()
            {
                damage.damageRange = EditorGUILayout.Vector2Field("Damage Range", damage.damageRange);
                damage.damageType = (DamageType) EditorGUILayout.EnumPopup("Damage Type", damage.damageType);
                damage.damageApplicationType = (DamageApplicationType) EditorGUILayout.EnumPopup("Damage Application Type", damage.damageApplicationType);

                if (GUILayout.Button("Create"))
                {
                    onCreate.Invoke(damage);
                    damage = new Damage(damage.damageRange, damage.damageType, damage.damageApplicationType);
                }
            }
        }
#endif

        #endregion
    }
}