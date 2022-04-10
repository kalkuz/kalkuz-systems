using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle.BuffSystem;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Summoning Skill", fileName = "New Summoning Skill", order = 0)]
    public class SummoningSkill : UsableSkill
    {
        [Header("Properties")]
        public bool summonsAtTarget;
        public List<Vector3> relativeSummonPositions;

        public override Skill Clone()
        {
            SummoningSkill clone = Instantiate(this);
            clone.buffsToApply = new List<BuffSystem.Buff>(this.buffsToApply);
            clone.damages = new DamageList(this.damages);

            clone.modificationContainers = new List<SkillPrefabModificationContainer>(this.modificationContainers);

            clone.relativeSummonPositions = new List<Vector3>(this.relativeSummonPositions);

            return clone;
        }
        public override bool Cast(SkillCaster caster)
        {
            CharacterData casterData = caster.GetComponent<CharacterData>();

            for (int i = 0; i < relativeSummonPositions.Count; i++)
            {
                Vector3 _summonoffset = Quaternion.AngleAxis(caster.transform.rotation.eulerAngles.y, Vector3.up) * relativeSummonPositions[i];
                Vector3 _summonPos;
                if (summonsAtTarget)
                {
                    if (castsExactlyAtRange)
                    {
                        _summonPos = (castPosition - caster.transform.position).CullAxes(Vector3Utilities.Vector3Axis.Y).normalized * range + caster.transform.position;
                    }
                    else
                    {
                        _summonPos = castPosition;
                    }
                }
                else
                {
                    _summonPos = caster.transform.position;
                }
                GameObject _newSummon = Instantiate(defaultSkillPrefab, (_summonPos + _summonoffset).CullAxes(Vector3Utilities.Vector3Axis.Y), caster.transform.rotation);

                SkillCaster summonSkillCaster = _newSummon.AddComponent<SkillCaster>();
                summonSkillCaster.skillManager = skillManager;

                summonSkillCaster.Initialize();

                if (isToggled)
                {
                    summonSkillCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
                }

                foreach (Buff buff in buffsToApply)
                {
                    Buff cloned = buff.Clone();
                    if (cloned is Soulbond)
                    {
                        (cloned as Soulbond).Bond(_newSummon);
                        cloned.Inflict(casterData, caster.transform);
                    }
                }
            }

            return true;
        }

        public override bool IsInRange(SkillCaster caster, Vector3 targetPosition)
        {
            if (range <= 0f || castsExactlyAtRange) return true;

            return Vector3.Distance(caster.transform.position, targetPosition) < range;
        }
    }
}
