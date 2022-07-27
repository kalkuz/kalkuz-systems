using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Analysis;
using KalkuzSystems.Utility.BinaryImage;
using UnityEngine;

namespace KalkuzSystems.ProceduralGeneration
{
    public class CaveGenerator : Generator
    {
        [Header("Caves")]
        [Range(0, 1)]
        public float threshold;
        public int openingCount;

        public override void Generate()
        {
            Benchmark bm = new Benchmark();

            quad.transform.localScale = new Vector3(10f * mapDimensions.x / mapDimensions.y, 10f * mapDimensions.y / mapDimensions.y, 0f) + Vector3.forward;

            Texture2D mapTexture = new Texture2D(mapDimensions.x, mapDimensions.y);
            Color[] pixels = new Color[mapDimensions.x * mapDimensions.y];

            var noiseMap = Noise.NoiseMap(mapDimensions.x, mapDimensions.y, scale, waves, offset);
            var binaryMap = noiseMap.ConvertToBinaryMap(threshold);

            Generator.Smooth(binaryMap, smoothIterations);

            binaryMap.Closing(openingCount);

            for (int x = 0; x < mapDimensions.x; x++)
            {
                for (int y = 0; y < mapDimensions.y; y++)
                {
                    pixels[x + y * mapDimensions.x] = binaryMap[x, y] ? Color.black : Color.gray;
                }
            }
            mapTexture.filterMode = FilterMode.Point;
            mapTexture.SetPixels(pixels);
            mapTexture.Apply();

            quad.sharedMaterial.mainTexture = mapTexture;

            bm.EndBenchmark();
        }
    }
}
