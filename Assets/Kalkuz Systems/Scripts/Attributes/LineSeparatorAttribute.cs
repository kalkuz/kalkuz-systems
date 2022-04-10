using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Attributes
{
    /// <summary>
    /// Adds Line Separator before the property in Inspector.
    /// </summary>
    public sealed class LineSeparatorAttribute : PropertyAttribute
    {
        public readonly float height;
        public readonly float verticalPadding;
        public readonly float grayScale;
        
        /// <param name="height">The height of the line</param>
        /// <param name="verticalPadding">The vertical spacing of the line</param>
        /// <param name="grayScale">The color of the line in terms of gray scale represented between 0 and 1</param>
        public LineSeparatorAttribute(float height, float verticalPadding, float grayScale = 0.4f)
        {
            this.height = height;
            this.verticalPadding = verticalPadding;
            this.grayScale = grayScale;
        }
    }
}