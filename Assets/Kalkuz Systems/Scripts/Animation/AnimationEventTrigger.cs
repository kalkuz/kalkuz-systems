using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle.SkillSystem;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems
{
    namespace Animation
    {
        [RequireComponent(typeof(Animator))]
        public class AnimationEventTrigger : MonoBehaviour
        {
            public UnityAction OnSpellCast;

            public void CastSpell()
            {
                OnSpellCast?.Invoke();
            }
        }
    }
}
