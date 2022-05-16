using System;
using KalkuzSystems.Attributes;
using KalkuzSystems.DataStructures.Pooling;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KalkuzSystems.Battle.SkillSystem
{
    [RequireComponent(typeof(PoolObject))]
    public sealed class Projectile : MonoBehaviour
    {
        private Transform m_transform;
        private Transform m_sourceTransform;
        
        // private SkillCaster m_skillCaster;

        private ProjectileSkillData m_skillData;
        
        private Vector3 currentSpeed = Vector3.zero;
        private Vector3 currentAcceleration = Vector3.zero;
        
        private float currentOrbitalAngle = 0f;
        private float orbitalRadius = 0f;
        private bool negateOrbitalRotation = false;
        
        [Title("Movement")]
        [SerializeField] private float speedMultiplier = 1f;
        
        [Title("Collision"), LineSeparator]
        [SerializeField] private float collisionRadius = 1f;
        [SerializeField] private float collisionRadiusMultiplier = 1f;

        [Title("Behaviour")]
        [SerializeField] private float pierce = 0f;
        [SerializeField] private float ricochet = 0f;
        [SerializeField] private float bounce = 0f;
        
        private float NetCollisionRadius => collisionRadius * collisionRadiusMultiplier;

        private Vector3 NetSpeed => currentSpeed * speedMultiplier;
        
        public void Initialize(ProjectileSkillData data, Transform sourceTransform, Vector3 position, Vector3 direction)
        {
            if (data == null) throw new NullReferenceException("Passed null parameter data to Initialize method.");
            
            Reset();
            
            // Dynamic variables
            m_skillData = data;
            m_sourceTransform = sourceTransform;
            
            // Transforms
            if (m_transform == null) m_transform = transform;

            m_transform.position = position;
            
            if (data.Is2D) m_transform.right = direction;
            else m_transform.forward = direction;
            
            // Set values
            currentSpeed = data.Speed;
            currentAcceleration = data.Acceleration;

            var distanceVector = m_transform.position - m_sourceTransform.position;
            orbitalRadius = distanceVector.magnitude;

            if (data.Is2D)
            {
                currentOrbitalAngle = Vector3.SignedAngle(Vector3.right, distanceVector, Vector3.forward) + 360f;
                currentOrbitalAngle %= 360f;
            }
            else
            {
                currentOrbitalAngle = Vector3.SignedAngle(Vector3.forward, distanceVector, Vector3.up) + 360f;
                currentOrbitalAngle %= 360f;
            }
            
            if (data.RandomizeOrbitalDirection) negateOrbitalRotation = Random.value < 0.5f;
            
            pierce = data.Pierce;
            ricochet = data.Ricochet;
            bounce = data.Bounce;
        }

        private void Reset()
        {
            speedMultiplier = 1f;
            
            collisionRadiusMultiplier = 1f;
        }

        #region Update

        private void Update()
        {
            if (m_skillData == null) return;

            currentAcceleration += m_skillData.AccelerationIncrease * Time.deltaTime;
            currentSpeed += currentAcceleration * Time.deltaTime;

            switch (m_skillData.ProjectileType)
            {
                case ProjectileType.ORBITAL:
                    OrbitalBehaviour();
                    break;
                case ProjectileType.BOOMERANG:
                    BoomerangBehaviour();
                    break;
                default:
                    DefaultBehaviour();
                    break;
            }
        }

        private void DefaultBehaviour()
        {
            if (m_skillData.Is2D) m_transform.right = NetSpeed.normalized;
            else m_transform.forward = NetSpeed.normalized;

            m_transform.position += NetSpeed * Time.deltaTime;
        }

        private void OrbitalBehaviour()
        {
            var angleSpeed = Mathf.Clamp(NetSpeed.x, NetSpeed.y, NetSpeed.z) * 360f;
            var deltaAngle = angleSpeed * Time.deltaTime;
            
            if (negateOrbitalRotation) deltaAngle *= -1;
            
            var distanceVector = m_transform.position - m_sourceTransform.position;
            var nextAngle = currentOrbitalAngle + deltaAngle;
            
            Vector3 nextPosition;
            if (m_skillData.Is2D)
            {
                nextPosition = Quaternion.AngleAxis(nextAngle, Vector3.forward) * Vector3.right * orbitalRadius;
                
                var dirVector = Vector3.Cross(deltaAngle > 0f ? Vector3.back : Vector3.forward, distanceVector);
                transform.right = dirVector;
            }
            else
            {
                nextPosition = Quaternion.AngleAxis(nextAngle, Vector3.down) * Vector3.forward * orbitalRadius;
                
                var dirVector = Vector3.Cross(deltaAngle > 0f ? Vector3.down : Vector3.up, distanceVector);
                transform.forward = dirVector;
            }

            m_transform.position = m_sourceTransform.position + nextPosition;
            currentOrbitalAngle = nextAngle % 360f;
        }

        private void BoomerangBehaviour()
        {
            
        }

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, NetCollisionRadius);
        }
    }
}
//  
// { 
//     public class DefaultProjectileBehaviour : ProjectileBehaviour 
//     { 
//         StandardProjectileSkill sourceSkill; 
//         SkillCaster skillCaster; 
//         int bounce, ricochet; 
//  
//         public override void Init(GameObject source, ProjectileSkill skill) 
//         { 
//             if (!(skill is StandardProjectileSkill)) throw new System.Exception($"Skill is not {typeof(StandardProjectileSkill)}"); 
//  
//             this.enabled = true; 
//  
//             skillCaster = GetComponent<SkillCaster>(); 
//             skillCaster.Initialize(); 
//  
//             hitAlready = new List<GameObject>(); 
//  
//             this.source = source; 
//             this.sourceSkill = skill as StandardProjectileSkill; 
//  
//             this.dimension = sourceSkill.skillDimension; 
//  
//             this.speed = sourceSkill.projectileSpeed; 
//             this.acceleration = sourceSkill.projectileAcceleration; 
//             this.accelerationForce = sourceSkill.projectileAccelerationForce; 
//  
//             this.pierce = sourceSkill.pierceCount; 
//             this.bounce = sourceSkill.bounceCount; 
//             this.ricochet = sourceSkill.ricochetCount; 
//  
//             this.damageScalar = 1f; 
//  
//             this.projectileBehaviourData = GetComponent<ProjectileBehaviourData>(); 
//  
//             if (sourceSkill.projectileLifetime > 0f) Invoke("OnDestroy", sourceSkill.projectileLifetime); 
//  
//             projectileBehaviourData.onEnable?.Invoke(); 
//         } 
//  
//         void Update() 
//         { 
//             if (source == null) 
//             { 
//                 OnDestroy(); 
//                 return; 
//             } 
//  
//             acceleration += accelerationForce * Time.deltaTime; 
//             speed += acceleration * Time.deltaTime; 
//  
//             if (sourceSkill.maxDistanceFromOwner > 0 && Vector3.Distance(transform.position, source.transform.position) > sourceSkill.maxDistanceFromOwner) 
//             { 
//                 OnDestroy(); 
//             } 
//  
//             if (dimension == SkillDimension.XY) Behave2D(); 
//             else Behave3D(); 
//         } 
//  
//         #region "2D" 
//         protected override void Behave2D() 
//         { 
//             RaycastHit2D hit = Physics2D.CircleCast(transform.position, projectileBehaviourData.collisionRadius, transform.right, speed * Time.deltaTime, sourceSkill.targets); 
//             if (hit.collider) 
//             { 
//                 Collider2D other = hit.collider; 
//  
//                 if (((1 << other.gameObject.layer) & projectileBehaviourData.bounceSurfaces) != 0) 
//                 { 
//                     transform.position = hit.point; 
//                     if (bounce == 0) 
//                     { 
//                         OnDestroy(); 
//                     } 
//                     else 
//                     { 
//                         transform.right = Vector3.Reflect(transform.right, hit.normal.normalized); 
//                         bounce--; 
//                         sourceSkill.onBounce?.Invoke(this, sourceSkill.bounceScalars); 
//                     } 
//                 } 
//                 else if (1 << source.gameObject.layer != 1 << other.gameObject.layer && !hitAlready.Contains(other.gameObject)) 
//                 { 
//                     transform.position = hit.point; 
//                     hitAlready.Add(other.gameObject); 
//  
//                     CharacterData charDataHit = other.gameObject.GetComponent<CharacterData>(); 
//  
//                     foreach (BuffSystem.Buff buff in sourceSkill.buffsToApply) 
//                     { 
//                         buff.Clone().Inflict(charDataHit, source.transform); 
//                     } 
//                     foreach (Damage damage in sourceSkill.damages.List) 
//                     { 
//                         charDataHit.TakeDamage(damage, sourceSkill.accuracy, sourceSkill.critChance, sourceSkill.critMultiplier); 
//                     } 
//  
//                     if (ricochet != 0) 
//                     { 
//                         skillCaster.OnHit(); 
//                         projectileBehaviourData.onHit?.Invoke(); 
//                         InstantiateParticles(); 
//  
//                         RicochetAnotherEnemy3D(1 << other.gameObject.layer); 
//                         ricochet--; 
//                         sourceSkill.onRicochet?.Invoke(this, sourceSkill.ricochetScalars); 
//                     } 
//                     else if (pierce == 0) 
//                     { 
//                         skillCaster.OnHit(); 
//                         projectileBehaviourData.onHit?.Invoke(); 
//                         InstantiateParticles(); 
//                         OnDestroy(); 
//                     } 
//                     else 
//                     { 
//                         skillCaster.OnHit(); 
//                         projectileBehaviourData.onHit?.Invoke(); 
//                         InstantiateParticles(); 
//                         pierce--; 
//                         sourceSkill.onPierce?.Invoke(this, sourceSkill.pierceScalars); 
//                     } 
//                 } 
//                 else 
//                 { 
//                     transform.position += transform.right * speed * Time.deltaTime; 
//                 } 
//             } 
//             else 
//             { 
//                 transform.position += transform.right * speed * Time.deltaTime; 
//             } 
//         } 
//         void RicochetAnotherEnemy2D(LayerMask otherLayer) 
//         { 
//             Vector3 newTarget = Vector3.zero; 
//             Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 20f, otherLayer); 
//             if (cols.Length == 0) OnDestroy(); 
//  
//             Vector3 bestTarget = transform.right * 1000f; 
//  
//             foreach (Collider2D col in cols) 
//             { 
//                 if (hitAlready.Contains(col.gameObject)) continue; 
//                 RaycastHit2D hit = Physics2D.Raycast(transform.position, col.transform.position - transform.position, Vector3.Distance(transform.position, col.transform.position), 
//                                     projectileBehaviourData.bounceSurfaces); 
//  
//                 if (!hit.collider) 
//                 { 
//                     newTarget = col.transform.position - transform.position; 
//                     if (newTarget.sqrMagnitude < bestTarget.sqrMagnitude) 
//                     { 
//                         bestTarget = newTarget; 
//                     } 
//                 } 
//             } 
//  
//             if (bestTarget == transform.right * 1000f) OnDestroy(); 
//  
//             if (newTarget != Vector3.zero) 
//             { 
//                 transform.right = bestTarget - transform.position; 
//             } 
//         } 
//         #endregion 
//         #region "3D" 
//         protected override void Behave3D() 
//         { 
//             if (Physics.SphereCast(transform.position, projectileBehaviourData.collisionRadius, transform.forward, out RaycastHit hit, speed * Time.deltaTime, sourceSkill.targets)) 
//             { 
//                 Collider other = hit.collider; 
//  
//                 if (((1 << other.gameObject.layer) & projectileBehaviourData.bounceSurfaces) != 0) 
//                 { 
//                     transform.position = hit.point; 
//                     if (bounce == 0) 
//                     { 
//                         OnDestroy(); 
//                     } 
//                     else 
//                     { 
//                         transform.rotation = Quaternion.LookRotation(Vector3.Reflect(transform.forward, hit.normal.normalized), Vector3.up); 
//                         bounce--; 
//                         sourceSkill.onBounce?.Invoke(this, sourceSkill.bounceScalars); 
//                     } 
//                 } 
//                 else if (1 << source.gameObject.layer != 1 << other.gameObject.layer && !hitAlready.Contains(other.gameObject)) 
//                 { 
//                     transform.position = hit.point; 
//                     hitAlready.Add(other.gameObject); 
//  
//                     var charDataHit = other.gameObject.GetComponent<CharacterData>(); 
//  
//                     foreach (BuffSystem.Buff buff in sourceSkill.buffsToApply) 
//                     { 
//                         buff.Clone().Inflict(charDataHit, skillCaster.transform); 
//                     } 
//                     foreach (Damage damage in sourceSkill.damages.List) 
//                     { 
//                         charDataHit.TakeDamage(damage, sourceSkill.accuracy, sourceSkill.critChance, sourceSkill.critMultiplier); 
//                     } 
//  
//                     if (ricochet != 0) 
//                     { 
//                         skillCaster.OnHit(); 
//                         projectileBehaviourData.onHit?.Invoke(); 
//                         InstantiateParticles(); 
//  
//                         RicochetAnotherEnemy3D(1 << other.gameObject.layer); 
//                         ricochet--; 
//                         sourceSkill.onRicochet?.Invoke(this, sourceSkill.ricochetScalars); 
//                     } 
//                     else if (pierce == 0) 
//                     { 
//                         skillCaster.OnHit(); 
//                         projectileBehaviourData.onHit?.Invoke(); 
//                         InstantiateParticles(); 
//  
//                         OnDestroy(); 
//                     } 
//                     else 
//                     { 
//                         skillCaster.OnHit(); 
//                         projectileBehaviourData.onHit?.Invoke(); 
//                         InstantiateParticles(); 
//                         pierce--; 
//                         sourceSkill.onPierce?.Invoke(this, sourceSkill.pierceScalars); 
//                     } 
//                 } 
//                 else 
//                 { 
//                     transform.position += transform.forward * speed * Time.deltaTime; 
//                 } 
//             } 
//             else 
//             { 
//                 transform.position += transform.forward * speed * Time.deltaTime; 
//             } 
//         } 
//         void RicochetAnotherEnemy3D(LayerMask otherLayer) 
//         { 
//             Vector3 newTarget = Vector3.zero; 
//             Collider[] cols = Physics.OverlapSphere(transform.position, 20f, otherLayer); 
//             if (cols.Length == 0) OnDestroy(); 
//  
//             Vector3 bestTarget = transform.forward * 1000f; 
//  
//             foreach (Collider col in cols) 
//             { 
//                 if (hitAlready.Contains(col.gameObject)) continue; 
//  
//                 if (!Physics.Raycast(transform.position, col.transform.position - transform.position, Vector3.Distance(transform.position, col.transform.position), 
//                     projectileBehaviourData.bounceSurfaces)) 
//                 { 
//                     newTarget = col.transform.position - transform.position.CullAxes(Vector3Utilities.Vector3Axis.Y); 
//                     if (newTarget.sqrMagnitude < bestTarget.sqrMagnitude) 
//                     { 
//                         bestTarget = newTarget; 
//                     } 
//                 } 
//             } 
//  
//             if (bestTarget == transform.forward * 1000f) OnDestroy(); 
//  
//             if (newTarget != Vector3.zero) 
//             { 
//                 transform.rotation = Quaternion.LookRotation(bestTarget, Vector3.up); 
//             } 
//         } 
//         #endregion 
//  
//         private void OnDestroy() 
//         { 
//             CancelInvoke("OnDestroy"); 
//             projectileBehaviourData.onDestroy?.Invoke(); 
//             skillCaster.OnEnd(); 
//             this.enabled = false; 
//         } 
//     } 
// } 
