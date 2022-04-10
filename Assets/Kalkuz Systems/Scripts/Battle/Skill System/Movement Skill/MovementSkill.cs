using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.DataStructures.Pooling;
using KalkuzSystems.Utility.Transform;
using UnityEngine;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skills/Usable Skill/Movement Skill", fileName = "New Movement Skill", order = 0)]
    public class MovementSkill : UsableSkill
    {
        [Header("Properties")]
        public AnimationCurve movementInterpolator;
        [Tooltip("How many steps needed to make the movement? 1 for instant teleportation.")]
        public int movementSteps;
        [Tooltip("The time between steps in seconds.")]
        public float waitUntilNextStep;
        [Tooltip("Should move over not movable surfaces")]
        public bool movesOverObstacles;
        [Tooltip("Immovable surfaces")]
        public LayerMask immovableSurfaces;
        [Tooltip("Does it moves reversely where it should move towards")]
        public bool movesReverse;
        public bool useExactSpeed;
        public float movementSpeed;

        [Space]
        [Tooltip("Should it buff self or targets?")]
        public bool buffsSelf;
        [Space]
        [Tooltip("Radius of damage operation")]
        public float damageRadius;

        bool breakRoutine;

        public override Skill Clone()
        {
            MovementSkill clone = Instantiate(this);

            clone.buffsToApply = new List<BuffSystem.Buff>(this.buffsToApply);
            clone.damages = new DamageList(this.damages);

            clone.modificationContainers = new List<SkillPrefabModificationContainer>(this.modificationContainers);

            return clone;
        }
        public override bool Cast(SkillCaster caster)
        {
            if (destroyBeforeInstantiatingNew)
            {
                foreach (GameObject g in instantiatedBehaviours)
                {
                    Destroy(g);
                }
                instantiatedBehaviours.Clear();
            }

            if (buffsSelf)
            {
                foreach (BuffSystem.Buff b in buffsToApply)
                {
                    b.Clone().Inflict(caster.GetComponent<CharacterData>(), caster.transform);
                }
            }

            breakRoutine = false;

            switch (skillDimension)
            {
                case SkillDimension.XY:
                    caster.StartCoroutine(MovementProcedureXY(caster.transform));
                    break;
                case SkillDimension.XZ:
                    caster.StartCoroutine(MovementProcedureXZ(caster.transform));
                    break;
                default:
                    caster.StartCoroutine(MovementProcedureXZ(caster.transform));
                    break;
            }

            return true;
        }
        #region "XY"
        IEnumerator MovementProcedureXY(Transform transform)
        {
            #region "Parameter Calculations"
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition;
            if (castsExactlyAtRange)
            {
                targetPosition = initialPosition + (castPosition - initialPosition).normalized * range * (movesReverse ? -1f : 1f);
            }
            else
            {
                targetPosition = initialPosition + Vector3.ClampMagnitude(castPosition - initialPosition, range) * (movesReverse ? -1f : 1f);
            }

            if (!movesOverObstacles)
            {
                RaycastHit2D hit = Physics2D.CircleCast(initialPosition, 1f, targetPosition, range, immovableSurfaces);
                if (hit.collider)
                {
                    targetPosition = hit.point;
                }
            }

            if (useExactSpeed && movementSpeed > 0)
            {
                var t = Vector3.Distance(targetPosition, transform.position) / movementSpeed;
                movementSteps = Mathf.FloorToInt(t / Time.deltaTime);
            }
            if (movementSteps < 1) movementSteps = 1;
            #endregion

            #region "Object creation"
            GameObject movementObject;
            if (defaultSkillPrefab.TryGetComponent<PoolObject>(out PoolObject po))
            {
                movementObject = UniversalPoolProvider.GetPool(po.ID).Request(po).gameObject;
                movementObject.transform.rotation = defaultSkillPrefab.transform.rotation;
            }
            else
            {
                movementObject = Instantiate(defaultSkillPrefab, transform);
            }
            movementObject.transform.parent = transform;
            movementObject.transform.localPosition = Vector3.zero;

            SkillCaster movementCaster = movementObject.AddComponent<SkillCaster>();
            movementCaster.skillManager = skillManager;

            MovementBehaviour movementBehaviour = movementObject.GetComponent<MovementBehaviour>();
            movementBehaviour.Init(movementCaster);
            #endregion

            if (isToggled)
            {
                movementCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
            }

            instantiatedBehaviours.Add(movementObject);

            UnityEngine.AI.NavMeshAgent nma = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();

            List<Collider> hitAlready = new List<Collider>();
            for (int i = 1; i <= movementSteps; i++)
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition, movementInterpolator.Evaluate(i * 1f / movementSteps));

                if (damages.List.Count > 0)
                {
                    foreach (Collider c in Physics.OverlapSphere(transform.position, damageRadius, targets))
                    {
                        if (hitAlready.Contains(c)) continue;

                        if (c.TryGetComponent<CharacterData>(out CharacterData cData))
                        {
                            movementCaster.OnHit();
                            hitAlready.Add(c);
                            foreach (Damage d in damages.List)
                            {
                                cData.TakeDamage(d, accuracy, critChance, critMultiplier);
                            }
                            if (!buffsSelf)
                            {
                                foreach (BuffSystem.Buff buff in buffsToApply)
                                {
                                    buff.Clone().Inflict(cData, transform);
                                }
                            }
                        }
                    }
                }

                if (nma) nma.SetDestination(transform.position);

                if (waitUntilNextStep == 0) yield return null;
                else yield return new WaitForSeconds(waitUntilNextStep);

                if (breakRoutine) break;
            }

            movementBehaviour.OnDestroy();
        }
        #endregion
        #region "XZ"
        IEnumerator MovementProcedureXZ(Transform transform)
        {
            #region "Parameter Calculations"
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition;
            if (castsExactlyAtRange)
            {
                targetPosition = initialPosition + (castPosition - initialPosition).CullAxes(Vector3Utilities.Vector3Axis.Y).normalized * range * (movesReverse ? -1f : 1f);
            }
            else
            {
                targetPosition = initialPosition + Vector3.ClampMagnitude(castPosition.CullAxes(Vector3Utilities.Vector3Axis.Y) - initialPosition, range) * (movesReverse ? -1f : 1f);
            }

            if (!movesOverObstacles)
            {
                RaycastHit hit;
                if (Physics.SphereCast(initialPosition, 1f, targetPosition.CullAxes(Vector3Utilities.Vector3Axis.Y), out hit, range, immovableSurfaces))
                {
                    targetPosition = hit.point;
                }
            }

            if (useExactSpeed && movementSpeed > 0)
            {
                var t = Vector3.Distance(targetPosition, transform.position) / movementSpeed;
                movementSteps = Mathf.FloorToInt(t / Time.deltaTime);
            }
            if (movementSteps < 1) movementSteps = 1;
            #endregion

            #region "Object creation"
            GameObject movementObject;
            if (defaultSkillPrefab.TryGetComponent<PoolObject>(out PoolObject po))
            {
                movementObject = UniversalPoolProvider.GetPool(po.ID).Request(po).gameObject;
                movementObject.transform.rotation = defaultSkillPrefab.transform.rotation;
            }
            else
            {
                movementObject = Instantiate(defaultSkillPrefab, transform);
            }
            movementObject.transform.parent = transform;
            movementObject.transform.localPosition = Vector3.zero;

            SkillCaster movementCaster = movementObject.AddComponent<SkillCaster>();
            movementCaster.skillManager = skillManager;

            MovementBehaviour movementBehaviour = movementObject.GetComponent<MovementBehaviour>();
            movementBehaviour.Init(movementCaster);
            #endregion

            if (isToggled)
            {
                movementCaster.onDestroy.AddListener(InstantiatedOnDestroyBehaviour);
            }

            instantiatedBehaviours.Add(movementObject);

            UnityEngine.AI.NavMeshAgent nma = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();

            List<Collider> hitAlready = new List<Collider>();
            for (int i = 1; i <= movementSteps; i++)
            {
                transform.position = Vector3.Lerp(initialPosition, targetPosition, i * 1f / movementSteps);

                if (damages.List.Count > 0)
                {
                    foreach (Collider c in Physics.OverlapSphere(transform.position, damageRadius, targets))
                    {
                        if (hitAlready.Contains(c)) continue;

                        if (c.TryGetComponent<CharacterData>(out CharacterData cData))
                        {
                            movementCaster.OnHit();
                            hitAlready.Add(c);
                            foreach (Damage d in damages.List)
                            {
                                cData.TakeDamage(d, accuracy, critChance, critMultiplier);
                            }
                            if (!buffsSelf)
                            {
                                foreach (BuffSystem.Buff buff in buffsToApply)
                                {
                                    buff.Clone().Inflict(cData, transform);
                                }
                            }
                        }
                    }
                }

                if (nma) nma.SetDestination(transform.position);

                if (breakRoutine) break;

                if (waitUntilNextStep == 0) yield return null;
                else yield return new WaitForSeconds(waitUntilNextStep);
            }

            movementBehaviour.OnDestroy();
        }
        #endregion

        public override bool IsInRange(SkillCaster caster, Vector3 targetPosition)
        {
            return true;
        }

        public void StopCoroutine()
        {
            breakRoutine = true;
        }
    }
}
