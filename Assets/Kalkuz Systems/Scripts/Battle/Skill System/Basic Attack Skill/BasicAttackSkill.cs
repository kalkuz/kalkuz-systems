using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Basic Attack Skill", fileName = "New Basic Attack Skill", order = 0)]
    public class BasicAttackSkill : UsableSkill
    {
        [Header("Properties")]
        [Tooltip("True if it is a projectile-like attack, false if melee or instant attack")]
        public bool isRanged;
        [Header("Ranged Props")]
        public float speed;
        public float acceleration;

        public override bool Cast(SkillCaster caster)
        {
            if (targetCharacter == null) return false;

            if (isRanged)
            {
                Vector3 pos = caster.projectileOut.position;
                GameObject attack = Instantiate(defaultSkillPrefab, pos, defaultSkillPrefab.transform.rotation);
                BasicAttackRangedBehaviour behaviour = attack.AddComponent<BasicAttackRangedBehaviour>();

                float rot = caster.projectileOut.rotation.eulerAngles.y;
                attack.transform.rotation = Quaternion.Euler(0f, rot, 0f);

                attack.AddComponent<SkillCaster>().skillManager = skillManager;
                behaviour.Init(this, caster);

                return true;
            }
            else
            {
                CharacterData targetData = targetCharacter.GetComponent<CharacterData>();
                foreach (BuffSystem.Buff b in buffsToApply)
                {
                    b.Clone().Inflict(targetData, caster.transform);
                }

                foreach (Damage d in damages.List)
                {
                    if (((1 << targetCharacter.gameObject.layer) & targets) != 0)
                    {
                        targetData.TakeDamage(d, accuracy, critChance, critMultiplier);
                    }
                    else
                    {
                        targetData.Stats.Heal(d.damageRange.x, d.damageApplicationType); //heal
                    }
                }

                return true;
            }
        }

        public override Skill Clone()
        {
            BasicAttackSkill clone = Instantiate(this);

            clone.buffsToApply = new List<BuffSystem.Buff>(buffsToApply);
            clone.damages = new DamageList(this.damages);

            clone.modificationContainers = new List<SkillPrefabModificationContainer>(modificationContainers);

            return clone;
        }

        public override bool IsInRange(SkillCaster caster, Vector3 targetPosition)
        {
            if (targetCharacter == null) return false;

            if (range <= 0f) return true;

            return Vector3.Distance(caster.transform.position, targetCharacter.transform.position) < range;
        }
    }
}
