using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle.BuffSystem;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    public abstract class ProjectileBehaviour : MonoBehaviour
    {
        protected ProjectileBehaviourData projectileBehaviourData;
        protected SkillDimension dimension;

        [HideInInspector] public float speed, acceleration, accelerationForce;
        [HideInInspector] public int pierce;
        [HideInInspector] public float damageScalar;

        protected GameObject source;
        protected List<GameObject> hitAlready;

        public abstract void Init(GameObject source, ProjectileSkill skill);
        protected abstract void Behave2D();
        protected abstract void Behave3D();

        public GameObject InstantiateParticles()
        {
            if (projectileBehaviourData.onHitParticles == null) return null;

            return Instantiate(projectileBehaviourData.onHitParticles, transform.position, projectileBehaviourData.onHitParticles.transform.rotation * transform.rotation);
        }
    }

    [System.Serializable]
    public class ProjectileBehaviourScalars
    {
        public float damageScalar, speedScalar, accelerationScalar, accelerationForceScalar;
        [Space]
        public float damageAddition;
        public float speedAddition, accelerationAddition, accelerationForceAddition;
    }
}
