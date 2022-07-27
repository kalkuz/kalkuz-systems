using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Utility.Gameplay
{
    /// <summary>
    /// Used to improve feeling of hits by stopping the motion a little bit
    /// </summary>
    public static class HitStop
    {
        private static bool m_timeSlowed = false;

        /// <summary>
        /// USed to apply Hit Stop. Beware of Time Scale while using
        /// </summary>
        /// <param name="caller">The instance that requested HitStop</param>
        /// <param name="timeScaleMultiplier">The change in the time scale. 0.25f slows the time to its quarter</param>
        /// <param name="duration">The duration before returning to original value</param>
        public static void Apply(MonoBehaviour caller, float timeScaleMultiplier, float duration)
        {
            if (m_timeSlowed) return;
            caller.StartCoroutine(HitStopCoroutine(timeScaleMultiplier, duration));
        }

        static IEnumerator HitStopCoroutine(float timeScaleMultiplier, float duration)
        {
            if (timeScaleMultiplier < 0f) timeScaleMultiplier = 0.001f;
            
            m_timeSlowed = true;
            Time.timeScale *= timeScaleMultiplier;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale /= timeScaleMultiplier;
            m_timeSlowed = false;
        }
    }
}
