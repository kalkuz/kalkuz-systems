using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KalkuzSystems
{
    public static class Maths
    {
        public static float Remap(float value, float fromMin, float fromMax, float targetMin, float targetMax)
        {
            return Mathf.LerpUnclamped(targetMin, targetMax, InverseLerpUnclamped(fromMin, fromMax, value));
        }

        public static float InverseLerpUnclamped(float a, float b, float value)
        {
            return (value - a) / (b - a);
        }

        public static float NonLinearInterpolation(AnimationCurve interpolator, float a, float b, float t)
        {
            return Mathf.Lerp(a, b, interpolator.Evaluate(t));
        }

        public static float NonLinearInterpolationUnclamped(AnimationCurve interpolator, float a, float b, float t)
        {
            return Mathf.LerpUnclamped(a, b, interpolator.Evaluate(t));
        }

        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }

        public static bool IsOdd(this int number)
        {
            return number % 2 == 1;
        }

        public static float Sum(params float[] numbers)
        {
            return numbers.Sum();
        }
        
        
    }
}
