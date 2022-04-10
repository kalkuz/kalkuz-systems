using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Skill Upgrades/Projectile Upgrades/Multiple Projectiles Upgrade", fileName = "New Multiple Projectiles Upgrade", order = 0)]
    public class MultipleProjectilesUpgrade : SkillUpgrade
    {
        [Header("Properties")]
        public int addition;
        public float spreadAngle;

        public override void ApplyUpgrade(UsableSkill skill)
        {
            if (skill is ProjectileSkill)
            {
                ProjectileSkill s = skill as ProjectileSkill;

                s.diagonalProjectileCount += addition;
                s.maxProjectileSpreadAngle = spreadAngle;
            }
        }

        public override Skill Clone()
        {
            return Instantiate(this);
        }
    }
}
