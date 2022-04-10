using UnityEngine;

namespace KalkuzSystems.Attributes
{
    public enum HeaderAlignment
    {
        LEFT,
        CENTER,
        RIGHT
    }

    /// <summary>
    /// Better header attribute with centering option.
    /// </summary>
    public sealed class HeaderAttribute : PropertyAttribute
    {
        public readonly string header;
        public readonly int fontSize;
        public readonly string fontColor;
        public readonly HeaderAlignment headerAlignment;

        /// <param name="header">The text of the header</param>
        /// <param name="fontSize">The size of the header's font appear in editor</param>
        /// <param name="fontColor">The color of the header's font in hex color</param>
        /// <param name="headerAlignment">Determines how the Header will be aligned</param>
        public HeaderAttribute(string header, int fontSize = 16, string fontColor = "#ffffff", HeaderAlignment headerAlignment = HeaderAlignment.LEFT)
        {
            this.header = header;
            this.fontSize = fontSize;
            this.fontColor = fontColor;
            this.headerAlignment = headerAlignment;
        }
    }
}