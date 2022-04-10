using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Indication.Highlighters
{
    /// <summary>
    /// Used to indicate a <see cref="SkinnedMeshRenderer"/>. Note that emission of the material should be reachable.
    /// </summary>
    public class SkinnedMeshBlinkHighlighter : BlinkHighlighter
    {
        [Header("Renderer")]
        public SkinnedMeshRenderer meshRenderer;

        private Color startColor;

        private void Awake()
        {
            if (meshRenderer == null) meshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        private void Start()
        {
            startColor = meshRenderer.material.GetColor("_EmissionColor");

            if (playOnAwake) Highlight();
        }

        protected override IEnumerator HighlightProcedure()
        {
            float timePassed = 0f;
            while (timePassed < blinkDuration || isLooping)
            {
                meshRenderer.material.SetColor("_EmissionColor", Color.Lerp(startColor, targetColor, easeCurve.Evaluate(Mathf.PingPong(timePassed / blinkDuration, 1))));
                timePassed += Time.deltaTime;
                yield return null;
            }

            meshRenderer.material.SetColor("_EmissionColor", startColor);
        }
    }

}