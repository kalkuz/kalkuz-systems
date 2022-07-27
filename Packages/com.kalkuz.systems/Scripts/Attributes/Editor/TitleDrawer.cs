using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Attributes
{
    [CustomPropertyDrawer(typeof(TitleAttribute))]
    internal sealed class TitleDrawer : DecoratorDrawer
    {
        private readonly GUIStyle style = new GUIStyle()
        {
            richText = true,
            fontStyle = FontStyle.Bold, 
            alignment = TextAnchor.MiddleCenter
        };
        
        public override void OnGUI(Rect position)
        {
            var att = attribute as TitleAttribute;
            if (att == null) return;

            if (att.titleAlignment == TitleAlignment.LEFT) style.alignment = TextAnchor.MiddleLeft;
            else if (att.titleAlignment == TitleAlignment.RIGHT) style.alignment = TextAnchor.MiddleRight;

            style.fontSize = att.fontSize;

            EditorGUI.LabelField(position, $"<color={att.fontColor}>{att.header}</color>", style);
        }

        public override float GetHeight()
        {
            var att = attribute as TitleAttribute;
            
            float fullTextHeight = EditorStyles.boldLabel.CalcHeight(new GUIContent(att.header), 1.0f);
            return EditorGUIUtility.singleLineHeight * 1.5f + (fullTextHeight);
        }
    }
}