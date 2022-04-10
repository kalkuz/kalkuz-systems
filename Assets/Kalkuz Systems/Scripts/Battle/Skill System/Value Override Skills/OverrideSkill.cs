using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle.SkillSystem
{
    public abstract class OverrideSkill : UsableSkill
    {
        [Header("Properties")]
        public int targetedSkillIndex;

        public float fadeInTime;
        public float fadeOutTime;
        public float delayBetweenFades;

        [Header("Events")]
        public UnityEvent<ProjectileBehaviour, ProjectileSkill, float> fadeInEvent;
        public UnityEvent<ProjectileBehaviour, ProjectileSkill, float> fadeOutEvent;
        public UnityEvent<ProjectileBehaviour, ProjectileSkill, float> betweenFadesEvent;
    }
}
