using UnityEngine;

// Author : Romain Chevalier

namespace Com.IsartDigital
{
    public class MapDislplay : MonoBehaviour
    {
        [SerializeField] private Renderer textureRenderer;

        /// <summary>
        /// Draw the given Texture
        /// </summary>
        public void DrawTexture(Texture2D pTexture)
        {
            textureRenderer.material.mainTexture = pTexture;
            textureRenderer.transform.localScale = new Vector3(pTexture.width, 1, pTexture.height);
        }
    }
}