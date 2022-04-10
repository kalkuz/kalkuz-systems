using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class AoeBehaviour : MonoBehaviour
    {
        public bool isSpherical;
        public float initialRadius;
        public Vector3 initialBoxHalfExtends;

        float startTime;

        AoeSkill sourceSkill;

        float radius, sizeMultiplier, effectMultiplier;
        Vector3 initialScale, boxHalfExtends;

        Collider[] colliders;
        Transform source;

        SkillCaster caster;

        public UnityEvent onEnable, onHit, onDestroy;

        public void Init(AoeSkill sourceSkill, Transform source)
        {
            this.enabled = true;

            caster = GetComponent<SkillCaster>();
            caster.Initialize();

            initialScale = Vector3.one;

            startTime = Time.time;

            this.sourceSkill = sourceSkill;
            this.source = source;

            sizeMultiplier = sourceSkill.aoeSizeMultiplier;
            effectMultiplier = sourceSkill.aoeEffectMultiplier;

            radius = initialRadius * sizeMultiplier;
            boxHalfExtends = initialBoxHalfExtends * effectMultiplier;

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

            radius = initialRadius * sizeMultiplier;
            boxHalfExtends = initialBoxHalfExtends * sizeMultiplier;

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
            if (isSpherical) colliders = Physics.OverlapSphere(transform.position, radius, sourceSkill.targets);
            else colliders = Physics.OverlapBox(transform.position, boxHalfExtends, transform.rotation, sourceSkill.targets);

            List<CharacterData> hitBefore = new List<CharacterData>();
            foreach (Collider col in colliders)
            {
                if (col.TryGetComponent<CharacterData>(out CharacterData cData))
                {
                    if (!hitBefore.Contains(cData))
                    {
                        hitBefore.Add(cData);
                        sourceSkill.ApplyEffect(source, cData, effectMultiplier);
                        onHit?.Invoke();
                        caster.OnHit();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            onDestroy?.Invoke();
            caster.OnEnd();
            if (this.enabled && sourceSkill.castType == AoeSkill.AOECastType.ON_END)
            {
                HitOverlappingEntities();
            }

            this.enabled = false;
        }
    }
}