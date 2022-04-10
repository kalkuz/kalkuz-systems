using System.Collections;
using KalkuzSystems.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace KalkuzSystems.UI.Resources
{
    /// <summary>
    /// Used to indicate <see cref="CharacterData"/>'s resources in UI elements. 
    /// </summary>
    public abstract class ResourceBar : MonoBehaviour
    {
        [Header("Binding")]
        [SerializeField] protected CharacterData bondCharacter;
        [SerializeField] protected bool isStatic;
        [SerializeField] protected Vector3 followOffset;

        [Header("Invocation")]
        [SerializeField] protected bool autoStart;

        [Header("Properties")] 
        [SerializeField, Min(1)] protected int updateIterationMax;

        [SerializeField, Min(0)] protected float timeBetweenIterations;

        protected Coroutine slideProcedure;
        
        public abstract void Initialize(CharacterData characterData, Vector3 followOffset);

        public void Initialize(CharacterData characterData)
        {
            Initialize(characterData, followOffset);
        }
        
        protected void Start()
        {
            if (autoStart && bondCharacter)
            {
                Initialize(null, Vector3.zero);
            }
        }
        
        private void Update()
        {
            if (!isStatic && bondCharacter != null)
            {
                transform.position = Camera.main.WorldToScreenPoint(bondCharacter.transform.position + followOffset);
            }
        }

        public abstract void UpdateBar(CharacterData charData, ResourceType resourceType);

        protected abstract IEnumerator SlideProcedure(float percent);
    }
}