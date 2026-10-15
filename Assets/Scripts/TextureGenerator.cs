using UnityEngine;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    public static class TextureGenerator
    {
        public static Texture2D TextureFromColor(Color[] pColorMap, int pMapWidth, int pMapHeight)
        {
            Texture2D lTexture = new Texture2D(pMapWidth, pMapHeight);
            lTexture.SetPixels(pColorMap);
            lTexture.Apply();
            return lTexture;
        }

        public static Texture2D TextureFromHeightMap(float[,] pHeightMap)
        {
            int lWidth = pHeightMap.GetLength(0);
            int lHeight = pHeightMap.GetLength(1);

            // Place Noise Color into Pixel Emplacement
            Color[] lColorMap = new Color[lWidth * lHeight];
            for (int y = 0; y < lHeight; y++)
                for (int x = 0; x < lWidth; x++)
                    lColorMap[y * lWidth + x] = Color.Lerp(Color.black, Color.white, pHeightMap[x, y]);

            return TextureFromColor(lColorMap, lWidth, lHeight);
        }
    }
}