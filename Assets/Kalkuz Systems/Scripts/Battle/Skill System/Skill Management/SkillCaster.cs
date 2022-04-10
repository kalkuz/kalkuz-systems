using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class SkillCaster : MonoBehaviour
    {
        public SkillManager skillManager;
        
        [Tooltip("Where the projectile should come out.")]
        public Transform projectileOut;
        public Transform meleeAttackPoint;

        public UnityEvent onDestroy;

        public void Initialize()
        {
            if (onDestroy == null) onDestroy = new UnityEvent();

            if (projectileOut == null) projectileOut = transform;

            foreach (SkillContainer sc in skillManager.skills)
            {
                sc.OnAttachmentChanged?.AddListener(sc.Initialize);
                sc.OnAttachmentChanged?.Invoke();

                if (sc.castAutomation == SkillContainer.CastAutomation.ON_START)
                {
                    OnStart(sc);
                }

                if (sc.castAutomation == SkillContainer.CastAutomation.OVER_TIME)
                {
                    StartCoroutine(OnDeltaTime(sc));
                }

                if (!sc.available) Cooldown(skillManager.skills.IndexOf(sc));
            }
        }

        void OnStart(SkillContainer sc)
        {
            sc.skill.castPosition = transform.position;
            sc.skill.Cast(this);
        }

        public void OnHit()
        {
            foreach (SkillContainer sc in skillManager.skills)
            {
                if (sc.castAutomation == SkillContainer.CastAutomation.ON_HIT)
                {
                    sc.skill.castPosition = transform.position;
                    sc.skill.Cast(this);
                }
            }
        }
        public void OnEnd()
        {
            foreach (SkillContainer sc in skillManager.skills)
            {
                if (sc.castAutomation == SkillContainer.CastAutomation.ON_END)
                {
                    sc.skill.castPosition = transform.position;
                    sc.skill.Cast(this);
                }
            }
        }

        IEnumerator OnDeltaTime(SkillContainer sc)
        {
            while (true)
            {
                yield return new WaitForSeconds(sc.secondsBetweenCasts);
                sc.skill.castPosition = transform.position;
                sc.skill.Cast(this);
            }
        }

        public void Cooldown(int skillIndex)
        {
            skillManager.skills[skillIndex].available = false;
            StartCoroutine(CooldownProcedure(skillManager.skills[skillIndex]));
        }
        IEnumerator CooldownProcedure(SkillContainer sc)
        {
            yield return new WaitForSeconds(sc.skill.cooldown);
            sc.available = true;
        }
        private void OnDestroy()
        {
            onDestroy?.Invoke();
        }
    }
}
