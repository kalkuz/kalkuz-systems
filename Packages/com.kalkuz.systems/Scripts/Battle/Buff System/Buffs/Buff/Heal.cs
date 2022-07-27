using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.BuffSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Buff/Buff/Heal", fileName = "New Heal", order = 0)]
    public class Heal : Buff
    {
        public DamageApplicationType healingType;
        public float healAmount;

        public override void ApplyEffect(CharacterData characterData, float deltaTime)
        {
            timePassed += deltaTime;
            if (timePassed >= maxDuration)
            {
                characterData.Stats.buffs.Remove(buffID);

                DurationEnd(characterData);
            }

            characterData.Stats.Heal(healAmount * deltaTime, healingType);
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
                if ((target.Stats.buffs[buffID] as Heal).healingType == healingType) target.Stats.buffs[buffID].Merge(this);
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
            if (other is Heal)
            {
                var oth = other as Heal;

                maxDuration = Mathf.Max(maxDuration, oth.maxDuration);
                timePassed = Mathf.Clamp(timePassed - oth.maxDuration, 0f, timePassed);

                healAmount = Mathf.Max(healAmount, oth.healAmount);
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
