using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class DefaultProjectileBehaviour : ProjectileBehaviour
    {
        StandardProjectileSkill sourceSkill;
        SkillCaster skillCaster;
        int bounce, ricochet;

        public override void Init(GameObject source, ProjectileSkill skill)
        {
            if (!(skill is StandardProjectileSkill)) throw new System.Exception($"Skill is not {typeof(StandardProjectileSkill)}");

            this.enabled = true;

            skillCaster = GetComponent<SkillCaster>();
            skillCaster.Initialize();

            hitAlready = new List<GameObject>();

            this.source = source;
            this.sourceSkill = skill as StandardProjectileSkill;

            this.dimension = sourceSkill.skillDimension;

            this.speed = sourceSkill.projectileSpeed;
            this.acceleration = sourceSkill.projectileAcceleration;
            this.accelerationForce = sourceSkill.projectileAccelerationForce;

            this.pierce = sourceSkill.pierceCount;
            this.bounce = sourceSkill.bounceCount;
            this.ricochet = sourceSkill.ricochetCount;

            this.damageScalar = 1f;

            this.projectileBehaviourData = GetComponent<ProjectileBehaviourData>();

            if (sourceSkill.projectileLifetime > 0f) Invoke("OnDestroy", sourceSkill.projectileLifetime);

            projectileBehaviourData.onEnable?.Invoke();
        }

        void Update()
        {
            if (source == null)
            {
                OnDestroy();
                return;
            }

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
                    transform.position = hit.point;
                    if (bounce == 0)
                    {
                        OnDestroy();
                    }
                    else
                    {
                        transform.right = Vector3.Reflect(transform.right, hit.normal.normalized);
                        bounce--;
                        sourceSkill.onBounce?.Invoke(this, sourceSkill.bounceScalars);
                    }
                }
                else if (1 << source.gameObject.layer != 1 << other.gameObject.layer && !hitAlready.Contains(other.gameObject))
                {
                    transform.position = hit.point;
                    hitAlready.Add(other.gameObject);

                    CharacterData charDataHit = other.gameObject.GetComponent<CharacterData>();

                    foreach (BuffSystem.Buff buff in sourceSkill.buffsToApply)
                    {
                        buff.Clone().Inflict(charDataHit, source.transform);
                    }
                    foreach (Damage damage in sourceSkill.damages.List)
                    {
                        charDataHit.TakeDamage(damage, sourceSkill.accuracy, sourceSkill.critChance, sourceSkill.critMultiplier);
                    }

                    if (ricochet != 0)
                    {
                        skillCaster.OnHit();
                        projectileBehaviourData.onHit?.Invoke();
                        InstantiateParticles();

                        RicochetAnotherEnemy3D(1 << other.gameObject.layer);
                        ricochet--;
                        sourceSkill.onRicochet?.Invoke(this, sourceSkill.ricochetScalars);
                    }
                    else if (pierce == 0)
                    {
                        skillCaster.OnHit();
                        projectileBehaviourData.onHit?.Invoke();
                        InstantiateParticles();
                        OnDestroy();
                    }
                    else
                    {
                        skillCaster.OnHit();
                        projectileBehaviourData.onHit?.Invoke();
                        InstantiateParticles();
                        pierce--;
                        sourceSkill.onPierce?.Invoke(this, sourceSkill.pierceScalars);
                    }
                }
                else
                {
                    transform.position += transform.right * speed * Time.deltaTime;
                }
            }
            else
            {
                transform.position += transform.right * speed * Time.deltaTime;
            }
        }
        void RicochetAnotherEnemy2D(LayerMask otherLayer)
        {
            Vector3 newTarget = Vector3.zero;
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 20f, otherLayer);
            if (cols.Length == 0) OnDestroy();

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
                    }
                }
            }

            if (bestTarget == transform.right * 1000f) OnDestroy();

            if (newTarget != Vector3.zero)
            {
                transform.right = bestTarget - transform.position;
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
                    transform.position = hit.point;
                    if (bounce == 0)
                    {
                        OnDestroy();
                    }
                    else
                    {
                        transform.rotation = Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal.normalized), Vector3.up);
                        bounce--;
                        sourceSkill.onBounce?.Invoke(this, sourceSkill.bounceScalars);
                    }
                }
                else if (1 << source.gameObject.layer != 1 << other.gameObject.layer && !hitAlready.Contains(other.gameObject))
                {
                    transform.position = hit.point;
                    hitAlready.Add(other.gameObject);

                    var charDataHit = other.gameObject.GetComponent<CharacterData>();

                    foreach (BuffSystem.Buff buff in sourceSkill.buffsToApply)
                    {
                        buff.Clone().Inflict(charDataHit, skillCaster.transform);
                    }
                    foreach (Damage damage in sourceSkill.damages.List)
                    {
                        charDataHit.TakeDamage(damage, sourceSkill.accuracy, sourceSkill.critChance, sourceSkill.critMultiplier);
                    }

                    if (ricochet != 0)
                    {
                        skillCaster.OnHit();
                        projectileBehaviourData.onHit?.Invoke();
                        InstantiateParticles();

                        RicochetAnotherEnemy3D(1 << other.gameObject.layer);
                        ricochet--;
                        sourceSkill.onRicochet?.Invoke(this, sourceSkill.ricochetScalars);
                    }
                    else if (pierce == 0)
                    {
                        skillCaster.OnHit();
                        projectileBehaviourData.onHit?.Invoke();
                        InstantiateParticles();

                        OnDestroy();
                    }
                    else
                    {
                        skillCaster.OnHit();
                        projectileBehaviourData.onHit?.Invoke();
                        InstantiateParticles();
                        pierce--;
                        sourceSkill.onPierce?.Invoke(this, sourceSkill.pierceScalars);
                    }
                }
                else
                {
                    transform.position += transform.forward * speed * Time.deltaTime;
                }
            }
            else
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
        void RicochetAnotherEnemy3D(LayerMask otherLayer)
        {
            Vector3 newTarget = Vector3.zero;
            Collider[] cols = Physics.OverlapSphere(transform.position, 20f, otherLayer);
            if (cols.Length == 0) OnDestroy();

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
                    }
                }
            }

            if (bestTarget == transform.forward * 1000f) OnDestroy();

            if (newTarget != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(bestTarget, Vector3.up);
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
