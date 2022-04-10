using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    public class ProjectileBehaviourData : MonoBehaviour
    {
        public LayerMask bounceSurfaces;
        public float collisionRadius;
        public GameObject onHitParticles;
        public UnityEngine.Events.UnityEvent onEnable, onHit, onDestroy;
    }
}
