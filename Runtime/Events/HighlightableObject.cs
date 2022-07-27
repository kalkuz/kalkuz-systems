using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Battle;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.Events
{
    [RequireComponent(typeof(Collider))]
    public class HighlightableObject : MonoBehaviour
    {
        public UnityEvent onMouseEnter, onMouseExit;

        private void OnMouseEnter()
        {
            onMouseEnter?.Invoke();
        }

        private void OnMouseExit()
        {
            onMouseExit?.Invoke();
        }
    }
}
