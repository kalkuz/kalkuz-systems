using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Projectile Skill/Homing", fileName = "New Homing Skill")]
    public class HomingSkill : ProjectileSkill
    {

        [Tooltip("How fast it rotates towards target.")]
        public float homingStrength;
        public float homingStrengthIncrease;

        public override bool Cast(SkillCaster caster)
        {
            switch (skillDimension)
            {
                case SkillDimension.XY:
                    caster.StartCoroutine(InstantiateFrontsXY(caster));
                    caster.StartCoroutine(InstantiateDiagonalsXY(caster));
                    break;
                case SkillDimension.XZ:
                    caster.StartCoroutine(InstantiateFrontsXZ(caster));
                    caster.StartCoroutine(InstantiateDiagonalsXZ(caster));
                    break;
                default:
                    caster.StartCoroutine(InstantiateFrontsXYZ(caster));
                    caster.StartCoroutine(InstantiateDiagonalsXYZ(caster));
                    break;
            }

            return true;
        }

        public override void InstantiateProjectile(SkillCaster caster, out GameObject cpy_projectile, out ProjectileBehaviour behaviour, out SkillCaster projectileSkillCaster)
        {
            base.InstantiateProjectile(caster, out cpy_projectile, out behaviour, out projectileSkillCaster);

            if (cpy_projectile.TryGetComponent<HomingBehaviour>(out HomingBehaviour hb))
            {
                behaviour = hb;
            }
            else behaviour = cpy_projectile.AddComponent<HomingBehaviour>();
        }
    }
}
