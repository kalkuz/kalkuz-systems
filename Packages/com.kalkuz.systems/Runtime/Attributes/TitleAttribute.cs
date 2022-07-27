using UnityEngine;

namespace KalkuzSystems.Attributes
{
    public enum TitleAlignment
    {
        LEFT,
        CENTER,
        RIGHT
    }

    /// <summary>
    /// Better header attribute with customization option.
    /// </summary>
    public sealed class TitleAttribute : PropertyAttribute
    {
        public readonly string header;
        public readonly int fontSize;
        public readonly string fontColor;
        public readonly TitleAlignment titleAlignment;

        /// <param name="header">The text of the header</param>
        /// <param name="fontSize">The size of the header's font appear in editor</param>
        /// <param name="fontColor">The color of the header's font in hex color</param>
        /// <param name="titleAlignment">Determines how the Header will be aligned</param>
        public TitleAttribute(string header, int fontSize = 16, string fontColor = "#ffffff", TitleAlignment titleAlignment = TitleAlignment.CENTER)
        {
            this.header = header;
            this.fontSize = fontSize;
            this.fontColor = fontColor;
            this.titleAlignment = titleAlignment;
        }
    }
}