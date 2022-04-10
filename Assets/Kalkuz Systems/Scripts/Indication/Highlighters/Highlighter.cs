using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Indication.Highlighters
{
    /// <summary>
    /// Base class for indicating objects.
    /// </summary>
    public abstract class Highlighter : MonoBehaviour
    {
        protected Coroutine highlightCoroutine;
        
        public virtual void Highlight()
        {
            if (highlightCoroutine != null) StopCoroutine(highlightCoroutine);
            highlightCoroutine = StartCoroutine(HighlightProcedure());
        }
        protected abstract IEnumerator HighlightProcedure();
    }
}
