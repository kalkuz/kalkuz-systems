using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.DataStructures.Pooling;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Melee Skill", fileName = "New Melee Skill", order = 0)]
    public class MeleeSkill : UsableSkill
    {
        [Header("Properties")]
        [Tooltip("Strike hits only one target, non-strike melee attack is an AoE.")]
        public bool isStrike;
        public float attackRadius;
        public bool debug;

        public override Skill Clone()
        {
            MeleeSkill clone = Instantiate(this);

            clone.buffsToApply = new List<BuffSystem.Buff>(this.buffsToApply);
            clone.damages = new DamageList(this.damages);

            clone.modificationContainers = new List<SkillPrefabModificationContainer>(this.modificationContainers);

            return clone;
        }

        public override bool Cast(SkillCaster caster)
        {
            if (isStrike) MeleeStrike(caster);
            else
            {
                switch (skillDimension)
                {
                    case SkillDimension.XY:
                        MeleeAoE2D(caster);
                        break;
                    default:
                        MeleeAoE3D(caster);
                        break;
                }
            }

            return true;
        }

        void MeleeStrike(SkillCaster caster)
        {
            if (targetCharacter == null) return;

            if (((1 << targetCharacter.gameObject.layer) & targets) != 0)
            {
                CharacterData targetData = targetCharacter.GetComponent<CharacterData>();
                foreach (BuffSystem.Buff b in buffsToApply)
                {
                    b.Clone().Inflict(targetData, caster.transform);
                }
                foreach (Damage d in damages.List)
                {
                    targetData.TakeDamage(d, accuracy, critChance, critMultiplier);
                }

                GameObject meleeObject;
                if (defaultSkillPrefab.TryGetComponent<PoolObject>(out PoolObject po))
                {
                    meleeObject = UniversalPoolProvider.GetPool(po.ID).Request(po).gameObject;
                    meleeObject.transform.rotation = defaultSkillPrefab.transform.rotation;
                }
                else
                {
                    meleeObject = Instantiate(defaultSkillPrefab);
                }

                meleeObject.transform.position = targetCharacter.transform.position;
                SkillCaster meleeCaster = meleeObject.AddComponent<SkillCaster>();
                meleeCaster.skillManager = skillManager;
                meleeCaster.Initialize();

                if (isToggled)
                {
                    meleeCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
                }
            }
        }

        void MeleeAoE2D(SkillCaster caster)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(caster.meleeAttackPoint.position, attackRadius, targets);

            if (debug)
            {
                for (float i = 1; i < 64; i++)
                {
                    Vector3 prev = new Vector3(Mathf.Cos(2 * Mathf.PI * (i - 1) / 64f), Mathf.Sin(2 * Mathf.PI * (i - 1) / 64f)) * attackRadius;
                    Vector3 cur = new Vector3(Mathf.Cos(2 * Mathf.PI * i / 64f), Mathf.Sin(2 * Mathf.PI * i / 64f)) * attackRadius;
                    Debug.DrawLine(caster.meleeAttackPoint.position + prev, caster.meleeAttackPoint.position + cur, Color.red, 1f);
                }
            }

            if (cols.Length < 1) return;

            foreach (Collider2D col in cols)
            {
                col.TryGetComponent<CharacterData>(out CharacterData targetData);

                foreach (BuffSystem.Buff b in buffsToApply)
                {
                    b.Clone().Inflict(targetData, caster.transform);
                }
                foreach (Damage d in damages.List)
                {
                    targetData.TakeDamage(d, accuracy, critChance, critMultiplier);
                }

                GameObject meleeObject;
                if (defaultSkillPrefab.TryGetComponent<PoolObject>(out PoolObject po))
                {
                    meleeObject = UniversalPoolProvider.GetPool(po.ID).Request(po).gameObject;
                    meleeObject.transform.rotation = defaultSkillPrefab.transform.rotation;
                }
                else
                {
                    meleeObject = Instantiate(defaultSkillPrefab);
                }

                meleeObject.transform.position = targetData.transform.position;
                SkillCaster meleeCaster = meleeObject.AddComponent<SkillCaster>();
                meleeCaster.skillManager = skillManager;
                meleeCaster.Initialize();

                if (isToggled)
                {
                    meleeCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
                }
            }
        }
        void MeleeAoE3D(SkillCaster caster)
        {
            Collider[] cols = Physics.OverlapSphere(caster.meleeAttackPoint.position, attackRadius, targets);

            if (debug)
            {
                for (float i = 1; i < 64; i++)
                {
                    Vector3 prev = new Vector3(Mathf.Cos(2 * Mathf.PI * (i - 1) / 64f), 0, Mathf.Sin(2 * Mathf.PI * (i - 1) / 64f)) * attackRadius;
                    Vector3 cur = new Vector3(Mathf.Cos(2 * Mathf.PI * i / 64f), 0, Mathf.Sin(2 * Mathf.PI * i / 64f)) * attackRadius;
                    Debug.DrawLine(caster.meleeAttackPoint.position + prev, caster.meleeAttackPoint.position + cur, Color.red, 1f);
                }
            }

            if (cols.Length < 1) return;

            foreach (Collider col in cols)
            {
                col.TryGetComponent<CharacterData>(out CharacterData targetData);

                foreach (BuffSystem.Buff b in buffsToApply)
                {
                    b.Clone().Inflict(targetData, caster.transform);
                }
                foreach (Damage d in damages.List)
                {
                    targetData.TakeDamage(d, accuracy, critChance, critMultiplier);
                }

                GameObject meleeObject;
                if (defaultSkillPrefab.TryGetComponent<PoolObject>(out PoolObject po))
                {
                    meleeObject = UniversalPoolProvider.GetPool(po.ID).Request(po).gameObject;
                    meleeObject.transform.rotation = defaultSkillPrefab.transform.rotation;
                }
                else
                {
                    meleeObject = Instantiate(defaultSkillPrefab);
                }

                meleeObject.transform.position = targetCharacter.transform.position;
                SkillCaster meleeCaster = meleeObject.AddComponent<SkillCaster>();
                meleeCaster.skillManager = skillManager;
                meleeCaster.Initialize();

                if (isToggled)
                {
                    meleeCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
                }
            }
        }

        public override bool IsInRange(SkillCaster caster, Vector3 targetPosition)
        {
            if (range <= 0f) return true;

            return Vector3.Distance(caster.transform.position, targetPosition) < range;
        }
    }
}
