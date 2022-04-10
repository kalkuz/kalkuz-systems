using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Skill Upgrades/Projectile Upgrades/Faster Projectiles Upgrade", fileName = "New Faster Projectiles Upgrade", order = 0)]
    public class FasterProjectilesUpgrade : SkillUpgrade
    {
        [Header("Properties")]
        public float percentage;
        public override void ApplyUpgrade(UsableSkill skill)
        {
            if (skill is ProjectileSkill)
            {
                ProjectileSkill s = skill as ProjectileSkill;

                s.projectileSpeed *= (1 + percentage / 100f);
            }
        }

        public override Skill Clone()
        {
            return Instantiate(this);
        }
    }
}
