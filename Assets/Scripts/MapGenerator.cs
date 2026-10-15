using System;
using UnityEngine;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    // [ExecuteAlways]
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private MapDislplay mapDislplay;

        [Header("Generic Noise Parameter")]
        [SerializeField, Range(1, 500)] private int mapWidth = 10;
        [SerializeField, Range(1, 500)] private int mapHeight = 10;
        [SerializeField, Range(1, 150)] private float noiseScale = 1;
        [Tooltip("Number of Layer")]
        [SerializeField, Range(1, 8)] private int octave = 1;
        [Tooltip("Add Detail to the Noise")]
        [SerializeField, Range(0, 1)] private float persistance = 1;
        [Tooltip("Remove Detail from the Noise")]
        [SerializeField, Range(1, 10)] private float lacunarity = 1;

        [Header("Pseudo Random")]
        [SerializeField] private int seed = 1;
        [SerializeField] private Vector2 offset;

        [Header("Regions")]
        [SerializeField] private TerrainType[] regions;
        [SerializeField] private bool drawNoiseMap = false; // if false show color

        private float[,] noiseMap;
        private Color[] colorMap;

        public void GenerateMap()
        {
            noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale,
            octave, persistance, lacunarity, offset);

            ApplyColorToNoiseMap();


            Texture2D lTexture;
            if (drawNoiseMap) lTexture = TextureGenerator.TextureFromHeightMap(noiseMap);
            else lTexture = TextureGenerator.TextureFromColor(colorMap, mapWidth, mapHeight);

            mapDislplay.DrawTexture(lTexture);
        }

        private void ApplyColorToNoiseMap()
        {
            colorMap = new Color[mapWidth * mapHeight];
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    for (int lRegionIndex = 0; lRegionIndex < regions.Length; lRegionIndex++)
                    {
                        if (regions[lRegionIndex].height >= noiseMap[x, y])
                        {
                            colorMap[y * mapWidth + x] = regions[lRegionIndex].color;
                        }
                    }
                }
            }
        }
    }

    [Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}
