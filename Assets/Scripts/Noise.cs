using UnityEngine;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    public static class Noise
    {
        public static float[,] GenerateNoiseMap(int pMapWidth, int pMapHeight, float pScale)
        {
            float[,] lNoiseMap = new float[pMapWidth, pMapHeight];

            // Ensure Scale is Bigger than 0
            if (pScale <= 0) pScale = 0.001f;

            // Creates Loop Variables
            float lSampleX, lSampleY, lPerlinValue;
            for (int y = 0; y < pMapHeight; y++)
                for (int x = 0; x < pMapWidth; x++)
                {
                    lSampleX = x / pScale;
                    lSampleY = y / pScale;

                    lPerlinValue = Mathf.PerlinNoise(lSampleX, lSampleY);
                    lPerlinValue = Mathf.Clamp01(lPerlinValue);
                    lNoiseMap[x, y] = lPerlinValue;
                }
            return lNoiseMap;
        }
    }
}
