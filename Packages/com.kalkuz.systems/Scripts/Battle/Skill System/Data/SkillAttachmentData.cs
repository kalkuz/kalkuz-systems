using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    public abstract class SkillAttachmentData : ScriptableObject
    {
        #region Fields

        /// <summary>
        /// Name of the attachment.
        /// </summary>
        [SerializeField] protected string attachmentName;
        
        /// <summary>
        /// Description of what attachment does.
        /// </summary>
        [SerializeField] protected string description;
        
        /// <summary>
        /// Icon of the attachment.
        /// </summary>
        [SerializeField] protected Sprite icon;

        #endregion

        #region Properties

        /// <inheritdoc cref="attachmentName"/>
        public string AttachmentName => attachmentName;
        
        /// <inheritdoc cref="description"/>
        public string Description => description;
        
        /// <inheritdoc cref="icon"/>
        public Sprite Icon => icon;

        #endregion
    }
}