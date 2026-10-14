using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int pMapWidth, int pMapHeight, float pScale)
    {
        float[,] lNoiseMap = new float[pMapWidth, pMapHeight];

        if (pScale <= 0) pScale = 0.001f;

    }
}
