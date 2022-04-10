using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.ProceduralGeneration
{
    public class Noise
    {
        public static float[,] NoiseMap(int width, int height, float scale, Wave[] waves, Vector2 offset)
        {
            float[,] noiseMap = new float[width, height];

            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = -height / 2; y < height / 2; y++)
                {
                    float samplePosX = (float)x / scale + offset.x;
                    float samplePosY = (float)y / scale + offset.y;
                    float normalization = 0.0f;

                    foreach (Wave wave in waves)
                    {
                        noiseMap[x + width / 2, y + height / 2] += wave.amplitude * Mathf.PerlinNoise(samplePosX * wave.frequency + wave.seed, samplePosY * wave.frequency + wave.seed);
                        normalization += wave.amplitude;
                    }
                    noiseMap[x + width / 2, y + height / 2] /= normalization;
                }
            }

            return noiseMap;
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int seed;
        public float amplitude;
        public float frequency;

        public void RandomizeSeed()
        {
            seed = new System.Random(Random.Range(int.MinValue, int.MaxValue)).Next(-100000, 100000);
        }
    }
}
