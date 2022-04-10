using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Powerup Skill", fileName = "New Powerup Skill", order = 0)]
    public class PowerupSkill : UsableSkill
    {
        public override Skill Clone()
        {
            PowerupSkill clone = Instantiate(this);

            clone.buffsToApply = new List<BuffSystem.Buff>(this.buffsToApply);
            clone.damages = new DamageList(this.damages);

            clone.modificationContainers = new List<SkillPrefabModificationContainer>(this.modificationContainers);

            return clone;
        }
        public override bool Cast(SkillCaster caster)
        {
            if (targetCharacter == null)
            {
                if (((1 << caster.gameObject.layer) & targets) != 0)
                {
                    CharacterData cData = caster.GetComponent<CharacterData>();

                    foreach (BuffSystem.Buff buff in buffsToApply)
                    {
                        buff.Clone().Inflict(cData, caster.transform);
                    }
                }
                else return false;
            }
            else
            {
                if (((1 << targetCharacter.gameObject.layer) & targets) != 0)
                {
                    foreach (BuffSystem.Buff buff in buffsToApply)
                    {
                        buff.Clone().Inflict(targetCharacter, caster.transform);
                    }
                }
                else return false;
            }

            return true;
        }

        public override bool IsInRange(SkillCaster caster, Vector3 targetPosition)
        {
            if (range <= 0f) return true;

            return Vector3.Distance(caster.transform.position, targetPosition) < range;
        }
    }
}

