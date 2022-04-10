using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Attributes
{
    [CustomPropertyDrawer(typeof(HeaderAttribute))]
    internal sealed class HeaderDrawer : DecoratorDrawer
    {
        private readonly GUIStyle style = new GUIStyle()
        {
            richText = true,
            fontStyle = FontStyle.Bold, 
            alignment = TextAnchor.MiddleLeft
        };
        
        public override void OnGUI(Rect position)
        {
            var att = attribute as HeaderAttribute;
            if (att == null) return;

            if (att.headerAlignment == HeaderAlignment.CENTER) style.alignment = TextAnchor.MiddleCenter;
            else if (att.headerAlignment == HeaderAlignment.RIGHT) style.alignment = TextAnchor.MiddleRight;

            style.fontSize = att.fontSize;

            EditorGUI.LabelField(position, $"<color={att.fontColor}>{att.header}</color>", style);
        }

        public override float GetHeight()
        {
            var att = attribute as HeaderAttribute;
            
            float fullTextHeight = EditorStyles.boldLabel.CalcHeight(new GUIContent(att.header), 1.0f);
            return EditorGUIUtility.singleLineHeight * 1.5f + (fullTextHeight);
        }
    }
}