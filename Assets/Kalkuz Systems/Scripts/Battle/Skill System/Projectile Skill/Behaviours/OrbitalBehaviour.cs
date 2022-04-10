using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class OrbitalBehaviour : ProjectileBehaviour
    {
        OrbitalSkill sourceSkill;
        SkillCaster skillCaster;

        public float orbitalRadius;
        public bool acceleratedRadius;

        Vector3 center;
        bool returning;

        public override void Init(GameObject source, ProjectileSkill skill)
        {
            if (!(skill is OrbitalSkill)) throw new System.Exception($"Skill is not {typeof(OrbitalSkill)}");

            this.enabled = true;

            skillCaster = GetComponent<SkillCaster>();
            skillCaster.Initialize();

            hitAlready = new List<GameObject>();

            this.source = source;
            this.sourceSkill = skill as OrbitalSkill;

            this.dimension = sourceSkill.skillDimension;

            this.speed = sourceSkill.projectileSpeed;
            this.acceleration = sourceSkill.projectileAcceleration;
            this.accelerationForce = sourceSkill.projectileAccelerationForce;

            this.pierce = sourceSkill.pierceCount;

            this.damageScalar = 1f;

            this.orbitalRadius = sourceSkill.orbitalRadius;
            this.acceleratedRadius = sourceSkill.orbitalRadiusAffectedByAcceleration;

            this.center = source.transform.position;

            returning = false;

            this.projectileBehaviourData = GetComponent<ProjectileBehaviourData>();

            if (sourceSkill.projectileLifetime > 0f) Invoke("OnDestroy", sourceSkill.projectileLifetime);

            projectileBehaviourData.onEnable?.Invoke();
        }

        private void LateUpdate()
        {
            if (source == null)
            {
                OnDestroy();
                return;
            }

            acceleration += accelerationForce * Time.deltaTime;
            speed += acceleration * Time.deltaTime;
            orbitalRadius += acceleration * Time.deltaTime;

            if (dimension == SkillDimension.XY) Behave2D();
            else Behave3D();
        }

        #region "2D"
        protected override void Behave2D()
        {
            Vector3 lastPos = transform.position;
            transform.position += (source.transform.position - center);

            if (acceleration < 0 && !returning) returning = true;
            if (returning && Vector3.Distance(transform.position, source.transform.position) < 0.1f) OnDestroy();

            float newAngle = Mathf.Atan2(transform.position.y - center.y, transform.position.x - center.x) + speed * Time.deltaTime * Mathf.Deg2Rad;
            Vector3 nextPos = center + new Vector3(Mathf.Cos(newAngle) * orbitalRadius, Mathf.Sin(newAngle) * orbitalRadius);

            Vector3 look = Vector3.Cross(-Vector3.forward, transform.position - source.transform.position).normalized;
            if (look != Vector3.zero) transform.right = look;

            RaycastHit2D hit = Physics2D.CircleCast(lastPos, projectileBehaviourData.collisionRadius, nextPos - lastPos, Vector3.Distance(nextPos, lastPos), sourceSkill.targets);
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
                    StartCoroutine(RemoveHitObject(other.gameObject, 1f / (speed * Mathf.Deg2Rad)));

                    var charDataHit = other.gameObject.GetComponent<CharacterData>();

                    foreach (BuffSystem.Buff buff in sourceSkill.buffsToApply)
                    {
                        buff.Clone().Inflict(charDataHit, source.transform);
                    }
                    foreach (Damage damage in sourceSkill.damages.List)
                    {
                        charDataHit.TakeDamage(damage, sourceSkill.accuracy, sourceSkill.critChance, sourceSkill.critMultiplier);
                    }

                    if (pierce == 0)
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
            }

            transform.position = nextPos;

            center = source.transform.position;
        }
        #endregion

        #region "3D"
        protected override void Behave3D()
        {
            Vector3 lastPos = transform.position;
            transform.position += (source.transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y) - center);

            if (acceleration < 0 && !returning) returning = true;
            if (returning && Vector3.Distance(transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y), source.transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y)) < 0.1f) OnDestroy();

            float newAngle = Mathf.Atan2(transform.position.z - center.z, transform.position.x - center.x) + speed * Time.deltaTime * Mathf.Deg2Rad;
            Vector3 nextPos = center + new Vector3(Mathf.Cos(newAngle) * orbitalRadius, transform.position.y, Mathf.Sin(newAngle) * orbitalRadius);

            Vector3 look = Vector3.Cross(-Vector3.up, transform.position - source.transform.position).normalized;
            if (look != Vector3.zero) transform.forward = look;

            if (Physics.SphereCast(lastPos, projectileBehaviourData.collisionRadius, nextPos - lastPos, out RaycastHit hit, Vector3.Distance(nextPos, lastPos), sourceSkill.targets))
            {
                Collider other = hit.collider;

                if (((1 << other.gameObject.layer) & projectileBehaviourData.bounceSurfaces) != 0)
                {
                    OnDestroy();
                }
                else if (1 << source.gameObject.layer != 1 << other.gameObject.layer && !hitAlready.Contains(other.gameObject))
                {
                    hitAlready.Add(other.gameObject);
                    StartCoroutine(RemoveHitObject(other.gameObject, 1f / (speed * Mathf.Deg2Rad)));

                    var charDataHit = other.gameObject.GetComponent<CharacterData>();

                    foreach (BuffSystem.Buff buff in sourceSkill.buffsToApply)
                    {
                        buff.Clone().Inflict(charDataHit, source.transform);
                    }
                    foreach (Damage damage in sourceSkill.damages.List)
                    {
                        charDataHit.TakeDamage(damage, sourceSkill.accuracy, sourceSkill.critChance, sourceSkill.critMultiplier);
                    }

                    if (pierce == 0)
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
            }

            transform.position = nextPos;

            center = source.transform.position;
        }
        #endregion

        IEnumerator RemoveHitObject(GameObject gameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            hitAlready.Remove(gameObject);
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