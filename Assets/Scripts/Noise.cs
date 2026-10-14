using UnityEngine;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    public static class Noise
    {
        /// <summary>
        /// First Noise Map Ultra Basic No Change Paramter on the Noise
        /// </summary>
        /// <param name="pMapWidth"></param>
        /// <param name="pMapHeight"></param>
        /// <param name="pScale"></param>
        /// <returns></returns>
        public static float[,] GenerateSimpleNoiseMap(int pMapWidth, int pMapHeight, float pScale)
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

        /// <summary>
        /// More Complex Noise Creation
        /// </summary>
        /// <param name="pMapWidth">Width in Pixel</param>
        /// <param name="pMapHeight">Height in Pixel</param>
        /// <param name="pSeed">to generate Same Map with Same Seed</param>
        /// <param name="pScale">Zoom of the Noise</param>
        /// <param name="pOctave">Number of Time Persistance and Lacunarity are Repeated for Each Pixel</param>
        /// <param name="pPersistance">Amplitude of the Wave (if 2 noise go from -1 2)</param>
        /// <param name="pLacunarity">Frequency of the Wave</param>
        /// <returns></returns>
        public static float[,] GenerateNoiseMap(int pMapWidth, int pMapHeight,
        int pSeed, float pScale, int pOctave, float pPersistance, float pLacunarity,
        Vector2 pOffset)
        {
            float[,] lNoiseMap = new float[pMapWidth, pMapHeight];

            // Seed Parameter
            System.Random lRng = new System.Random(pSeed);
            Vector2[] lOctaveOffsets = new Vector2[pOctave];
            for (int i = 0; i < pOctave; i++)
            {
                float lOffsetX = lRng.Next(-100000, 100000) + pOffset.x;
                float lOffsetY = lRng.Next(-100000, 100000) + pOffset.y;
                lOctaveOffsets[i] = new Vector2(lOffsetX, lOffsetY);
            }

            // Ensure Scale is Bigger than 0
            if (pScale <= 0) pScale = 0.001f;

            float lMaxNoiseHeight = float.MinValue;
            float lMinNoiseHeight = float.MaxValue;

            float lHalfWidth = pMapWidth / 2f;
            float lHalfHeight = pMapHeight / 2f;

            // Creates Loop Variables
            float lSampleX, lSampleY, lPerlinValue, lAmplitude, lFrequency, lNoiseHeight;
            for (int y = 0; y < pMapHeight; y++)
                for (int x = 0; x < pMapWidth; x++)
                {
                    lAmplitude = 1;
                    lFrequency = 1;
                    lNoiseHeight = 0;

                    for (int i = 0; i < pOctave; i++)
                    {
                        lSampleX = (x - lHalfWidth) / pScale * lFrequency + lOctaveOffsets[i].x;
                        lSampleY = (y - lHalfHeight) / pScale * lFrequency + lOctaveOffsets[i].y;

                        lPerlinValue = Mathf.PerlinNoise(lSampleX, lSampleY) * 2 - 1;
                        lNoiseHeight += lPerlinValue * lAmplitude;

                        lAmplitude *= pPersistance;
                        lFrequency *= pLacunarity;
                    }
                    if (lNoiseHeight > lMaxNoiseHeight)
                        lMaxNoiseHeight = lNoiseHeight;
                    else if (lNoiseHeight < lMinNoiseHeight)
                        lMinNoiseHeight = lNoiseHeight;
                    lNoiseMap[x, y] = lNoiseHeight;
                }

            // Noise Map Return Value greater than 0 & 1 so we clamp Them With a Loop
            for (int y = 0; y < pMapHeight; y++)
                for (int x = 0; x < pMapWidth; x++)
                    lNoiseMap[x, y] = Mathf.InverseLerp(lMinNoiseHeight, lMaxNoiseHeight, lNoiseMap[x, y]);

            return lNoiseMap;
        }
    }
}

