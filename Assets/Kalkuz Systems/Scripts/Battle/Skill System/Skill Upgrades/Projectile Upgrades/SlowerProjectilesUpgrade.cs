using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Skill Upgrades/Projectile Upgrades/Slower Projectiles Upgrade", fileName = "New Slower Projectiles Upgrade", order = 0)]
    public class SlowerProjectilesUpgrade : SkillUpgrade
    {
        [Header("Properties")]
        public float percentage;
        public override void ApplyUpgrade(UsableSkill skill)
        {
            if (skill is ProjectileSkill)
            {
                ProjectileSkill s = skill as ProjectileSkill;

                s.projectileSpeed *= (1 - percentage / 100f);
            }
        }

        public override Skill Clone()
        {
            return Instantiate(this);
        }
    }
}
