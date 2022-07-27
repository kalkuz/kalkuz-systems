using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KalkuzSystems.Battle
{
    [System.Serializable]
    public class Damage
    {
        [Tooltip("A vector that holds the information of the range of the damage. X for minimum damage, Y for maximum damage.")]
        public Vector2 damageRange;
        [Tooltip("Type of the damage.")]
        public DamageType damageType;
        public DamageApplicationType damageApplicationType;

        public Damage() : this(Vector2.zero, DamageType.NONE, DamageApplicationType.EXACT)
        {
        }
        public Damage(Vector2 damageRange, DamageType damageType, DamageApplicationType damageApplicationType)
        {
            this.damageRange = damageRange;
            this.damageType = damageType;
            this.damageApplicationType = damageApplicationType;
        }

        //Merges two damages with same damage type. Returns first if they have different damage types.
        public static Damage Merge(Damage dmg1, Damage dmg2)
        {
            if (dmg1.damageType == dmg2.damageType && dmg1.damageApplicationType == dmg2.damageApplicationType)
            {
                float min = Mathf.Max(dmg1.damageRange.x, dmg2.damageRange.x);
                float max = Mathf.Max(dmg1.damageRange.y, dmg2.damageRange.y);
                return new Damage(new Vector2(min, max), dmg1.damageType, dmg1.damageApplicationType);
            }
            else return dmg1;
        }

        public static Damage Convert(Damage dmg, DamageType convertToDamageType, DamageApplicationType convertToDamageAppType, float conversionPercentage)
        {
            return new Damage(dmg.damageRange * conversionPercentage, convertToDamageType, convertToDamageAppType);
        }

        public static Damage operator *(Damage damage, float multiplier)
        {
            return new Damage(damage.damageRange * multiplier, damage.damageType, damage.damageApplicationType);
        }
        public static Damage operator /(Damage damage, float multiplier)
        {
            if (multiplier == 0f) throw new System.DivideByZeroException();

            return damage * (1f / multiplier);
        }

        public override string ToString()
        {
            return $"Damage Range: {damageRange}\nDamage Type: {damageType}\nApplication Type: {damageApplicationType}";
        }
    }
}

namespace KalkuzSystems.Battle
{
    public enum DamageApplicationType
    {
        EXACT, PERCENTAGE_CURRENT_HP, PERCENTAGE_MAX_HP, PERCENTAGE_MISSING_HP
    }
}

