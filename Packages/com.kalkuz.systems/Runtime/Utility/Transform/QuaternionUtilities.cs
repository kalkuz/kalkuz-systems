using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Utility.Transform
{
    public static class QuaternionUtilities
    {
        public static IEnumerator RotateTowards(this UnityEngine.Transform transform, Vector3 forward, Vector3 up, float duration)
        {
            Quaternion startingRotation = transform.rotation;
            Quaternion lookRotation = Quaternion.LookRotation(forward, up);

            float timePassed = 0f;
            while (timePassed < duration)
            {
                transform.rotation = Quaternion.Slerp(startingRotation, lookRotation, timePassed / duration);
                yield return null;
                timePassed += Time.deltaTime;
            }
            transform.rotation = lookRotation;
        }
    }
}