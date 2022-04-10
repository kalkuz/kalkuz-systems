using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Skill Upgrades/Damage Upgrades/Fire Upgrade", fileName = "New Fire Upgrade", order = 0)]
    public class FireUpgrade : SkillUpgrade
    {
        [Header("Properties")]
        public BuffSystem.Buff buff;
        [Range(0f, 10f)] public float damageConversionPercentage;

        public override void ApplyUpgrade(UsableSkill skill)
        {
            if (buff) skill.buffsToApply.Add(buff);

            foreach (Damage d in skill.damages.List.ToArray())
            {
                if (d.damageType == DamageType.NONE)
                {
                    skill.damages.Add(Damage.Convert(d, DamageType.FIRE, d.damageApplicationType, damageConversionPercentage));

                    if (damageConversionPercentage < 1f) skill.damages.UpdateDamageRange(d, d.damageRange * (1f - damageConversionPercentage));
                    else skill.damages.Remove(d);
                }
            }

            foreach (SkillPrefabModificationContainer spmc in skill.modificationContainers)
            {
                if (spmc.upgradeType is FireUpgrade)
                {
                    skill.defaultSkillPrefab = spmc.prefabReplacement;
                    skill.sprite = spmc.spriteReplacement;

                    break;
                }
            }

            foreach (SkillContainer sc in skill.skillManager.skills)
            {
                if (sc.inheritDamageTypeUpgrade)
                {
                    sc.attachments.Add(this);
                }
            }
        }

        public override Skill Clone()
        {
            return Instantiate(this);
        }
    }
}
