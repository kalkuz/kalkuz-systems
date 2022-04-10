using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Skill Upgrades/Projectile Upgrades/Piercing Projectiles Upgrade", fileName = "New Piercing Projectiles Upgrade", order = 0)]
    public class PiercingProjectilesUpgrade : SkillUpgrade
    {
        public int pierceAmount;

        public override void ApplyUpgrade(UsableSkill skill)
        {
            if (skill is ProjectileSkill)
            {
                ProjectileSkill s = skill as ProjectileSkill;

                s.pierceCount = pierceAmount;
            }
        }

        public override Skill Clone()
        {
            return Instantiate(this);
        }
    }
}

