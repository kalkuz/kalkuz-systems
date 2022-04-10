using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class AoeBehaviour2D : MonoBehaviour
    {
        public Collider2D collider2DCast;

        float startTime;

        AoeSkill sourceSkill;

        float radius, sizeMultiplier, effectMultiplier;
        Vector3 initialScale, boxHalfExtends;

        List<Collider2D> colliders;
        Transform source;

        SkillCaster caster;

        public UnityEvent onEnable, onHit, onDestroy;

        public void Init(AoeSkill sourceSkill, Transform owner)
        {
            this.enabled = true;

            caster = GetComponent<SkillCaster>();
            caster.Initialize();

            initialScale = Vector3.one;

            startTime = Time.time;

            this.sourceSkill = sourceSkill;
            this.source = owner;

            sizeMultiplier = sourceSkill.aoeSizeMultiplier;
            effectMultiplier = sourceSkill.aoeEffectMultiplier;

            transform.localScale = initialScale * sizeMultiplier;

            onEnable?.Invoke();

            StartCoroutine(Tick());
        }

        private void Update()
        {
            if (source == null)
            {
                OnDestroy();
                return;
            }

            if (sourceSkill.velocity != Vector3.zero && sourceSkill.maxDistanceFromOwner > 0 && Vector3.Distance(transform.position, source.transform.position) > sourceSkill.maxDistanceFromOwner)
            {
                OnDestroy();
            }

            sizeMultiplier += (sourceSkill.sizeOverTime + sourceSkill.sizeOverDistance * sourceSkill.velocity.magnitude) * Time.deltaTime;
            effectMultiplier += (sourceSkill.effectOverTime + sourceSkill.effectOverDistance * sourceSkill.velocity.magnitude) * Time.deltaTime;

            transform.localScale = initialScale * sizeMultiplier;

            transform.position += transform.rotation * sourceSkill.velocity * Time.deltaTime;
        }

        IEnumerator Tick()
        {
            if (sourceSkill.castType == AoeSkill.AOECastType.ON_START) HitOverlappingEntities();
            if (sourceSkill.castType == AoeSkill.AOECastType.OVER_TIME)
            {
                while (Time.time - startTime < sourceSkill.lifetime)
                {
                    HitOverlappingEntities();
                    yield return new WaitForSeconds(1f / sourceSkill.triggerPerSecond);
                }
            }
            else yield return new WaitForSeconds(sourceSkill.lifetime);

            OnDestroy();
        }

        void HitOverlappingEntities()
        {
            if (colliders == null) colliders = new List<Collider2D>();

            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.useLayerMask = true;
            contactFilter.layerMask = sourceSkill.targets;

            Physics2D.OverlapCollider(collider2DCast, contactFilter, colliders);

            foreach (Collider2D col in colliders)
            {
                if (col.TryGetComponent<CharacterData>(out CharacterData cData))
                {
                    sourceSkill.ApplyEffect(source, cData, effectMultiplier);
                    onHit?.Invoke();
                    caster.OnHit();
                }
            }
        }

        private void OnDestroy()
        {
            onDestroy?.Invoke();
            caster.OnEnd();
            if (sourceSkill.castType == AoeSkill.AOECastType.ON_END)
            {
                HitOverlappingEntities();
            }

            this.enabled = false;
        }
    }
}