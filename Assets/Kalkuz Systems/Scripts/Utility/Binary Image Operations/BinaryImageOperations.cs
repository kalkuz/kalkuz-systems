using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.Utility.BinaryImage
{
    public static class BinaryImageOperations
    {
        static Vector2[] vectors = new Vector2[] { Vector2.down, Vector2.up, Vector2.left, Vector2.right };
        static void SetNeighbours(this bool[,] map, bool value, int x, int y, int maxX, int maxY)
        {
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y) continue;
                    if (i < 0 || i >= maxX || j < 0 || j >= maxY) continue;
                    map[i, j] = value;
                }
            }
        }
        static void SetNeighboursV2(this bool[,] map, bool value, int x, int y, int maxX, int maxY)
        {
            foreach (Vector2 vector in vectors)
            {
                int i = x + (int)vector.x;
                int j = y + (int)vector.y;
                if (i == x && j == y) continue;
                if (i < 0 || i >= maxX || j < 0 || j >= maxY) continue;
                map[i, j] = value;
            }

        }
        public static void Dilation(this bool[,] map, int dilationCount)
        {
            int maxX = map.GetLength(0);
            int maxY = map.GetLength(1);
            for (int i = 0; i < dilationCount; i++)
            {
                bool[,] result = map.Clone() as bool[,];

                for (int x = 0; x < maxX; x++)
                {
                    for (int y = 0; y < maxY; y++)
                    {
                        if (result[x, y] == true) map.SetNeighbours(true, x, y, maxX, maxY);
                    }
                }
            }
        }

        public static void Erosion(this bool[,] map, int erosionCount)
        {
            int maxX = map.GetLength(0);
            int maxY = map.GetLength(1);
            for (int i = 0; i < erosionCount; i++)
            {
                bool[,] result = map.Clone() as bool[,];

                for (int x = 0; x < maxX; x++)
                {
                    for (int y = 0; y < maxY; y++)
                    {
                        if (result[x, y] == false) map.SetNeighbours(false, x, y, maxX, maxY);
                    }
                }
            }
        }

        public static void Opening(this bool[,] map, int openingIterations)
        {
            map.Dilation(openingIterations);
            map.Erosion(openingIterations);
        }
        public static void Closing(this bool[,] map, int closingIterations)
        {
            map.Erosion(closingIterations);
            map.Dilation(closingIterations);
        }

        public static bool[,] ConvertToBinaryMap(this float[,] map, float threshold)
        {
            bool[,] binaryMap = new bool[map.GetLength(0), map.GetLength(1)];

            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    binaryMap[x, y] = map[x, y] < threshold ? false : true;
                }
            }

            return binaryMap;
        }
    }
}