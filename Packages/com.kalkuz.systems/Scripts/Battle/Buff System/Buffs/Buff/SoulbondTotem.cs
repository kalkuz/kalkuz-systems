using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.BuffSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Buff/Buff/Soulbond Totem", fileName = "New Soulbond Totem", order = 0)]
    public class SoulbondTotem : Soulbond
    {
        public override void ApplyEffect(CharacterData characterData, float deltaTime)
        {
            if (timed)
            {
                timePassed += deltaTime;
                if (timePassed >= maxDuration)
                {
                    characterData.Stats.buffs.Remove(buffID);

                    DurationEnd(characterData);
                }
            }
        }

        public override void Bond(params GameObject[] objects)
        {
            foreach (GameObject o in objects)
            {
                if (currentBonds.Count < capacity) currentBonds.Add(o);
                else
                {
                    GameObject removed = currentBonds[0];
                    currentBonds.RemoveAt(0);
                    Destroy(removed);

                    currentBonds.Add(o);
                }
            }
        }

        public override Buff Clone()
        {
            var clone = Instantiate(this);

            clone.currentBonds = new List<GameObject>(this.currentBonds);

            return clone;
        }

        public override void DurationEnd(CharacterData characterData)
        {
            foreach (GameObject o in currentBonds)
            {
                Destroy(o);
            }

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
            if (other is SoulbondTotem)
            {
                var oth = other as SoulbondTotem;

                maxDuration = Mathf.Max(maxDuration, oth.maxDuration);
                timePassed = Mathf.Clamp(timePassed - oth.maxDuration, 0f, timePassed);

                this.Bond(oth.currentBonds.ToArray());
            }
        }
    }
}
