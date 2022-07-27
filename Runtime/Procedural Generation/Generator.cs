using System.Collections;
using System.Collections.Generic;
using System.Threading;
using KalkuzSystems.Analysis;
using UnityEngine;

namespace KalkuzSystems.ProceduralGeneration
{
    public abstract class Generator : MonoBehaviour
    {
        public MeshRenderer quad;
        [Space]
        [Min(1)]
        public Vector2Int mapDimensions;

        [Min(0.001f)]
        public float scale;
        public Vector2 offset;

        [Header("Chunks")]
        public int chunkSize;

        [Header("Noise Waves")]
        public Wave[] waves;

        [Header("Smoothing")]
        public int smoothIterations;

        public abstract void Generate();

        public static void Smooth(bool[,] map, int iterations)
        {
            if (map.GetLength(0) < 3 || map.GetLength(1) < 3) return;

            for (int iter = 0; iter < iterations; iter++)
            {
                for (int x = 1; x < map.GetLength(0) - 1; x++)
                {
                    for (int y = 1; y < map.GetLength(1) - 1; y++)
                    {
                        map[x, y] = CellularAutomata(map, x, y);
                    }
                }
            }
        }

        public static bool CellularAutomata(bool[,] map, int x, int y, int neighbourCount = 4)
        {
            int count = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y) continue;

                    if (map[i, j]) count++;
                }
            }

            if (count >= neighbourCount) return true;
            else return false;
        }
    }
}

