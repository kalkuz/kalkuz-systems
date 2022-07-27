using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KalkuzSystems.Utility.Transform
{
    public static class Vector3Utilities
    {
        public enum Vector3Axis { X, Y, Z }
        public enum RestrainAxes { X, Y, Z, XY, XZ, YZ, XYZ }

        public static Vector3 XY2XZ(this Vector3 vector)
        {
            return new Vector3(vector.x, vector.z, vector.y);
        }
        public static Vector3 XY2XZ(this Vector3 vector, float newY)
        {
            return new Vector3(vector.x, newY, vector.y);
        }

        public static Vector3 XY2YZ(this Vector3 vector)
        {
            return new Vector3(vector.z, vector.y, vector.x);
        }
        public static Vector3 XY2YZ(this Vector3 vector, float newX)
        {
            return new Vector3(newX, vector.y, vector.x);
        }

        public static Vector3 CullAxes(this Vector3 vector, params Vector3Axis[] axes)
        {
            var axesList = axes.ToList();
            Vector3 result = Vector3.zero;

            if (!axesList.Contains(Vector3Axis.X)) result.x = vector.x;
            if (!axesList.Contains(Vector3Axis.Y)) result.y = vector.y;
            if (!axesList.Contains(Vector3Axis.Z)) result.z = vector.z;

            return result;
        }
        public static Vector3 Vector2ToVector3(this Vector2 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }
    }
}
