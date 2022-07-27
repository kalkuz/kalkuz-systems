using KalkuzSystems.Attributes;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    public abstract class SkillData : ScriptableObject
    {
        #region Fields

        /// <summary>
        /// Name of the skill.
        /// </summary>
        [Title("Identification")]
        [SerializeField] protected string skillName;
        
        /// <summary>
        /// Description of what skill does.
        /// </summary>
        [SerializeField] protected string description;
        
        /// <summary>
        /// Icon of the skill.
        /// </summary>
        [SerializeField] protected Sprite icon;

        /// <summary>
        /// Dimensional limitation of the skill.
        /// </summary>
        [SerializeField] protected bool is2D;

        #endregion

        #region Properties

        /// <inheritdoc cref="skillName"/>
        public string SkillName => skillName;
        
        /// <inheritdoc cref="description"/>
        public string Description => description;
        
        /// <inheritdoc cref="icon"/>
        public Sprite Icon => icon;

        /// <inheritdoc cref="is2D"/>
        public bool Is2D => is2D;

        #endregion

        public abstract void Cast(Vector3 position, Vector3 direction, CharacterData targetCharacter, Transform source);
    }
}