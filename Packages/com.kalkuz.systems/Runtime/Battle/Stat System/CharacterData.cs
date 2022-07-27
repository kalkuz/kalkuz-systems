using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle.BuffSystem;
using KalkuzSystems.Indication.Highlighters;
using KalkuzSystems.UI.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Battle
{
    /// <summary>
    /// Container for both data and logic for a Character's properties.
    /// </summary>
    public sealed class CharacterData : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Determines how frequently the update thread should be.
        /// </summary>
        private const float TICK_RATE = 1 / 2f;

        /// <summary>
        /// Determines whether or not the script should start itself automatically.
        /// </summary>
        [Tooltip("Should the script start automatically?")] 
        [SerializeField] private bool initializeOnStart;
        
        /// <summary>
        /// Name of the entity which may be used for UI texts.
        /// </summary>
        [Tooltip("Name of the entity.")] 
        [SerializeField] private string characterName;

        [SerializeField] private List<HostilityTag> tags;

        /// <summary>
        /// The system that manages the stats of the character.
        /// </summary>
        [Tooltip("The container for stats of the character.")]
        [SerializeField] private StatSystem stats;

        /// <summary>
        /// The UI element that is used to indicate health.
        /// </summary>
        [Header("References")]
        [SerializeField] private GameObject healthBar;

        /// <summary>
        /// The highlighter script responsible for object's various actions.
        /// </summary>
        [SerializeField] private Highlighter highlighter;

        [Header("Events")]
        [SerializeField] private UnityEvent<CharacterData> onDeath;
        [SerializeField] private UnityEvent<CharacterData> onSizeChanged;
        [SerializeField] private UnityEvent<CharacterData, DamageType> onResistanceChanged;
        [SerializeField] private UnityEvent<CharacterData, ResourceType> onResourceChanged;

        #endregion

        #region Properties

        /// <inheritdoc cref="characterName"/>
        public string CharacterName => characterName;

        public List<HostilityTag> Tags => tags;
        
        /// <inheritdoc cref="stats"/>
        public StatSystem Stats => stats;

        /// <inheritdoc cref="highlighter"/>
        public Highlighter Highlighter => highlighter;
        
        /// <summary>
        /// Event that will cover the characters death condition.
        /// </summary>
        public UnityEvent<CharacterData> OnDeath => onDeath;
        
        /// <summary>
        /// Event that follows any change in the character's size.
        /// </summary>
        public UnityEvent<CharacterData> OnSizeChanged => onSizeChanged;
        
        /// <summary>
        /// Event that follows any change in the character's resistances.
        /// </summary>
        public UnityEvent<CharacterData, DamageType> OnResistancesChanged => onResistanceChanged;
        
        /// <summary>
        /// Event that follows any change in the character's resources.
        /// </summary>
        public UnityEvent<CharacterData, ResourceType> OnResourceChanged => onResourceChanged;

        #endregion
        
        private void Start()
        {
            if (initializeOnStart) Initialize();
        }

        /// <summary>
        /// Initializes the connected props that are belong to Character Data object.
        /// </summary>
        public void Initialize()
        {
            Stats.Initialize(this);

            if (healthBar)
            {
                var instantiatedHPBar = Instantiate(healthBar);
                var healthBarComponent = instantiatedHPBar.GetComponent<HealthBar>();
                healthBarComponent.Initialize(this);
            }

            StartCoroutine(Tick());

            Stats.Level.onLevelChanged.AddListener(() =>
            {
                ResourceContainer hp = Stats.CurrentStats.GetResource(ResourceType.HEALTH);
                hp.Amount += hp.IncreasePerLevelScalar * hp.IncreaseCurve.Evaluate(Stats.Level.GetCurrentLevelInterpolator());
            });
        }

        IEnumerator Tick()
        {
            while (true)
            {
                yield return new WaitForSeconds(TICK_RATE);
                Stats.Tick(TICK_RATE);
            }
        }

        public void TakeDamage(Damage damage, float accuracy, float critChance, float critMultiplier)
        {
            Stats.TakeDamage(damage, accuracy, critChance, critMultiplier);
        }

        public void Die()
        {
            if (TryGetComponent<CapsuleCollider>(out var capsuleCollider))
            {
                capsuleCollider.enabled = false;
            }
            if (TryGetComponent<CharacterController>(out var characterController))
            {
                characterController.enabled = false;
            }
            if (TryGetComponent<UnityEngine.AI.NavMeshAgent>(out var navMeshAgent))
            {
                navMeshAgent.enabled = false;
            }

            OnDeath?.Invoke(this);
            StopAllCoroutines();

            this.enabled = false;
        }

        public bool CanMove()
        {
            foreach (StatusEffect item in StatusEffectHelpers.movementPreventers)
            {
                if (Stats.buffs.ContainsKey(item))
                {
                    return false;
                }
            }
            return true;
        }
    }

}