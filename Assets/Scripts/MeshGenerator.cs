using UnityEngine;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    public static class MeshGenerator
    {
        public static MeshData GenerateTerrainMesh(float[,] pHeightMap)
        {
            int lWidth = pHeightMap.GetLength(0);
            int lHeight = pHeightMap.GetLength(1);

            // Values to Center the Mesh
            float lTopLeftX = (lWidth - 1) / -2f;
            float lTopLeftZ = (lHeight - 1) / 2f;



            MeshData lMeshData = new MeshData(lWidth, lHeight);
            int lVertexIndex = 0;
            Vector3 lPointPosition;

            for (int y = 0; y < lHeight; y++)
                for (int x = 0; x < lWidth; x++)
                {
                    lPointPosition = new Vector3(lTopLeftX + x,
                        pHeightMap[x, y], lTopLeftZ - y);

                    lMeshData.vertices[lVertexIndex] = lPointPosition;
                    lMeshData.Uvs[lVertexIndex] = new Vector2(x / (float)lWidth, y / (float)lHeight);

                    if (x < lWidth - 1 && y < lHeight - 1)
                    {
                        // Points For Vertices
                        int lPointTopLeft = lVertexIndex;
                        int lPointTopRight = lVertexIndex + 1;
                        int lPointBottomLeft = lVertexIndex + lWidth;
                        int lPointBottomRight = lVertexIndex + lWidth + 1;

                        // Triangle 1
                        lMeshData.AddTriangle(lPointTopLeft, lPointBottomRight, lPointBottomLeft);
                        // Triangle 2
                        lMeshData.AddTriangle(lPointBottomRight, lPointTopLeft, lPointTopRight);
                    }
                    lVertexIndex++;
                }
            return lMeshData;
        }
    }



    public class MeshData
    {
        /// <summary>
        /// Number of Cell we Have = Number of pixel of the noise
        /// One Value is one Point of the Mesh
        /// </summary>
        public Vector3[] vertices; // width * height
        /// <summary>
        /// Cell Position in Space
        /// </summary>
        public Vector2[] Uvs;
        /// <summary>
        /// 2 Triangles per Vectice
        /// to Find How many Triangles we need to know how many Vertices we have
        /// 3 Values Make a Triangle
        /// </summary>
        public int[] triangles; // 2 triangles per Vertice, 3 values per Triangle
        /// <summary>
        /// Max number of Points For All the Triangles
        /// </summary>
        int triangleIndex;

        /// <summary>
        /// Create a New Mesh
        /// </summary>
        /// <param name="pMeshWidth"></param>
        /// <param name="pMeshHeight"></param>
        public MeshData(int pMeshWidth, int pMeshHeight)
        {
            vertices = new Vector3[pMeshWidth * pMeshHeight];
            Uvs = new Vector2[pMeshWidth * pMeshHeight];

            // Triangles Calcul
            // If 25 pixels, we have 24 Cells (so Width-1)
            int lWidth = pMeshWidth - 1;
            int lHeight = pMeshHeight - 1;
            int lFinalValue = lWidth * lHeight;
            // 6 because each Triangle is 3 points and 1 vertices is 2 triangles
            // 3 points * 2 triangles = 6
            lFinalValue *= 6;
            triangles = new int[(pMeshWidth - 1) * (pMeshHeight - 1) * 6];
        }

        public void AddTriangle(int pValueA, int pValueB, int pValueC)
        {
            triangles[triangleIndex] = pValueA;
            triangles[triangleIndex + 1] = pValueB;
            triangles[triangleIndex + 2] = pValueC;
            triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh lMesh = new Mesh();
            lMesh.vertices = vertices;
            lMesh.uv = Uvs;
            lMesh.triangles = triangles;

            // Recalculate for Lightning Reflexion
            lMesh.RecalculateNormals();

            return lMesh;
        }

    }
}