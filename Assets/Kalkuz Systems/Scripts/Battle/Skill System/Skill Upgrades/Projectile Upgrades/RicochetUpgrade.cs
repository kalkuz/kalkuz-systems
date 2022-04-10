using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Skill Upgrades/Projectile Upgrades/Ricochet Upgrade", fileName = "New Ricochet Upgrade", order = 0)]
    public class RicochetUpgrade : SkillUpgrade
    {
        public int ricochetAmount;

        public override void ApplyUpgrade(UsableSkill skill)
        {
            if (skill is StandardProjectileSkill)
            {
                StandardProjectileSkill s = skill as StandardProjectileSkill;

                s.ricochetCount = ricochetAmount;
            }
        }

        public override Skill Clone()
        {
            return Instantiate(this);
        }
    }
}
