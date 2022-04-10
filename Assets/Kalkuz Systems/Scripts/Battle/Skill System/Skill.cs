using KalkuzSystems.Attributes;
using UnityEngine;

using HeaderAttribute = KalkuzSystems.Attributes.HeaderAttribute;

namespace KalkuzSystems.Battle.SkillSystem
{
    /// <summary>
    /// Base class of all of the skill subclasses.
    /// </summary>
    public abstract class Skill : ScriptableObject
    {
        /// <summary>
        /// The name of the skill.
        /// </summary>
        [LineSeparator(1, 20, order = 0)]
        [Header("Skill Details", headerAlignment: HeaderAlignment.CENTER, order = 1)]
        [Tooltip("Name of the skill.")]
        public string skillName;
        
        /// <summary>
        /// Description of what the skill does or anything else.
        /// </summary>
        [Tooltip("Text that explains what the skill does."), TextArea(3, 10)]
        public string description;
        
        /// <summary>
        /// The sprite committed to the skill.
        /// </summary>
        [Tooltip("Icon of the skill which will be shown in the UI.")]
        public Sprite sprite; 
        
        /// <summary>
        /// Clones the Skill.
        /// </summary>
        /// <returns>The clone of the skill</returns>
        public abstract Skill Clone();
    }

    /// <summary>
    /// Used to distinguish between 2D, X-Z 3D and Full 3D games.
    /// </summary>
    public enum SkillDimension { XY, XZ, XYZ }
}
