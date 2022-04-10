using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Indication.Highlighters
{
    /// <summary>
    /// Used to indicate a <see cref="SpriteRenderer"/>.
    /// </summary>
    public class SpriteBlinkHighlighter : BlinkHighlighter
    {
        [Header("Renderer")]
        public SpriteRenderer spriteRenderer;

        private Color startColor;

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            startColor = spriteRenderer.color;

            if (playOnAwake) Highlight();
        }

        protected override IEnumerator HighlightProcedure()
        {
            float timePassed = 0f;
            while (timePassed < blinkDuration || isLooping)
            {
                spriteRenderer.color = Color.Lerp(startColor, targetColor, easeCurve.Evaluate(Mathf.PingPong(timePassed / blinkDuration, 1)));
                timePassed += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = startColor;
        }
    }
}
