using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Projectile Skill/Standard", fileName = "New Projectile Skill")]
    public class StandardProjectileSkill : ProjectileSkill
    {
        [Space, Tooltip("How many times the projectile can ricochet between close targets.")]
        public int ricochetCount;
        public ProjectileBehaviourScalars ricochetScalars;
        [Space, Tooltip("How many times the projectile can bounce off the surfaces before it gets destroyed.")]
        public int bounceCount;
        public ProjectileBehaviourScalars bounceScalars;

        public UnityEvent<ProjectileBehaviour, ProjectileBehaviourScalars> onBounce, onRicochet;

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

            if (cpy_projectile.TryGetComponent<DefaultProjectileBehaviour>(out DefaultProjectileBehaviour db))
            {
                behaviour = db;
            }
            else behaviour = cpy_projectile.AddComponent<DefaultProjectileBehaviour>();
        }
    }
}
