using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.SkillSystem
{
    [System.Serializable]
    public class SkillToggleEvent
    {
        public SkillToggleEventType eventType;
        public UnityEvent toggleEvent;
    }

    public enum SkillToggleEventType
    {
        CAST,
        DESTROY_INSTANTIATED_BEHAVIOUR,
        STOP_COROUTINE
    }
}
