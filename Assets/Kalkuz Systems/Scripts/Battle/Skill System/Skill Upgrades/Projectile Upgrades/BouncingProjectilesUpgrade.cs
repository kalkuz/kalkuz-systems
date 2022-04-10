using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Skill Upgrades/Projectile Upgrades/Bouncing Projectiles Upgrade", fileName = "New Bouncing Projectiles Upgrade", order = 0)]
    public class BouncingProjectilesUpgrade : SkillUpgrade
    {
        public int bounceAmount;

        public override void ApplyUpgrade(UsableSkill skill)
        {
            if (skill is StandardProjectileSkill)
            {
                StandardProjectileSkill s = skill as StandardProjectileSkill;

                s.bounceCount = bounceAmount;
            }
        }

        public override Skill Clone()
        {
            return Instantiate(this);
        }
    }
}
