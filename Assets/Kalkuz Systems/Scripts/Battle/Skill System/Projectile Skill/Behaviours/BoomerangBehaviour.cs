using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class BoomerangBehaviour : ProjectileBehaviour
    {
        BoomerangSkill sourceSkill;
        SkillCaster skillCaster;

        bool returning;
        Transform returnTarget;

        public override void Init(GameObject source, ProjectileSkill skill)
        {
            if (!(skill is BoomerangSkill)) throw new System.Exception($"Skill is not {typeof(BoomerangSkill)}");

            this.enabled = true;

            skillCaster = GetComponent<SkillCaster>();
            skillCaster.Initialize();

            hitAlready = new List<GameObject>();

            this.source = source;
            this.sourceSkill = skill as BoomerangSkill;

            this.dimension = sourceSkill.skillDimension;

            this.speed = sourceSkill.projectileSpeed;
            this.acceleration = sourceSkill.projectileAcceleration;
            this.accelerationForce = sourceSkill.projectileAccelerationForce;

            this.pierce = sourceSkill.pierceCount;

            this.damageScalar = 1f;

            this.returning = false;
            this.returnTarget = source.transform;

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

            acceleration += accelerationForce * Time.deltaTime;
            speed += acceleration * Time.deltaTime;

            if (speed < 0 && !returning)
            {
                returning = true;
                acceleration *= -1;

                hitAlready = new List<GameObject>();
            }

            if (dimension == SkillDimension.XY) Behave2D();
            else Behave3D();
        }

        #region "2D"
        protected override void Behave2D()
        {
            if (returning)
            {
                if (Vector3.Distance(transform.position, source.transform.position) < 0.1f) OnDestroy();

                transform.right = (returnTarget.position - transform.position).normalized;
            }
            else if (Vector3.Distance(transform.position, source.transform.position) > sourceSkill.boomerangRange)
            {
                StartReturning();
            }

            RaycastHit2D hit = Physics2D.CircleCast(transform.position, projectileBehaviourData.collisionRadius, transform.right, speed * Time.deltaTime, sourceSkill.targets);
            if (hit.collider)
            {
                Collider2D other = hit.collider;

                if (((1 << other.gameObject.layer) & projectileBehaviourData.bounceSurfaces) != 0)
                {
                    transform.position = hit.point;
                    StartReturning();
                }
                else if (1 << source.gameObject.layer != 1 << other.gameObject.layer && !hitAlready.Contains(other.gameObject))
                {
                    transform.position = hit.point;
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

                    if (pierce == 0)
                    {
                        if (!returning)
                        {
                            StartReturning();
                        }
                        else
                        {
                            sourceSkill.onPierce?.Invoke(this, sourceSkill.pierceScalars);
                        }
                    }
                    else if (!returning)
                    {
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
                transform.position += transform.right * speed * Time.deltaTime;
            }
        }
        #endregion

        #region "3D"
        protected override void Behave3D()
        {
            if (returning)
            {
                if (Vector3.Distance(transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y), source.transform.position) < 0.1f) OnDestroy();

                Quaternion targetRot = Quaternion.LookRotation(returnTarget.position - transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y), Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f);
            }
            else if (Vector3.Distance(transform.position, source.transform.position) > sourceSkill.boomerangRange)
            {
                StartReturning();
            }

            if (Physics.SphereCast(transform.position, projectileBehaviourData.collisionRadius, transform.forward, out RaycastHit hit, speed * Time.deltaTime, sourceSkill.targets))
            {
                Collider other = hit.collider;

                if (((1 << other.gameObject.layer) & projectileBehaviourData.bounceSurfaces) != 0)
                {
                    transform.position = hit.point;
                    StartReturning();
                }
                else if (1 << source.gameObject.layer != 1 << other.gameObject.layer && !hitAlready.Contains(other.gameObject))
                {
                    transform.position = hit.point;
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

                    if (pierce == 0)
                    {
                        if (!returning)
                        {
                            StartReturning();
                        }
                        else
                        {
                            sourceSkill.onPierce?.Invoke(this, sourceSkill.pierceScalars);
                        }
                    }
                    else if (!returning)
                    {
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
        #endregion

        public void StartReturning()
        {
            returning = true;
            transform.forward *= -1;
            acceleration *= -1;

            hitAlready = new List<GameObject>();
        }

        private void OnDestroy()
        {
            CancelInvoke("OnDestroy");
            projectileBehaviourData.onDestroy?.Invoke();
            skillCaster.OnEnd();
            this.enabled = false;
        }
    }
}
