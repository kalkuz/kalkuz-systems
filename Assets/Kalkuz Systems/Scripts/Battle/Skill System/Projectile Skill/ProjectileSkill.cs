using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.DataStructures.Pooling;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.SkillSystem
{
    public abstract class ProjectileSkill : UsableSkill
    {
        [Header("Properties"), Tooltip("How fast the projectile should move.")]
        public float projectileSpeed;
        [Tooltip("Acceleration of projectile")]
        public float projectileAcceleration;
        [Tooltip("Acceleration increase")]
        public float projectileAccelerationForce;

        [Space, Tooltip("How many projectiles the skill will cast towards forward. If orbital, it specifies the orbital projectiles.")]
        public int frontProjectileCount;
        [Tooltip("How far the front projectiles will be placed while spawning.")]
        public float projectilePositionOffset;

        [Space, Tooltip("How many projectiles the skill will spread homogeneously.")]
        public int diagonalProjectileCount;
        [Tooltip("The parameter that determines the spread amount of the skill i.e. 360 for casting in every direction.")]
        public float maxProjectileSpreadAngle;

        [Space]
        public bool sequential;
        public float timeBetweenEachSequence;

        [Space, Tooltip("Maximum distance it gets far from its source")]
        public float maxDistanceFromOwner;
        [Tooltip("Projectile will be destroyed after this.")]
        public float projectileLifetime;

        [Space, Tooltip("How many entities the projectile can pierce before it gets destroyed.")]
        public int pierceCount;
        public ProjectileBehaviourScalars pierceScalars;
        public UnityEvent<ProjectileBehaviour, ProjectileBehaviourScalars> onPierce;

        public override Skill Clone()
        {
            ProjectileSkill clone = Instantiate(this);

            clone.buffsToApply = new List<BuffSystem.Buff>(this.buffsToApply);

            clone.damages = new DamageList(this.damages);

            clone.modificationContainers = new List<SkillPrefabModificationContainer>(this.modificationContainers);

            return clone;
        }

        #region "Fronts"
        protected IEnumerator InstantiateFrontsXY(SkillCaster caster)
        {
            for (int i = 0; i < frontProjectileCount; i++)
            {
                InstantiateProjectile(caster, out GameObject cpy_projectile, out ProjectileBehaviour behaviour, out SkillCaster projectileSkillCaster);

                cpy_projectile.transform.right = caster.projectileOut.right;

                cpy_projectile.transform.position += cpy_projectile.transform.up * (projectilePositionOffset * i - (projectilePositionOffset * (frontProjectileCount - 1) / 2));

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
        protected IEnumerator InstantiateFrontsXZ(SkillCaster caster)
        {
            for (int i = 0; i < frontProjectileCount; i++)
            {
                InstantiateProjectile(caster, out GameObject cpy_projectile, out ProjectileBehaviour behaviour, out SkillCaster projectileSkillCaster);

                cpy_projectile.transform.forward = caster.projectileOut.forward;

                cpy_projectile.transform.position += cpy_projectile.transform.right * (projectilePositionOffset * i - (projectilePositionOffset * (frontProjectileCount - 1) / 2));

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
        protected IEnumerator InstantiateFrontsXYZ(SkillCaster caster)
        {
            yield return InstantiateFrontsXZ(caster);
        }
        #endregion
        #region "Diagonals"
        protected IEnumerator InstantiateDiagonalsXY(SkillCaster caster)
        {
            for (int i = 0; i < diagonalProjectileCount; i++)
            {
                InstantiateProjectile(caster, out GameObject cpy_projectile, out ProjectileBehaviour behaviour, out SkillCaster projectileSkillCaster);

                float rot = -maxProjectileSpreadAngle * 0.5f + i * maxProjectileSpreadAngle / (diagonalProjectileCount - 1) + caster.projectileOut.eulerAngles.z;
                cpy_projectile.transform.rotation = Quaternion.Euler(0f, 0f, rot);

                projectileSkillCaster.skillManager = skillManager;

                behaviour.Init(caster.gameObject, this);

                if (isToggled)
                {
                    projectileSkillCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
                }

                if (!instantiatedBehaviours.Contains(cpy_projectile)) instantiatedBehaviours.Add(cpy_projectile);

                if (sequential && i != diagonalProjectileCount - 1) yield return new WaitForSeconds(timeBetweenEachSequence);
            }

            yield return null;
        }
        protected IEnumerator InstantiateDiagonalsXZ(SkillCaster caster)
        {
            for (int i = 0; i < diagonalProjectileCount; i++)
            {
                InstantiateProjectile(caster, out GameObject cpy_projectile, out ProjectileBehaviour behaviour, out SkillCaster projectileSkillCaster);

                float rot = -maxProjectileSpreadAngle * 0.5f + i * maxProjectileSpreadAngle / (diagonalProjectileCount - 1) + caster.projectileOut.eulerAngles.y;
                cpy_projectile.transform.rotation = Quaternion.Euler(0f, rot, 0f);

                projectileSkillCaster.skillManager = skillManager;

                behaviour.Init(caster.gameObject, this);

                if (isToggled)
                {
                    projectileSkillCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
                }

                if (!instantiatedBehaviours.Contains(cpy_projectile)) instantiatedBehaviours.Add(cpy_projectile);

                if (sequential && i != diagonalProjectileCount - 1) yield return new WaitForSeconds(timeBetweenEachSequence);
            }

            yield return null;
        }
        protected IEnumerator InstantiateDiagonalsXYZ(SkillCaster caster)
        {
            yield return InstantiateDiagonalsXZ(caster);
        }
        #endregion

        public override bool IsInRange(SkillCaster caster, Vector3 targetPosition)
        {
            return true;
        }

        public virtual void InstantiateProjectile(SkillCaster caster, out GameObject cpy_projectile, out ProjectileBehaviour behaviour, out SkillCaster projectileSkillCaster)
        {
            if (defaultSkillPrefab.TryGetComponent<PoolObject>(out PoolObject po))
            {
                cpy_projectile = UniversalPoolProvider.GetPool(po.ID).Request(po).gameObject;
                cpy_projectile.transform.position = caster.projectileOut.position;
                cpy_projectile.transform.rotation = defaultSkillPrefab.transform.rotation;
            }
            else
            {
                cpy_projectile = Instantiate(defaultSkillPrefab, caster.projectileOut.position, defaultSkillPrefab.transform.rotation);
            }

            if (cpy_projectile.TryGetComponent<SkillCaster>(out SkillCaster _skillCaster))
            {
                projectileSkillCaster = _skillCaster;
            }
            else projectileSkillCaster = cpy_projectile.AddComponent<SkillCaster>();

            behaviour = null;
        }

        #region "Setter Methods"
        #region "Scale"
        public void ScaleBehaviourDamage(ProjectileBehaviour behaviour, ProjectileBehaviourScalars scalar)
        {
            behaviour.damageScalar *= scalar.damageScalar;
        }
        public void ScaleBehaviourSpeed(ProjectileBehaviour behaviour, ProjectileBehaviourScalars scalar)
        {
            behaviour.speed *= scalar.speedScalar;
        }
        public void ScaleBehaviourAcceleration(ProjectileBehaviour behaviour, ProjectileBehaviourScalars scalar)
        {
            behaviour.acceleration *= scalar.accelerationScalar;
        }
        public void ScaleBehaviourAccelerationForce(ProjectileBehaviour behaviour, ProjectileBehaviourScalars scalar)
        {
            behaviour.accelerationForce *= scalar.accelerationForceScalar;
        }
        #endregion
        #region "Add"
        public void AddBehaviourDamage(ProjectileBehaviour behaviour, ProjectileBehaviourScalars scalar)
        {
            behaviour.damageScalar += scalar.damageAddition;
        }
        public void AddBehaviourSpeed(ProjectileBehaviour behaviour, ProjectileBehaviourScalars scalar)
        {
            behaviour.speed += scalar.speedAddition;
        }
        public void AddBehaviourAcceleration(ProjectileBehaviour behaviour, ProjectileBehaviourScalars scalar)
        {
            behaviour.acceleration += scalar.accelerationAddition;
        }
        public void AddBehaviourAccelerationForce(ProjectileBehaviour behaviour, ProjectileBehaviourScalars scalar)
        {
            behaviour.accelerationForce += scalar.accelerationForceAddition;
        }
        #endregion
        #endregion
    }
}