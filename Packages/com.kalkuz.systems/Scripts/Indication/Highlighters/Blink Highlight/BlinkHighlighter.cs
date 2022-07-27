using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Indication.Highlighters
{
    /// <summary>
    /// Base class for blink indication of objects.
    /// </summary>
    public abstract class BlinkHighlighter : Highlighter
    {
        [Header("Properties")]
        [SerializeField] protected AnimationCurve easeCurve;
        [SerializeField] protected float blinkDuration;
        [SerializeField] protected bool isLooping;
        [SerializeField] protected bool playOnAwake;
        [ColorUsage(showAlpha: true, hdr: true), SerializeField] protected Color targetColor = Color.white;
    }
}
