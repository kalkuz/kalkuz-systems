using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KalkuzSystems.Attributes;
using KalkuzSystems.Battle.BuffSystem;

namespace KalkuzSystems.Battle
{
    /// <summary>
    /// Stat data container.
    /// </summary>
    [Serializable]
    public class Stats
    {
        #region Fields

        /// <summary>
        /// Resource list which includes health, mana etc.
        /// </summary>
        [Title("Main Stats")]
        [SerializeField] protected List<ResourceContainer> resources;
        
        /// <summary>
        /// The size of the character.
        /// </summary>
        public float characterSize;

        /// <summary>
        /// Damage resistance list such as Physical Resistance.
        /// </summary>
        [Title("Resistances", order = 1)]
        [SerializeField] protected List<ResistanceContainer> resistances;
        
        /// <summary>
        /// List of Status Effect immunity.
        /// </summary>
        [SerializeField] protected List<StatusEffect> immunities;

        /// <summary>
        /// The indicator of the pace of movement.
        /// </summary>
        [Title("Movement", order = 1)]
        public float movementSpeed;
        
        /// <summary>
        /// Determines the boost in the movement speed in percent.
        /// </summary>
        public float moveFastPercent;
        
        /// <summary>
        /// Determines the reduction in the movement speed in percent.
        /// </summary>
        public float moveSlowPercent;

        #endregion

        #region Properties
        
        /// <inheritdoc cref="resources"/>
        public IReadOnlyList<ResourceContainer> Resources => resources;
        
        /// <inheritdoc cref="resistances"/>
        public IReadOnlyList<ResistanceContainer> Resistances => resistances;
        
        /// <inheritdoc cref="immunities"/>
        public IReadOnlyList<StatusEffect> Immunities => immunities;

        #endregion

        #region Methods

        /// <summary>
        /// Clones the Stats object.
        /// </summary>
        /// <returns>Clone of the Stats object.</returns>
        public virtual Stats Clone()
        {
            Stats result = new Stats
            {
                resources = resources.ConvertAll(i => i.Clone()),
                characterSize = characterSize,
                resistances = resistances.ConvertAll(i => i.Clone()),
                immunities = new List<StatusEffect>(immunities),
                movementSpeed = movementSpeed,
                moveFastPercent = moveFastPercent,
                moveSlowPercent = moveSlowPercent
            };

            return result;
        }
        
        /// <summary>
        /// Finds the resistance with same type.
        /// </summary>
        /// <param name="damageType">The type to look at.</param>
        /// <returns>The <see cref="ResistanceContainer"/> including the given type. Returns null if there is no.</returns>
        public ResistanceContainer GetResistance(DamageType damageType)
        {
            return resistances.FirstOrDefault(item => item.ResistanceType == damageType);
        }
        
        
        /// <summary>
        /// Finds the resource with same type.
        /// </summary>
        /// <param name="resourceType">The type to look at.</param>
        /// <returns>The <see cref="ResourceContainer"/> including the given type. Returns null if there is no.</returns>
        public ResourceContainer GetResource(ResourceType resourceType)
        {
            return resources.FirstOrDefault(item => item.ResourceType == resourceType);
        }

        #endregion
    }

    [Serializable]
    public class ResistanceContainer
    {
        #region Fields

        /// <summary>
        /// Determines the maximum allowed resistance.
        /// </summary>
        private const float CONSTANT_RESISTANCE_CAP = 0.9f;

        /// <summary>
        /// The type of the resistance.
        /// </summary>
        [SerializeField] protected DamageType resistanceType;
        
        /// <summary>
        /// Amount of the resistance.
        /// </summary>
        [SerializeField, Range(-1f, CONSTANT_RESISTANCE_CAP)] protected float amount;

        #endregion

        #region Properties

        /// <inheritdoc cref="resistanceType"/>
        public DamageType ResistanceType => resistanceType;

        /// <inheritdoc cref="amount"/>
        public float Amount
        {
            get => amount;
            set => amount = Mathf.Clamp(value, -1, CONSTANT_RESISTANCE_CAP);
        }

        #endregion

        /// <summary>
        /// Clones the ResistanceContainer Object.
        /// </summary>
        /// <returns>Clone of the ResistanceContainer Object.</returns>
        public ResistanceContainer Clone()
        {
            ResistanceContainer clone = new ResistanceContainer
            {
                resistanceType = resistanceType,
                amount = amount
            };

            return clone;
        }
    }

    [Serializable]
    public class ResourceContainer
    {
        #region Fields

        /// <summary>
        /// The type of the resource.
        /// </summary>
        [SerializeField] protected ResourceType resourceType;
        
        /// <summary>
        /// Amount of the resource.
        /// </summary>
        [SerializeField] protected float amount;
        
        /// <summary>
        /// The multiplier of the increase in resource coming from the level.
        /// </summary>
        [SerializeField] protected float increasePerLevelScalar;
        
        /// <summary>
        /// The curve of the increase.
        /// </summary>
        [SerializeField] protected AnimationCurve increaseCurve;

        #endregion

        #region Properties

        /// <inheritdoc cref="resourceType"/>
        public ResourceType ResourceType => resourceType;

        /// <inheritdoc cref="amount"/>
        public float Amount
        {
            get => amount;
            set => amount = value;
        }

        /// <inheritdoc cref="increasePerLevelScalar"/>
        public float IncreasePerLevelScalar => increasePerLevelScalar;

        /// <inheritdoc cref="increaseCurve"/>
        public AnimationCurve IncreaseCurve => increaseCurve;

        #endregion

        /// <summary>
        /// Clones the ResourceContainer Object.
        /// </summary>
        /// <returns>Clone of the ResourceContainer Object.</returns>
        public ResourceContainer Clone()
        {
            ResourceContainer clone = new ResourceContainer
            {
                resourceType = resourceType,
                amount = amount,
                increasePerLevelScalar = increasePerLevelScalar,
                increaseCurve = increaseCurve
            };

            return clone;
        }
    }
}
