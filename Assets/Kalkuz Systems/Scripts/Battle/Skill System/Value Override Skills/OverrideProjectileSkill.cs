using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Override Projectile Skill", fileName = "New Override Projectile Skill", order = 0)]
    public class OverrideProjectileSkill : OverrideSkill
    {
        public ProjectileSkill overrideSkill;
        [HideInInspector]
        public ProjectileSkill currentDefaultValues;

        UnityEngine.Events.UnityAction onStopCoroutine;

        public override Skill Clone()
        {
            OverrideProjectileSkill clone = Instantiate(this);

            this.buffsToApply = new List<BuffSystem.Buff>(this.buffsToApply);
            this.damages = new DamageList(this.damages);

            this.modificationContainers = new List<SkillPrefabModificationContainer>(this.modificationContainers);

            return clone;
        }

        public override bool IsInRange(SkillCaster caster, Vector3 targetPosition)
        {
            return true;
        }

        public override bool Cast(SkillCaster caster)
        {
            if (caster.skillManager.skills.Count < targetedSkillIndex) return false;

            ProjectileSkill targetSkill = caster.skillManager.skills[targetedSkillIndex].skill as ProjectileSkill;
            currentDefaultValues = Instantiate(targetSkill);

            onStopCoroutine = null;
            foreach (GameObject g in targetSkill.instantiatedBehaviours)
            {
                ProjectileBehaviour behaviour = g.GetComponent<ProjectileBehaviour>();
                Coroutine coroutine = behaviour.StartCoroutine(InterpolationProcedure(behaviour, targetSkill));
                onStopCoroutine += () =>
                {
                    behaviour.StopCoroutine(coroutine);
                    SetCurrentDefaultValues(behaviour);
                    behaviour.StartCoroutine(FadeOut(behaviour, targetSkill, fadeOutTime));
                };
            }

            return true;
        }

        public void StopCoroutine()
        {
            onStopCoroutine?.Invoke();
        }

        void SetCurrentDefaultValues(ProjectileBehaviour behaviour)
        {
            currentDefaultValues.projectileSpeed = behaviour.speed;
            currentDefaultValues.projectileAcceleration = behaviour.acceleration;
            currentDefaultValues.projectileAccelerationForce = behaviour.accelerationForce;

            if (behaviour is OrbitalBehaviour)
            {
                var _behaviour = behaviour as OrbitalBehaviour;
                ((OrbitalSkill)currentDefaultValues).orbitalRadius = _behaviour.orbitalRadius;
            }
        }

        IEnumerator InterpolationProcedure(ProjectileBehaviour behaviour, ProjectileSkill defaultVersion)
        {
            SetCurrentDefaultValues(behaviour);
            yield return FadeIn(behaviour, overrideSkill, fadeInTime);

            SetCurrentDefaultValues(behaviour);
            yield return BetweenFades(behaviour, defaultVersion, delayBetweenFades);

            SetCurrentDefaultValues(behaviour);
            yield return FadeOut(behaviour, defaultVersion, fadeOutTime);
        }

        IEnumerator FadeIn(ProjectileBehaviour behaviour, ProjectileSkill to, float time)
        {
            float timePassed = 0f;

            while (timePassed < time)
            {
                fadeInEvent?.Invoke(behaviour, to, Mathf.InverseLerp(0, time, timePassed));
                yield return null;

                timePassed += Time.deltaTime;
            }
        }
        IEnumerator FadeOut(ProjectileBehaviour behaviour, ProjectileSkill to, float time)
        {
            float timePassed = 0f;

            while (timePassed < time)
            {
                fadeOutEvent?.Invoke(behaviour, to, Mathf.InverseLerp(0, time, timePassed));
                yield return null;

                timePassed += Time.deltaTime;
            }
        }
        IEnumerator BetweenFades(ProjectileBehaviour behaviour, ProjectileSkill to, float time)
        {
            float timePassed = 0f;

            while (timePassed < time)
            {
                betweenFadesEvent?.Invoke(behaviour, to, Mathf.InverseLerp(0, time, timePassed));
                yield return null;

                timePassed += Time.deltaTime;
            }
        }
        #region Interpolation Functions
        public void InterpolateSpeed(ProjectileBehaviour behaviour, ProjectileSkill to, float t)
        {
            behaviour.speed = Mathf.Lerp(currentDefaultValues.projectileSpeed, to.projectileSpeed, t);
        }
        public void InterpolateAcceleration(ProjectileBehaviour behaviour, ProjectileSkill to, float t)
        {
            behaviour.acceleration = Mathf.Lerp(currentDefaultValues.projectileAcceleration, to.projectileAcceleration, t);
        }
        public void InterpolateAccelerationForce(ProjectileBehaviour behaviour, ProjectileSkill to, float t)
        {
            behaviour.accelerationForce = Mathf.Lerp(currentDefaultValues.projectileAccelerationForce, to.projectileAccelerationForce, t);
        }
        public void InterpolateOrbitalRadius(ProjectileBehaviour behaviour, ProjectileSkill to, float t)
        {
            if (behaviour is OrbitalBehaviour)
            {
                var _from = behaviour as OrbitalBehaviour;
                var defaultValues = currentDefaultValues as OrbitalSkill;
                var toValues = to as OrbitalSkill;
                
                _from.orbitalRadius = Mathf.Lerp(defaultValues.orbitalRadius, toValues.orbitalRadius, t);

                var vfx = behaviour.GetComponentInChildren<ParticleSystem>();
                if (vfx)
                {
                    float vfxScale = _from.orbitalRadius > defaultValues.orbitalRadius ? _from.orbitalRadius / defaultValues.orbitalRadius : _from.orbitalRadius / toValues.orbitalRadius;
                    vfx.transform.localScale = Vector3.one * vfxScale;
                }
            }
        }
        #endregion
    }
}
