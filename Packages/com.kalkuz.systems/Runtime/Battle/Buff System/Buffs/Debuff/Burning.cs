using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.BuffSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Buff/Debuff/Burning", fileName = "New Burning", order = 0)]
    public class Burning : Buff
    {
        [Header("Specific Properties")]
        [Tooltip("Damage applied per second while active on an entity.")]
        public Damage damage;

        public override void ApplyEffect(CharacterData characterData, float deltaTime)
        {
            characterData.TakeDamage(damage * deltaTime, 1f, 0f, 0f);

            timePassed += deltaTime;
            if (timePassed >= maxDuration)
            {
                characterData.Stats.buffs.Remove(buffID);

                DurationEnd(characterData);
            }
        }

        public override Buff Clone()
        {
            return Instantiate(this);
        }

        public override void Inflict(CharacterData target, Transform source)
        {
            base.Inflict(target, source);

            if (target == null || target.enabled == false) return;

            timePassed = 0f;

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
            }
        }

        public override void Merge(Buff other)
        {
            if (other is Burning)
            {
                var oth = other as Burning;

                maxDuration = Mathf.Max(maxDuration, oth.maxDuration);
                timePassed = Mathf.Clamp(timePassed - oth.maxDuration, 0f, timePassed);

                damage = Damage.Merge(damage, oth.damage);
            }
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
    }
}
