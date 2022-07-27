using System.Collections;
using System.Collections.Generic;
using System.Threading;
using KalkuzSystems.Analysis;
using UnityEngine;
using UnityEngine.Events;

namespace KalkuzSystems.ProceduralGeneration
{
    public class SideViewMapGenerator : Generator
    {
        [Header("Side View Specific")]
        [Range(0, 1)]
        public float threshold;
        public bool useThread;

        public Wave[] caveWaves;

        Queue<UnityAction> threadActionQueue = new Queue<UnityAction>();
        Color[] pixelColors;
        public Color ground, wall, sky;
        private void Update()
        {
            if (threadActionQueue.Count > 0)
            {
                for (int i = 0; i < threadActionQueue.Count; i++)
                {
                    threadActionQueue.Dequeue()?.Invoke();
                }
            }
        }

        public override void Generate()
        {
            quad.enabled = false;
            if (useThread) ThreadedGenerate();
            else
            {
                pixelColors = GenerateColorMap();
                ApplyTexture();
            }
        }

        void ThreadedGenerate()
        {
            new Thread(() =>
                {
                    pixelColors = GenerateColorMap();
                    lock (threadActionQueue)
                    {
                        threadActionQueue.Enqueue(ApplyTexture);
                    }
                }).Start();
        }

        void ApplyTexture()
        {
            quad.transform.localScale = new Vector3(10f * mapDimensions.x / mapDimensions.y, 10f * mapDimensions.y / mapDimensions.y, 0f) + Vector3.forward;
            Texture2D mapTexture = new Texture2D(mapDimensions.x, mapDimensions.y);

            mapTexture.filterMode = FilterMode.Point;
            mapTexture.SetPixels(pixelColors);
            mapTexture.Apply();

            quad.sharedMaterial.mainTexture = mapTexture;
            quad.enabled = true;
        }

        Color[] GenerateColorMap()
        {
            Color[] pixels = new Color[mapDimensions.x * mapDimensions.y];

            var noiseMap = Noise.NoiseMap(mapDimensions.x, mapDimensions.y, scale, waves, offset);
            var caves = Noise.NoiseMap(mapDimensions.x, mapDimensions.y, scale, caveWaves, offset);

            for (int x = 0; x < mapDimensions.x; x++)
            {
                for (int y = 0; y < mapDimensions.y; y++)
                {
                    if (y <= mapDimensions.y * noiseMap[x, 0])
                    {
                        pixels[x + y * mapDimensions.x] = noiseMap[x, 0] * caves[x, y] < threshold ? wall : ground;
                    }
                    else
                    {
                        pixels[x + y * mapDimensions.x] = sky;
                    }
                }
            }

            return pixels;
        }
    }
}
