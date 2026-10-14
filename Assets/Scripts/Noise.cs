using UnityEngine;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    public static class Noise
    {
        private const int OFFSET_MAX_VALUE = 100000;
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
        /// <param name="pOctave">Number of Layer</param>
        /// <param name="pPersistance">Amplitude of the Wave (if 2 noise go from -1 2)</param>
        /// <param name="pLacunarity">Frequency of the Wave</param>
        /// <returns></returns>
        public static float[,] GenerateNoiseMap(int pMapWidth, int pMapHeight,
        int pSeed, float pScale, int pOctave, float pPersistance, float pLacunarity,
        Vector2 pOffset)
        {
            #region Variables
            float[,] lNoiseMap = new float[pMapWidth, pMapHeight];

            // Seed Parameter
            System.Random lRng = new System.Random(pSeed);
            Vector2[] lOctaveOffsets = new Vector2[pOctave];
            float lOffsetX, lOffsetY;

            float lMaxNoiseHeight = float.MinValue;
            float lMinNoiseHeight = float.MaxValue;

            float lHalfWidth = pMapWidth / 2f;
            float lHalfHeight = pMapHeight / 2f;

            float lSampleX, lSampleY, lPerlinValue, lAmplitude, lFrequency, lNoiseHeight;
            #endregion
            #region Logic
            // Seed Offset Creation
            for (int lIndex = 0; lIndex < pOctave; lIndex++)
            {
                // Create Seed based Random For Octave Offset Position
                lOffsetX = lRng.Next(-OFFSET_MAX_VALUE, OFFSET_MAX_VALUE) + pOffset.x;
                lOffsetY = lRng.Next(-OFFSET_MAX_VALUE, OFFSET_MAX_VALUE) + pOffset.y;
                lOctaveOffsets[lIndex] = new Vector2(lOffsetX, lOffsetY);
            }

            // Ensure Scale is Bigger than 0
            if (pScale <= 0) pScale = 0.001f;

            // Loop in Every Pixel of the Noise
            for (int y = 0; y < pMapHeight; y++)
                for (int x = 0; x < pMapWidth; x++)
                {
                    // Reset Value For All New Line
                    lAmplitude = 1;
                    lFrequency = 1;
                    lNoiseHeight = 0;

                    // Loop For Octave To Add New Perlin Noise Layer
                    for (int lIndex = 0; lIndex < pOctave; lIndex++)
                    {
                        lSampleX = (x - lHalfWidth) / pScale * lFrequency + lOctaveOffsets[lIndex].x;
                        lSampleY = (y - lHalfHeight) / pScale * lFrequency + lOctaveOffsets[lIndex].y;

                        // Make Values Goes from 0;1 to -1;1
                        lPerlinValue = Mathf.PerlinNoise(lSampleX, lSampleY) * 2 - 1;
                        // Add Amplitude to Noise Height
                        lNoiseHeight += lPerlinValue * lAmplitude;

                        // Increment Amplitude & Frequency each Loop
                        lAmplitude *= pPersistance;
                        lFrequency *= pLacunarity;
                    }
                    // Update Min & Max Noise Height Value
                    if (lNoiseHeight > lMaxNoiseHeight)
                        lMaxNoiseHeight = lNoiseHeight;
                    else if (lNoiseHeight < lMinNoiseHeight)
                        lMinNoiseHeight = lNoiseHeight;
                    // Update Noise Map w New Height Value
                    lNoiseMap[x, y] = lNoiseHeight;
                }

            // Noise Map Return Value greater than 0 & 1 so we clamp Them With a Loop
            for (int y = 0; y < pMapHeight; y++)
                for (int x = 0; x < pMapWidth; x++)
                    lNoiseMap[x, y] = Mathf.InverseLerp(lMinNoiseHeight, lMaxNoiseHeight, lNoiseMap[x, y]);

            return lNoiseMap;
            #endregion
        }
    }
}

