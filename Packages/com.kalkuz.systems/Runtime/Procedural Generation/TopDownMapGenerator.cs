using System.Collections;
using System.Collections.Generic;
using KalkuzSystems.Analysis;
using UnityEngine;

namespace KalkuzSystems.ProceduralGeneration
{
    public class TopDownMapGenerator : Generator
    {
        public override void Generate()
        {
            Benchmark bm = new Benchmark();

            Texture2D mapTexture = new Texture2D(mapDimensions.x, mapDimensions.y);
            Color[] pixels = new Color[mapDimensions.x * mapDimensions.y];

            var noiseMap = Noise.NoiseMap(mapDimensions.x, mapDimensions.y, scale, waves, offset);
            for (int x = 0; x < mapDimensions.x; x++)
            {
                for (int y = 0; y < mapDimensions.y; y++)
                {
                    pixels[x * mapDimensions.y + y] = Color.Lerp(Color.white, Color.black, noiseMap[x, y]);
                }
            }
            mapTexture.SetPixels(pixels);
            mapTexture.Apply();

            quad.sharedMaterial.mainTexture = mapTexture;

            bm.EndBenchmark();
        }
    }
}
