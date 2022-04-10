using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Projectile Skill/Orbital", fileName = "New Orbital Skill")]
    public class OrbitalSkill : ProjectileSkill
    {
        [Tooltip("Radius of the orbital")]
        public float orbitalRadius;
        [Tooltip("Acceleration also affects orbital radius")]
        public bool orbitalRadiusAffectedByAcceleration;

        public override bool Cast(SkillCaster caster)
        {
            if (destroyBeforeInstantiatingNew)
            {
                foreach (GameObject g in instantiatedBehaviours)
                {
                    Destroy(g);
                }
                instantiatedBehaviours.Clear();
            }

            switch (skillDimension)
            {
                case SkillDimension.XY:
                    caster.StartCoroutine(InstantiateOrbitalsXY(caster));
                    break;
                case SkillDimension.XZ:
                    caster.StartCoroutine(InstantiateOrbitalsXZ(caster));
                    break;
                default:
                    caster.StartCoroutine(InstantiateOrbitalsXYZ(caster));
                    break;
            }

            return true;
        }

        #region "Orbitals"
        IEnumerator InstantiateOrbitalsXY(SkillCaster caster)
        {
            for (int i = 0; i < frontProjectileCount; i++)
            {
                float angle = Mathf.Lerp(0f, 2 * Mathf.PI, Mathf.InverseLerp(0, frontProjectileCount, i));
                Vector3 pos = caster.transform.position + new Vector3(Mathf.Cos(angle) * orbitalRadius, Mathf.Sin(angle) * orbitalRadius);

                InstantiateProjectile(caster, out GameObject cpy_projectile, out ProjectileBehaviour behaviour, out SkillCaster projectileSkillCaster);
                cpy_projectile.transform.position = pos;

                Vector3 look = Vector3.Cross(Vector3.forward, cpy_projectile.transform.position - caster.transform.position).normalized;
                if (look != Vector3.zero) cpy_projectile.transform.right = look;

                projectileSkillCaster.skillManager = skillManager;

                behaviour.Init(caster.gameObject, this);

                if (isToggled)
                {
                    projectileSkillCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
                }

                if (!instantiatedBehaviours.Contains(cpy_projectile)) instantiatedBehaviours.Add(cpy_projectile);

                if (sequential && i != frontProjectileCount - 1) yield return new WaitForSeconds(timeBetweenEachSequence);
            }

            yield return null;
        }
        IEnumerator InstantiateOrbitalsXZ(SkillCaster caster)
        {
            for (int i = 0; i < frontProjectileCount; i++)
            {
                float angle = Mathf.Lerp(0f, 2 * Mathf.PI, Mathf.InverseLerp(0, frontProjectileCount, i));
                Vector3 pos = caster.transform.position + new Vector3(Mathf.Cos(angle) * orbitalRadius, caster.projectileOut.position.y, Mathf.Sin(angle) * orbitalRadius);

                InstantiateProjectile(caster, out GameObject cpy_projectile, out ProjectileBehaviour behaviour, out SkillCaster projectileSkillCaster);
                cpy_projectile.transform.position = pos;

                Vector3 look = Vector3.Cross(Vector3.up, cpy_projectile.transform.position - caster.transform.position).normalized;
                if (look != Vector3.zero) cpy_projectile.transform.forward = look;

                projectileSkillCaster.skillManager = skillManager;

                behaviour.Init(caster.gameObject, this);

                if (isToggled)
                {
                    projectileSkillCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
                }

                if (!instantiatedBehaviours.Contains(cpy_projectile)) instantiatedBehaviours.Add(cpy_projectile);

                if (sequential && i != frontProjectileCount - 1) yield return new WaitForSeconds(timeBetweenEachSequence);
            }

            yield return null;
        }
        IEnumerator InstantiateOrbitalsXYZ(SkillCaster caster)
        {
            yield return InstantiateOrbitalsXZ(caster);
        }
        #endregion

        public override void InstantiateProjectile(SkillCaster caster, out GameObject cpy_projectile, out ProjectileBehaviour behaviour, out SkillCaster projectileSkillCaster)
        {
            base.InstantiateProjectile(caster, out cpy_projectile, out behaviour, out projectileSkillCaster);

            if (cpy_projectile.TryGetComponent<OrbitalBehaviour>(out OrbitalBehaviour ob))
            {
                behaviour = ob;
            }
            else behaviour = cpy_projectile.AddComponent<OrbitalBehaviour>();
        }
    }
}
