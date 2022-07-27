using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.BuffSystem
{
    public abstract class Buff : ScriptableObject
    {
        [Tooltip("Every different buff should have different IDs. Make sure to have unique IDs for each buff to get rid of any bugs in future.")]
        public StatusEffect buffID;
        public string buffName;
        [TextArea]
        public string description;
        public Sprite sprite;
        [Tooltip("Determine whether it is a buff or debuff.")]
        public bool isDebuff;
        [Tooltip("How long should the buff last.")]
        public float maxDuration;
        [HideInInspector] public float timePassed;
        [Tooltip("Vfx of buff that will be shown when active on an entity. Leave it empty if the buff has no indicator.")]
        public GameObject vfx;
        [HideInInspector] public GameObject vfxInstantiated;

        //Initiate a buff on a character.
        public virtual void Inflict(CharacterData target, Transform source)
        {
            if (target.Stats.CurrentStats.Immunities.Contains(buffID)) return;
        }
        //Apply buff effects overtime.
        public abstract void ApplyEffect(CharacterData characterData, float deltaTime);
        //Compare and Merge with same kind of Buff with best properties of them.
        public abstract void Merge(Buff other);
        //Clone and return the clone of the buff.
        public abstract Buff Clone();

        public abstract void DurationEnd(CharacterData characterData);
    }
}

