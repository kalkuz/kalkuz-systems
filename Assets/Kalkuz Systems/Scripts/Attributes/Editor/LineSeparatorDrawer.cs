using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Attributes
{
    [CustomPropertyDrawer(typeof(LineSeparatorAttribute))]
    internal sealed class LineSeparatorDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            LineSeparatorAttribute att = attribute as LineSeparatorAttribute;

            Rect lineRect = new Rect(position.x, position.y, position.width, att.verticalPadding);
            Rect line = new Rect(lineRect.x, lineRect.y + att.verticalPadding, lineRect.width, att.height);
            position.y += att.verticalPadding * 2;
            position.height -= att.verticalPadding * 2;

            EditorGUI.DrawRect(line, new Color(att.grayScale, att.grayScale, att.grayScale));
        }

        public override float GetHeight()
        {
            return (attribute as LineSeparatorAttribute).verticalPadding * 2;
        }
    }
}