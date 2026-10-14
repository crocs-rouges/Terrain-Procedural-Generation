using UnityEngine;
using System;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    public class MapDislplay : MonoBehaviour
    {
        [SerializeField] private Renderer textureRenderer;
        public void DrawNoiseMap(float[,] pNoiseMap)
        {
            // Create New Texture
            int lWidth = pNoiseMap.GetLength(0);
            int lHeight = pNoiseMap.GetLength(1);
            Texture2D lTexture = new Texture2D(lWidth, lHeight);

            // Place Noise Color into Pixel Emplacement
            Color[] lColorMap = new Color[lWidth * lHeight];
            for (int y = 0; y < lHeight; y++)
                for (int x = 0; x < lWidth; x++)
                    lColorMap[y * lWidth + x] = Color.Lerp(Color.black, Color.white, pNoiseMap[x, y]);

            // Apply Noise Color to Texture
            lTexture.SetPixels(lColorMap);
            lTexture.Apply();

            textureRenderer.material.mainTexture = lTexture;
            textureRenderer.transform.localScale = new Vector3(lWidth, 1, lHeight);
        }
    }
}