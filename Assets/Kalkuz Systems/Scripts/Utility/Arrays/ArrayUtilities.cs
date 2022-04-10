using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Utility.Arrays
{
    public static class ArrayUtilities
    {
        #region "1D arrays"
        public static bool ContainsIndex<T>(this T[] array, int index)
        {
            return index >= 0 && index < array.GetLength(0);
        }
        #endregion

        #region "2D arrays"
        public static bool ContainsIndices<T>(this T[,] array, int x, int y)
        {
            return x >= 0 && x < array.GetLength(0) && y >= 0 && y < array.GetLength(1);
        }
        #endregion
    }
}
