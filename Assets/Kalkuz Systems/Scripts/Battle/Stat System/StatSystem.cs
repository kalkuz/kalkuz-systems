using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KalkuzSystems.Battle.BuffSystem;
using UnityEngine.Events;

namespace KalkuzSystems.Battle
{
    [System.Serializable]
    public class StatSystem
    {
        #region Fields

        /// <summary>
        /// The base stat values of the character.
        /// </summary>
        [Tooltip("Base stats of the entity that is used to get reference values. These do not change at runtime.")]
        [SerializeField] protected Stats baseStats;
        
        /// <summary>
        /// Modified values of the stats which are current for the character.
        /// </summary>
        protected Stats currentStats = new Stats();
        
        /// <summary>
        /// The level object which manages the stat system's leveling properties.
        /// </summary>
        [SerializeField] protected Level level;
        
        /// <summary>
        /// The list of buffs and debuffs which are inflicted and live on the Stat System.
        /// </summary>
        public Dictionary<StatusEffect, Buff> buffs;

        private CharacterData m_owner;
        private Animator m_animator;

        #endregion
        
        #region Properties

        /// <summary>
        /// The base stat values of the character.
        /// </summary>
        public Stats BaseStats => baseStats;

        /// <summary>
        /// Modified values of the stats which are current for the character.
        /// </summary>
        public Stats CurrentStats => currentStats;

        /// <summary>
        /// The level of the character.
        /// </summary>
        public Level Level => level;

        #endregion

        /// <summary>
        /// Initializes the Stat System applying the modifications on the current stats and process.
        /// </summary>
        /// <param name="owner">The owner of the Stat System</param>
        public virtual void Initialize(CharacterData owner)
        {
            currentStats = baseStats.Clone();
            m_owner = owner;

            buffs = new Dictionary<StatusEffect, Buff>();

            m_animator = m_owner.GetComponentInChildren<Animator>();
        }

        /// <summary>
        /// Used to inflict damage on the stat system. Heals the character if damage turns out to be negative.
        /// </summary>
        /// <param name="damage">The Damage object holding data</param>
        /// <param name="accuracy">The accuracy of the hit</param>
        /// <param name="critChance">The critical strike chance of the hit</param>
        /// <param name="critMultiplier">The multiplier of damage if it is critical</param>
        public virtual void TakeDamage(Damage damage, float accuracy, float critChance, float critMultiplier)
        {
            if (accuracy < Mathf.Clamp(Random.value, 0f, 0.99f)) return;

            ResistanceContainer resistance = CurrentStats.GetResistance(damage.damageType);
            ResourceContainer baseHealth = baseStats.GetResource(ResourceType.HEALTH);
            ResourceContainer currentHealth = CurrentStats.GetResource(ResourceType.HEALTH);

            if (baseHealth == null || currentHealth == null) return;

            bool isCrit = Mathf.Clamp(Random.value, 0f, 0.99f) < critChance;
            float dmg = Random.Range(damage.damageRange.x, damage.damageRange.y);

            if (resistance != null) dmg = dmg * (1f - resistance.Amount);

            if (isCrit)
            {
                dmg *= critMultiplier;
            }

            if (damage.damageApplicationType == DamageApplicationType.PERCENTAGE_MAX_HP)
            {
                dmg *= baseHealth.Amount;
            }
            else if (damage.damageApplicationType == DamageApplicationType.PERCENTAGE_CURRENT_HP)
            {
                dmg *= currentHealth.Amount;
            }
            else if (damage.damageApplicationType == DamageApplicationType.PERCENTAGE_MISSING_HP)
            {
                dmg *= (baseHealth.Amount - currentHealth.Amount);
            }

            if (dmg > 0f)
            {
                currentHealth.Amount = Mathf.Clamp(currentHealth.Amount - dmg, 0f, baseHealth.Amount);

                if (m_owner.Highlighter) m_owner.Highlighter.Highlight();

                if (currentHealth.Amount == 0f) Die();

                m_owner.OnResourceChanged?.Invoke(m_owner, ResourceType.HEALTH);
            }
            else if (dmg < 0f) Heal(dmg, DamageApplicationType.EXACT);

            //Debug.Log($"Damage Debug\n\nIncoming Damage:\n{damage}\n\nTaken:\nCrit: {isCrit}\nDamage Dealt: {dmg}");
        }

        /// <summary>
        /// Increases character's health with given amount. Note that the absolute value of the amount will be used.
        /// </summary>
        /// <param name="amount">The amount of the healing</param>
        /// <param name="applicationType">The application type of the healing effect</param>
        public virtual void Heal(float amount, DamageApplicationType applicationType)
        {
            ResourceContainer baseHealth = baseStats.GetResource(ResourceType.HEALTH);
            ResourceContainer currentHealth = CurrentStats.GetResource(ResourceType.HEALTH);

            if (baseHealth == null || currentHealth == null) return;

            if (applicationType == DamageApplicationType.PERCENTAGE_MAX_HP)
            {
                amount *= baseHealth.Amount;
            }
            else if (applicationType == DamageApplicationType.PERCENTAGE_CURRENT_HP)
            {
                amount *= currentHealth.Amount;
            }
            else if (applicationType == DamageApplicationType.PERCENTAGE_MISSING_HP)
            {
                amount *= (baseHealth.Amount - currentHealth.Amount);
            }

            amount = Mathf.Abs(amount);
            
            currentHealth.Amount = Mathf.Clamp(currentHealth.Amount + amount, 0f, baseHealth.Amount);
            if (currentHealth.Amount == 0f) Die();

            m_owner.OnResourceChanged?.Invoke(m_owner, ResourceType.HEALTH);
        }

        /// <summary>
        /// The tick event of the Stat Systems. Used to update buffs on the character by default.
        /// </summary>
        /// <param name="deltaTime">The delta time between ticks</param>
        public virtual void Tick(float deltaTime)
        {
            foreach (StatusEffect key in buffs.Keys.ToList())
            {
                try
                {
                    buffs[key].ApplyEffect(m_owner, deltaTime);
                }
                catch (KeyNotFoundException)
                {
                    continue;
                }
            }

            float faster = Mathf.Clamp(1f + CurrentStats.moveFastPercent, 1f, 100f);
            float slower = Mathf.Clamp(1f - CurrentStats.moveSlowPercent, 0.2f, 1f);
            CurrentStats.movementSpeed = baseStats.movementSpeed * faster * slower;
            //_animator.SetFloat("MovementSpeedMultiplier", faster * slower);
        }

        public virtual void Die()
        {
            if (m_animator.GetBool("Died")) return;

            ResourceContainer currentHealth = CurrentStats.GetResource(ResourceType.HEALTH);
            if (currentHealth == null) return;

            foreach (StatusEffect key in buffs.Keys.ToList())
            {
                try
                {
                    buffs[key].DurationEnd(m_owner);
                }
                catch (KeyNotFoundException)
                {
                    continue;
                }
            }
            buffs.Clear();

            m_animator.SetBool("Died", true);
            m_animator.SetTrigger("Death");

            m_owner.Die();
        }
    }
}

