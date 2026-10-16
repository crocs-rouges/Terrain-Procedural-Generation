using System;
using UnityEngine;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    public enum DrawMode
    {
        NoiseMap, ColorMap, MeshMap
    }

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
        [SerializeField] private bool isBlurr = false;

        [Header("Pseudo Random")]
        [SerializeField] private int seed = 1;
        [SerializeField] private Vector2 offset;

        [Header("Regions")]
        [SerializeField] private TerrainType[] regions;
        [SerializeField] private DrawMode drawMode;

        [Header("Mesh")]
        [SerializeField, Range(0, 100)] private float heightMultiply = 1;


        private float[,] noiseMap;
        private Color[] colorMap;

        public void GenerateMap()
        {
            noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale,
            octave, persistance, lacunarity, offset);

            ApplyColorToNoiseMap();


            if (drawMode == DrawMode.NoiseMap)
                mapDislplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap, isBlurr));
            else if (drawMode == DrawMode.ColorMap)
                mapDislplay.DrawTexture(TextureGenerator.TextureFromColor(
                    colorMap, mapWidth, mapHeight, isBlurr));
            else if (drawMode == DrawMode.MeshMap)
                mapDislplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiply), TextureGenerator.TextureFromColor(
                    colorMap, mapWidth, mapHeight, isBlurr));
        }

        private void ApplyColorToNoiseMap()
        {
            colorMap = new Color[mapWidth * mapHeight];
            for (int y = 0; y < mapHeight; y++)
                for (int x = 0; x < mapWidth; x++)
                    foreach (TerrainType lRegion in regions)
                        if (lRegion.minHeight < noiseMap[x, y] &&
                            lRegion.maxHeight >= noiseMap[x, y])
                            colorMap[y * mapWidth + x] = lRegion.color;
        }
    }

    [Serializable]
    public struct TerrainType
    {
        public string name;
        public float minHeight;
        public float maxHeight;
        public Color color;
    }
}
