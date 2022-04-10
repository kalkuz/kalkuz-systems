using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class MovementBehaviour : MonoBehaviour
    {
        public UnityEvent onEnable, onHit, onDestroy;

        SkillCaster caster;

        public void Init(SkillCaster caster)
        {
            this.caster = caster;
            caster.Initialize();
        }

        private void OnEnable()
        {
            onEnable?.Invoke();
        }

        public void OnDestroy()
        {
            caster.OnEnd();
            onDestroy?.Invoke();
        }
    }
}
