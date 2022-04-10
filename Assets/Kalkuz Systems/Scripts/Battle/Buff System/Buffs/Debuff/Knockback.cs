using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.BuffSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Buff/Debuff/Knockback", fileName = "New Knockback", order = 0)]
    public class Knockback : Buff
    {
        [Header("Properties")]
        public float knockbackDistance;

        [Space]
        public bool exactDistanceFromSource;
        // public bool applyIfTargetIsInRange;

        Vector3 knockbackVector;
        Vector3 initialPosition, sourcePosition;

        public override void ApplyEffect(CharacterData characterData, float deltaTime)
        {
            return;
        }

        public override Buff Clone()
        {
            return Instantiate(this);
        }

        public override void DurationEnd(CharacterData characterData)
        {
            if (vfxInstantiated)
            {
                if (vfxInstantiated.TryGetComponent<ParticleSystem>(out ParticleSystem ps))
                {
                    ps.Stop();

                    Destroy(vfxInstantiated, 3f);
                }
                else
                {
                    Destroy(vfxInstantiated);
                }
            }
        }

        public override void Inflict(CharacterData target, Transform source)
        {
            base.Inflict(target, source);

            if (target == null || target.enabled == false) return;

            timePassed = 0f;

            initialPosition = target.transform.position;
            sourcePosition = source.position;
            if (source != null)
            {
                if (exactDistanceFromSource)
                {
                    if ((initialPosition - sourcePosition).magnitude > knockbackDistance) return;

                    knockbackVector = (initialPosition - source.position).normalized * knockbackDistance;
                }
                else
                {
                    knockbackVector = (initialPosition - source.position).normalized * knockbackDistance;
                }
            }

            //if buffs already exists then merge them else add this buff
            if (target.Stats.buffs.ContainsKey(buffID))
            {
                target.Stats.buffs[buffID].Merge(this);
            }
            else
            {
                target.Stats.buffs.Add(buffID, this);
                if (vfx)
                {
                    var _parent = target.transform;
                    vfxInstantiated = Instantiate(vfx, vfx.transform.position, Quaternion.identity);
                    vfxInstantiated.transform.SetParent(_parent, false);
                }

                target.StartCoroutine(KnockbackProcedure(target.transform));
            }
        }

        public override void Merge(Buff other)
        {
            if (other is Knockback)
            {
                var oth = other as Knockback;

                maxDuration = oth.maxDuration;
                timePassed = 0f;

                initialPosition = oth.initialPosition;

                knockbackVector = oth.knockbackVector;
            }
        }

        IEnumerator KnockbackProcedure(Transform t)
        {
            Vector3 destination = exactDistanceFromSource ? sourcePosition + knockbackVector : initialPosition + knockbackVector;
            while (timePassed < maxDuration)
            {
                t.position = Vector3.Lerp(initialPosition, destination, timePassed / maxDuration);
                yield return null;
                timePassed += Time.deltaTime;
            }
            t.position = destination;

            if (t.TryGetComponent<CharacterData>(out var cData))
            {
                cData.Stats.buffs.Remove(buffID);

                DurationEnd(cData);
            }
        }
    }
}
