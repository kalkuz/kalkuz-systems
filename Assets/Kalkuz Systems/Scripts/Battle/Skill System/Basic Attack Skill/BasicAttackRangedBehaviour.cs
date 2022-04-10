using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KalkuzSystems.Utility.Transform;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class BasicAttackRangedBehaviour : MonoBehaviour
    {
        float speed, acceleration;
        CharacterData target;
        SkillCaster source;
        BasicAttackSkill sourceSkill;

        public void Init(BasicAttackSkill sourceSkill, SkillCaster source)
        {
            this.sourceSkill = sourceSkill;

            target = sourceSkill.targetCharacter;
            this.source = source;

            speed = sourceSkill.speed;
            acceleration = sourceSkill.acceleration;
        }

        private void Update()
        {
            speed += acceleration * Time.deltaTime;

            float dist = Vector3.Distance(transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y), target.transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y));
            if (dist < 0.1f)
            {
                foreach (BuffSystem.Buff buff in sourceSkill.buffsToApply)
                {
                    buff.Clone().Inflict(target, source.transform);
                }
                foreach (Damage damage in sourceSkill.damages.List)
                {
                    target.TakeDamage(damage, sourceSkill.accuracy, sourceSkill.critChance, sourceSkill.critMultiplier);
                }

                Destroy(gameObject);
            }
            else
            {
                transform.forward = (target.transform.position - transform.position).CullAxes(Vector3Utilities.Vector3Axis.Y).normalized;
            }

            if (dist < speed * Time.deltaTime)
            {
                transform.position = target.transform.position;
            }
            else
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }
    }
}
