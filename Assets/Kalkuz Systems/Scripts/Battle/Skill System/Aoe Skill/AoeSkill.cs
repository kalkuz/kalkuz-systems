using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle.BuffSystem;
using KalkuzSystems.DataStructures.Pooling;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Area of Effect Skill", fileName = "New Area of Effect Skill", order = 0)]
    public class AoeSkill : UsableSkill
    {
        [Header("Properties")]
        [Tooltip("Makes the skill be child of skill caster. Do not work with any velocity.")]
        public bool isChildOfCaster;
        public Vector3 velocity;
        [Space]
        [Tooltip("Effective range of skill. This will scale the prefab on cast.")]
        public float aoeSizeMultiplier;
        public float sizeOverTime;
        public float sizeOverDistance;
        [Space]
        [Tooltip("How strong the skill's effect/damage will be.")]
        public float aoeEffectMultiplier;
        public float effectOverTime;
        public float effectOverDistance;
        [Space]
        [Tooltip("Type when the effect is applied")]
        public AOECastType castType;
        [Tooltip("How long it will stay on the map.")]
        public float lifetime;
        [Tooltip("Maximum distance it gets far from its source")]
        public float maxDistanceFromOwner;
        [Tooltip("Trigger rate")]
        public float triggerPerSecond;

        // input => characterdata, work principle => apply overtime effects on character.
        public void ApplyEffect(Transform source, CharacterData target, float effectMultiplier)
        {
            foreach (Buff buff in buffsToApply)
            {
                buff.Clone().Inflict(target, source);
            }
            foreach (Damage damage in damages.List)
            {
                Damage dmg = castType == AOECastType.OVER_TIME ? damage / triggerPerSecond : damage;
                target.TakeDamage(dmg * effectMultiplier, accuracy, critChance, critMultiplier);
            }
        }

        public override Skill Clone()
        {
            AoeSkill clone = Instantiate(this);
            clone.buffsToApply = new List<BuffSystem.Buff>(this.buffsToApply);
            clone.damages = new DamageList(this.damages);

            clone.modificationContainers = new List<SkillPrefabModificationContainer>(this.modificationContainers);

            return clone;
        }

        public override bool Cast(SkillCaster caller)
        {
            if (destroyBeforeInstantiatingNew)
            {
                foreach (GameObject g in instantiatedBehaviours)
                {
                    Destroy(g);
                }
                instantiatedBehaviours.Clear();
            }

            switch (skillDimension)
            {
                case SkillDimension.XY:
                    CastXY(caller);
                    break;
                case SkillDimension.XZ:
                    CastXZ(caller);
                    break;
                default:
                    CastXYZ(caller);
                    break;
            }

            return true;
        }
        #region "Cast Types"
        void CastXY(SkillCaster caster)
        {
            Vector3 pos;
            if (!isChildOfCaster)
            {
                pos = castPosition;
                if (castsExactlyAtRange)
                {
                    pos = (pos - caster.transform.position).normalized * range + caster.transform.position;
                }
            }
            else
            {
                pos = caster.transform.position;
            }

            InstantiateAOE(caster, out GameObject instantiated, out SkillCaster aoeSkillCaster);
            instantiated.transform.position = pos;
            instantiated.transform.rotation = caster.projectileOut.rotation;

            if (isChildOfCaster && velocity == Vector3.zero) instantiated.transform.parent = caster.transform;
            AoeBehaviour2D behaviour = instantiated.GetComponent<AoeBehaviour2D>();

            if (!instantiatedBehaviours.Contains(instantiated)) instantiatedBehaviours.Add(instantiated);

            aoeSkillCaster.skillManager = skillManager;

            behaviour.Init(this, caster.transform);

            if (isToggled)
            {
                aoeSkillCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
            }
        }
        void CastXZ(SkillCaster caster)
        {
            Vector3 pos;
            if (velocity == Vector3.zero && !isChildOfCaster)
            {
                pos = castPosition;
                if (castsExactlyAtRange)
                {
                    pos = (pos - caster.transform.position).CullAxes(Vector3Utilities.Vector3Axis.Y).normalized * range + caster.transform.position;
                }
            }
            else
            {
                pos = caster.transform.position;
            }

            InstantiateAOE(caster, out GameObject instantiated, out SkillCaster aoeSkillCaster);
            instantiated.transform.position = pos;
            instantiated.transform.rotation = caster.projectileOut.rotation;

            if (isChildOfCaster && velocity == Vector3.zero) instantiated.transform.parent = caster.transform;
            AoeBehaviour behaviour = instantiated.GetComponent<AoeBehaviour>();

            if (!instantiatedBehaviours.Contains(instantiated)) instantiatedBehaviours.Add(instantiated);

            aoeSkillCaster.skillManager = skillManager;

            behaviour.Init(this, caster.transform);

            if (isToggled)
            {
                aoeSkillCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
            }
        }
        void CastXYZ(SkillCaster caster)
        {
            CastXZ(caster);
        }
        #endregion

        void InstantiateAOE(SkillCaster caster, out GameObject instantiated, out SkillCaster aoeSkillCaster)
        {
            if (defaultSkillPrefab.TryGetComponent<PoolObject>(out PoolObject po))
            {
                instantiated = UniversalPoolProvider.GetPool(po.ID).Request(po).gameObject;
            }
            else instantiated = Instantiate(defaultSkillPrefab);

            if (instantiated.TryGetComponent<SkillCaster>(out SkillCaster skillCaster))
            {
                aoeSkillCaster = skillCaster;
            }
            else aoeSkillCaster = instantiated.AddComponent<SkillCaster>();
        }

        public override bool IsInRange(SkillCaster caster, Vector3 targetPosition)
        {
            if (range <= 0f || castsExactlyAtRange) return true;

            return Vector3.Distance(caster.transform.position, targetPosition) < range;
        }

        public enum AOECastType
        {
            ON_START,
            ON_END,
            OVER_TIME
        }
    }
}
