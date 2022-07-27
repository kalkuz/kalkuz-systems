using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems
{
    [System.Serializable]
    public class Level
    {
        [SerializeField] private int min, max;
        [SerializeField] private AnimationCurve levelingCurve;
        private int current;
        public int Current => Mathf.Clamp(current, min, max);

        [SerializeField] private float experienceCoefficient, minimumRequiredExperience;
        [SerializeField] private AnimationCurve experienceScaling;
        private float currentExperience;
        public float RequiredExperience => experienceScaling.Evaluate(Mathf.InverseLerp(min, max, Current)) * experienceCoefficient + minimumRequiredExperience;

        public UnityEvent onLevelChanged;

        public float GetCurrentScalingValue()
        {
            return 1 + levelingCurve.Evaluate(GetCurrentLevelInterpolator());
        }

        public float GetCurrentLevelInterpolator()
        {
            return Mathf.InverseLerp(min, max, Current);
        }

        public void GainExperience(float amount)
        {
            currentExperience += amount;

            float req = RequiredExperience;
            if (currentExperience >= req)
            {
                if (current < max)
                {
                    current = Mathf.Min(current + 1, max);
                    currentExperience -= req;
                    onLevelChanged?.Invoke();
                }
                else
                {
                    currentExperience = RequiredExperience;
                }
            }
        }
    }

}