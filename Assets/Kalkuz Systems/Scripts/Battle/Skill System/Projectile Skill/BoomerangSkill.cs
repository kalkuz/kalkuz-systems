using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Projectile Skill/Boomerang", fileName = "New Boomerang Skill")]
    public class BoomerangSkill : ProjectileSkill
    {
        [Tooltip("Range of the boomerang")]
        public float boomerangRange;

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

            if (cpy_projectile.TryGetComponent<BoomerangBehaviour>(out BoomerangBehaviour bb))
            {
                behaviour = bb;
            }
            else behaviour = cpy_projectile.AddComponent<BoomerangBehaviour>();
        }
    }
}
