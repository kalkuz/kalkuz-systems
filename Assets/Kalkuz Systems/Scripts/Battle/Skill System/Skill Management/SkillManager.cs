using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Analysis.Debugging;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.SkillSystem
{
    /// <summary>
    /// Used as a container of multiple <see cref="SkillContainer"/>s.
    /// </summary>
    [System.Serializable]
    public class SkillManager
    {
        /// <summary>
        /// Skills that the entity can cast.
        /// </summary>
        [Tooltip("Skills that the entity can cast.")]
        public List<SkillContainer> skills;
    }

    /// <summary>
    /// Used to hold <see cref="UsableSkill"/> and its <see cref="SkillUpgrade"/>s together.
    /// </summary>
    [System.Serializable]
    public class SkillContainer
    {
        /// <summary>
        /// Determines how or when the skill will be casted.
        /// </summary>
        public enum CastAutomation { INPUT, ON_START, ON_HIT, ON_END, OVER_TIME }

        /// <summary>
        /// <inheritdoc cref="CastAutomation"/>
        /// </summary>
        [Tooltip("Determines how or when the skill will be casted.")]
        public CastAutomation castAutomation;
        
        /// <summary>
        /// Used only if the <see cref="castAutomation"/> set to <see cref="CastAutomation.OVER_TIME"/>.
        /// It determines the time difference between casts.
        /// </summary>
        [Tooltip("If it is an overtime automation, it specifies the delta between automatic casts.")]
        public float secondsBetweenCasts;
        
        /// <summary>
        /// If skill casted from another skill, this helps to inherit Damage Type Upgrades that was on the parent.
        /// </summary>
        [Space, Tooltip("If skill casted from another skill, this helps to inherit Damage Type Upgrades that was on the parent.")]
        public bool inheritDamageTypeUpgrade;
        
        /// <summary>
        /// Used to show whether or not the skill is ready to cast.
        /// </summary>
        [Space, Tooltip("Used to show whether or not the skill is ready to cast.")]
        public bool available;

        /// <summary>
        /// The pure asset of the <see cref="UsableSkill"/> that will be copied and modified by <see cref="attachments"/> later.
        /// </summary>
        [SerializeField, Tooltip("The skill that is going to be enhanced by attachments.")] 
        UsableSkill baseSkill;
        
        /// <summary>
        /// The layers that the skill can interact with.
        /// </summary>
        [SerializeField, Tooltip("The layers that the skill can interact with.")] 
        LayerMask targets;
        [HideInInspector] public UsableSkill skill;
        
        /// <summary>
        /// Attachments which are used to improve the <see cref="baseSkill"/>'s capability.
        /// </summary>
        public List<SkillUpgrade> attachments = new List<SkillUpgrade>();

        /// <summary>
        /// Should be invoked after every change in the <see cref="attachments"/>.
        /// </summary>
        public UnityEvent OnAttachmentChanged;

        /// <summary>
        /// Initializes the skill container by applying upgrades to base skill.
        /// </summary>
        public void Initialize()
        {
            if (baseSkill == null)
            {
                KalkuzLogger.Warning("Base Skill was not assigned.");
                return;
            }
            skill = baseSkill.Clone() as UsableSkill;

            if (skill == null)
            {
                KalkuzLogger.Warning("Base skill is not an Usable Skill.");
                return;
            }

            skill.targets = targets;

            foreach (SkillUpgrade u in attachments)
            {
                if (u != null) u.ApplyUpgrade(skill);
            }
        }
    }
}

