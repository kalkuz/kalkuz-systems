using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class HomingBehaviour : ProjectileBehaviour
    {
        HomingSkill sourceSkill;
        SkillCaster skillCaster;

        public float homingStrength;
        public float homingStrengthIncrease;
        Transform homingTarget;

        public override void Init(GameObject source, ProjectileSkill skill)
        {
            if (!(skill is HomingSkill)) throw new System.Exception($"Skill is not {typeof(HomingSkill)}");

            this.enabled = true;

            skillCaster = GetComponent<SkillCaster>();
            skillCaster.Initialize();

            hitAlready = new List<GameObject>();

            this.source = source;
            this.sourceSkill = skill as HomingSkill;

            this.dimension = sourceSkill.skillDimension;

            this.speed = sourceSkill.projectileSpeed;
            this.acceleration = sourceSkill.projectileAcceleration;
            this.accelerationForce = sourceSkill.projectileAccelerationForce;

            this.pierce = sourceSkill.pierceCount;

            this.damageScalar = 1f;

            this.homingStrength = sourceSkill.homingStrength;
            this.homingStrengthIncrease = sourceSkill.homingStrengthIncrease;

            this.projectileBehaviourData = GetComponent<ProjectileBehaviourData>();

            if (sourceSkill.projectileLifetime > 0f) Invoke("OnDestroy", sourceSkill.projectileLifetime);

            projectileBehaviourData.onEnable?.Invoke();
        }

        private void Update()
        {
            if (source == null)
            {
                OnDestroy();
                return;
            }

            homingStrength += homingStrengthIncrease * Time.deltaTime;

            acceleration += accelerationForce * Time.deltaTime;
            speed += acceleration * Time.deltaTime;

            if (sourceSkill.maxDistanceFromOwner > 0 && Vector3.Distance(transform.position, source.transform.position) > sourceSkill.maxDistanceFromOwner)
            {
                OnDestroy();
            }

            if (dimension == SkillDimension.XY) Behave2D();
            else Behave3D();
        }

        #region "2D"
        protected override void Behave2D()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, projectileBehaviourData.collisionRadius, transform.right, speed * Time.deltaTime, sourceSkill.targets);
            if (hit.collider)
            {
                Collider2D other = hit.collider;

                if (((1 << other.gameObject.layer) & projectileBehaviourData.bounceSurfaces) != 0)
                {
                    OnDestroy();
                }
                else if (1 << source.gameObject.layer != 1 << other.gameObject.layer && !hitAlready.Contains(other.gameObject))
                {
                    hitAlready.Add(other.gameObject);

                    var charDataHit = other.gameObject.GetComponent<CharacterData>();

                    foreach (BuffSystem.Buff buff in sourceSkill.buffsToApply)
                    {
                        buff.Clone().Inflict(charDataHit, source.transform);
                    }
                    foreach (Damage damage in sourceSkill.damages.List)
                    {
                        charDataHit.TakeDamage(damage, sourceSkill.accuracy, sourceSkill.critChance, sourceSkill.critMultiplier);
                    }

                    skillCaster.OnHit();
                    projectileBehaviourData.onHit?.Invoke();
                    InstantiateParticles();

                    OnDestroy();
                }
            }

            if (homingTarget)
            {
                if (!homingTarget.GetComponent<CharacterData>())
                {
                    homingTarget = null;
                }
                else if (homingTarget.position != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, homingTarget.position - transform.position, Vector3.forward));
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, homingStrength * Time.deltaTime);
                }
            }
            else HomeOntoNewEnemy2D(sourceSkill.targets);

            transform.position += transform.right * speed * Time.deltaTime;
        }
        void HomeOntoNewEnemy2D(LayerMask otherLayer)
        {
            Vector3 newTarget = Vector3.zero;
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 20f, otherLayer);
            if (cols.Length == 0) return;

            Vector3 bestTarget = transform.right * 1000f;

            foreach (Collider2D col in cols)
            {
                if (hitAlready.Contains(col.gameObject)) continue;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, col.transform.position - transform.position, Vector3.Distance(transform.position, col.transform.position),
                    projectileBehaviourData.bounceSurfaces);
                if (!hit.collider)
                {
                    newTarget = col.transform.position - transform.position;
                    if (newTarget.sqrMagnitude < bestTarget.sqrMagnitude)
                    {
                        bestTarget = newTarget;
                        homingTarget = col.transform;
                    }
                }
            }
        }
        #endregion
        #region "3D"
        protected override void Behave3D()
        {
            if (Physics.SphereCast(transform.position, projectileBehaviourData.collisionRadius, transform.forward, out RaycastHit hit, speed * Time.deltaTime, sourceSkill.targets))
            {
                Collider other = hit.collider;

                if (((1 << other.gameObject.layer) & projectileBehaviourData.bounceSurfaces) != 0)
                {
                    OnDestroy();
                }
                else if (1 << source.gameObject.layer != 1 << other.gameObject.layer && !hitAlready.Contains(other.gameObject))
                {
                    hitAlready.Add(other.gameObject);

                    var charDataHit = other.gameObject.GetComponent<CharacterData>();

                    foreach (BuffSystem.Buff buff in sourceSkill.buffsToApply)
                    {
                        buff.Clone().Inflict(charDataHit, source.transform);
                    }
                    foreach (Damage damage in sourceSkill.damages.List)
                    {
                        charDataHit.TakeDamage(damage, sourceSkill.accuracy, sourceSkill.critChance, sourceSkill.critMultiplier);
                    }

                    skillCaster.OnHit();
                    projectileBehaviourData.onHit?.Invoke();
                    InstantiateParticles();

                    OnDestroy();
                }
            }

            if (homingTarget)
            {
                if (!homingTarget.GetComponent<CharacterData>())
                {
                    homingTarget = null;
                }
                else if (homingTarget.position != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(homingTarget.position - transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y), Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, homingStrength);
                }
            }
            else HomeOntoNewEnemy3D(sourceSkill.targets);

            transform.position += transform.forward * speed * Time.deltaTime;
        }
        void HomeOntoNewEnemy3D(LayerMask otherLayer)
        {
            Vector3 newTarget = Vector3.zero;
            Collider[] cols = Physics.OverlapSphere(transform.position, 20f, otherLayer);
            if (cols.Length == 0) return;

            Vector3 bestTarget = transform.forward * 1000f;

            foreach (Collider col in cols)
            {
                if (hitAlready.Contains(col.gameObject)) continue;

                if (!Physics.Raycast(transform.position, col.transform.position - transform.position, Vector3.Distance(transform.position, col.transform.position),
                    projectileBehaviourData.bounceSurfaces))
                {
                    newTarget = col.transform.position - transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y);
                    if (newTarget.sqrMagnitude < bestTarget.sqrMagnitude)
                    {
                        bestTarget = newTarget;
                        homingTarget = col.transform;
                    }
                }
            }
        }
        #endregion


        private void OnDestroy()
        {
            CancelInvoke("OnDestroy");
            projectileBehaviourData.onDestroy?.Invoke();
            skillCaster.OnEnd();
            this.enabled = false;
        }
    }
}
