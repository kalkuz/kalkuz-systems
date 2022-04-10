using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Attributes;
using KalkuzSystems.Battle.BuffSystem;
using UnityEngine;

using HeaderAttribute = KalkuzSystems.Attributes.HeaderAttribute;

namespace KalkuzSystems.Battle.SkillSystem
{
    /// <summary>
    /// The skill which is going to be interacted or casted to observe their behaviour.
    /// </summary>
    public abstract class UsableSkill : Skill
    {
        /// <summary>
        /// Indicates the dimension of the skill in terms of XY, XZ and XYZ.
        /// </summary>
        [LineSeparator(1, 16, order = 0)]
        [Header("Skill Foundation", headerAlignment: HeaderAlignment.CENTER, order = 1)]
        [Tooltip("Indicates the dimension of the skill in terms of XY, XZ and XYZ.")]
        public SkillDimension skillDimension;
        
        [Tooltip("How long does it take between the use of the skill consecutively.")]
        public float cooldown;
        
        [Tooltip("Skill range")]
        public float range;
        [Tooltip("Casts exact distance of range")]
        public bool castsExactlyAtRange;
        public bool requiresTargetCharacter;
        
        [LineSeparator(1,16)]
        [Tooltip("The Animator trigger name which the skill should call.")]
        public string animationCallName;
        [Tooltip("Vfx of the skill that is shown while casting.")]
        public GameObject preAnimationVFX;
        
        [LineSeparator(1,16)]
        [Tooltip("Buffs that the skill applies on successful hit.")]
        public List<Buff> buffsToApply;
        [Tooltip("Damages of the skill.")]
        public DamageList damages;
        [HideInInspector] public LayerMask targets; // Set it when casting

        [HideInInspector] public CharacterData targetCharacter; // Set it when casting
        [HideInInspector] public Vector3 castPosition; // Set it where to hit when casting.

        [LineSeparator(1,16, order = 0)]
        [Header("Toggle", headerAlignment: HeaderAlignment.CENTER, order = 1)]
        public bool isToggled;
        public List<SkillToggleEvent> toggleEvents;
        [HideInInspector] public int currentToggle;

        [Header("Prefabs",order = 1)]
        [Tooltip("The area object that the skill will cast.")]
        public GameObject defaultSkillPrefab;
        public List<SkillPrefabModificationContainer> modificationContainers;

        [Header("Behaviours")]
        public bool destroyBeforeInstantiatingNew;
        [HideInInspector] public List<GameObject> instantiatedBehaviours;

        [Header("Attachment")]
        [Tooltip("Aoe skill which will be casted on the position of where the projectile hits, and when it hits something.")]
        public SkillManager skillManager;

        [Header("Stats")]
        public float accuracy;
        public float critChance;
        public float critMultiplier;

        //return false if casting fails. return true if casting successful.
        public abstract bool Cast(SkillCaster caster);
        public abstract bool IsInRange(SkillCaster caster, Vector3 targetPosition);

        public void Toggle()
        {
            if (!isToggled) return;
            if (toggleEvents.Count == 0) return;

            toggleEvents[currentToggle].toggleEvent?.Invoke();
            currentToggle = (currentToggle + 1) % toggleEvents.Count;
        }

        public void DestroyInstantiated()
        {
            foreach (GameObject g in instantiatedBehaviours)
            {
                Destroy(g);
            }
        }

        public void InstantiatedOnDestroyBehaviour()
        {
            if (isToggled)
            {
                var item = toggleEvents.Find(i => i.eventType == SkillToggleEventType.DESTROY_INSTANTIATED_BEHAVIOUR);
                var index = toggleEvents.IndexOf(item);
                if (currentToggle == index)
                {
                    currentToggle = (toggleEvents.IndexOf(item) + 1) % toggleEvents.Count;
                }
            }
        }
    }
}
