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

        private float[,] noiseMap;

        public void GenerateMap()
        {
            noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);
            mapDislplay.DrawNoiseMap(noiseMap);
        }
    }
}
