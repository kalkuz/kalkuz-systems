using System;
using System.Collections.Generic;
using KalkuzSystems.Attributes;
using KalkuzSystems.DataStructures.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KalkuzSystems.Battle.SkillSystem
{
    [CreateAssetMenu(menuName = "Kalkuz Systems/Skill/Projectile Skill")]
    public sealed class ProjectileSkillData : SkillData
    {
        #region Fields

        /// <summary>
        /// Projectile prefab that will be instantiated.
        /// </summary>
        [Title("References"), LineSeparator]
        [SerializeField] private Projectile projectilePrefab;
        
        /// <summary>
        /// Type of the projectile which affects behaviour.
        /// </summary>
        [Title("Attributes"), LineSeparator]
        [SerializeField] private ProjectileType projectileType;

        [SerializeField] private int burstCount = 1;
        
        /// <summary>
        /// Projectile's moving speed.
        /// If orbital; x value is rotational speed in terms of frequency and if negative rotates to opposite direction.
        /// If orbital; y value is the lower bound and z value is upper bound limit of the rotation.
        /// Please note that this is a global vector of the speed.
        /// </summary>
        [SerializeField] private Vector3 speed;
        
        /// <summary>
        /// Projectile's speed increase over time.
        /// Please note that this is a global vector of the acceleration.
        /// </summary>
        [SerializeField] private Vector3 acceleration;
        
        /// <summary>
        /// Projectile's acceleration increase over time.
        /// Please note that this is a global vector of the acceleration increase.
        /// </summary>
        [SerializeField] private Vector3 accelerationIncrease;
        
        [LineSeparator]
        [SerializeField] private int pierce;
        [SerializeField] private int ricochet;
        [SerializeField] private int bounce;

        [Title("Shape")]
        [SerializeField] private float diagonalSpreadAngle = 0f;
        
        /// <summary>
        /// Minimum and maximum of the orbital radius. This value is randomly set between these two.
        /// </summary>
        [Title("Orbital")]
        [SerializeField] private Vector2 orbitalRadius;

        [SerializeField] private bool randomizeOrbitalDirection;

        /// <summary>
        /// Projectile lifetime in seconds.
        /// </summary>
        [Title("Limits"), LineSeparator]
        [SerializeField] private float lifetime;
        
        /// <summary>
        /// Maximum distance which the projectile can travel from its source.
        /// </summary>
        [SerializeField] private float maxDistanceFromSource;
        
        #endregion

        #region Properties

        /// <inheritdoc cref="projectilePrefab"/>
        public Projectile ProjectilePrefab => projectilePrefab;

        /// <inheritdoc cref="projectileType"/>
        public ProjectileType ProjectileType => projectileType;

        public int BurstCount => burstCount;
        
        /// <inheritdoc cref="speed"/>
        public Vector3 Speed => speed;
        
        /// <inheritdoc cref="acceleration"/>
        public Vector3 Acceleration => acceleration;
        
        /// <inheritdoc cref="accelerationIncrease"/>
        public Vector3 AccelerationIncrease => accelerationIncrease;

        public int Pierce => pierce;
        public int Ricochet => ricochet;
        public int Bounce => bounce;

        public Vector2 OrbitalRadius => orbitalRadius;
        public bool RandomizeOrbitalDirection => randomizeOrbitalDirection;

        /// <inheritdoc cref="lifetime"/>
        public float Lifetime => lifetime;
        
        /// <inheritdoc cref="maxDistanceFromSource"/>
        public float MaxDistanceFromSource => maxDistanceFromSource;

        #endregion

        public override void Cast(Vector3 position, Vector3 direction, CharacterData targetCharacter, Transform source)
        {
            var isOrbital = projectileType == ProjectileType.ORBITAL;
            var angleOffset = (isOrbital ? 360f : diagonalSpreadAngle) / burstCount;
            
            for (int i = 0; i < burstCount; i++)
            {
                if (!TryGetPooledProjectile(out var projectile)) continue;

                if (isOrbital)
                {
                    Vector3 posVector, dirVector;
                    if (is2D)
                    {
                        posVector = Quaternion.AngleAxis(i * angleOffset, Vector3.forward) * Vector3.right;
                        dirVector = Vector3.Cross(Vector3.back, posVector);
                    }
                    else
                    {
                        posVector = Quaternion.AngleAxis(i * angleOffset, Vector3.up) * Vector3.forward;
                        dirVector = Vector3.Cross(Vector3.up, posVector);
                    }
                    
                    posVector *= Random.Range(orbitalRadius.x, orbitalRadius.y);
                    
                    projectile.Initialize(this, source, position + posVector, dirVector);
                }
                else
                {
                    projectile.Initialize(this, source, position, direction);
                }
            }
        }

        private bool TryGetPooledProjectile(out Projectile pooledProjectile)
        {
            if (projectilePrefab == null)
            {
                pooledProjectile = null;
                return false;
            }

            var poolComponent = projectilePrefab.GetComponent<PoolObject>();
            var pool = UniversalPoolProvider.GetPool(poolComponent.ID);
            var objectFromPool = pool.Request(poolComponent);

            pooledProjectile = objectFromPool.GetComponent<Projectile>();
            return true;
        }
    }
}