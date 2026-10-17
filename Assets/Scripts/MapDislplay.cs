using UnityEngine;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    public class MapDislplay : MonoBehaviour
    {
        [SerializeField] private Renderer textureRenderer;
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private MeshCollider meshCollider;

        /// <summary>
        /// Draw the given Texture
        /// </summary>
        public void DrawTexture(Texture2D pTexture)
        {
            textureRenderer.material.mainTexture = pTexture;
            textureRenderer.transform.localScale = Vector3.one;
            // textureRenderer.transform.localScale = new Vector3(pTexture.width, 1, pTexture.height);
        }

        public void DrawMesh(MeshData pMeshData, Texture2D pTexture)
        {
            meshFilter.sharedMesh = pMeshData.CreateMesh();
            meshCollider.sharedMesh = meshFilter.sharedMesh;
            meshRenderer.sharedMaterial.mainTexture = pTexture;
            textureRenderer.transform.localScale = Vector3.one;
        }
    }
}