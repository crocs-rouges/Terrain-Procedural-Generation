using UnityEngine;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    // [ExecuteAlways]
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private MapDislplay mapDislplay;
        [SerializeField, Range(1, 500)] private int mapWidth = 10;
        [SerializeField, Range(1, 500)] private int mapHeight = 10;
        [SerializeField, Range(1, 150)] private float noiseScale = 1;
        [SerializeField] private int seed = 1;
        [SerializeField, Range(1, 8)] private int octave = 1;
        [SerializeField, Range(0, 1)] private float persistance = 1;
        [SerializeField, Range(1, 10)] private float lacunarity = 1;

        private float[,] noiseMap;

        public void GenerateMap()
        {
            noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, octave, persistance, lacunarity);
            mapDislplay.DrawNoiseMap(noiseMap);
        }
    }
}
