using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Skill Upgrades/Projectile Upgrades/More Front Projectiles Upgrade", fileName = "New More Front Projectiles Upgrade", order = 0)]
    public class MoreFrontProjectilesUpgrade : SkillUpgrade
    {
        public int frontProjectileAddition;
        public float distanceBetweenProjectiles;
        public override void ApplyUpgrade(UsableSkill skill)
        {
            if (skill is ProjectileSkill)
            {
                ProjectileSkill s = skill as ProjectileSkill;

                s.frontProjectileCount = frontProjectileAddition;
                s.projectilePositionOffset = distanceBetweenProjectiles;
            }
        }

        public override Skill Clone()
        {
            return Instantiate(this);
        }
    }
}