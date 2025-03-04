using UnityEngine;

namespace Game.Terrain.Procedural
{
    public class MapDisplay : MonoBehaviour
    {
        public MeshRenderer Renderer;
        
        [Header("Texture Settings")]
        public FilterMode FilterMode;

        [Header("Mesh Settings")]
        public MeshFilter MeshFilter;
        public MeshRenderer MeshRenderer;
        public MeshCollider MeshCollider;

        [Header("Rendering")]
        public Material Material;

        public void DrawTexture(Texture2D texture)
        {
            Renderer.sharedMaterial.mainTexture = texture;
            Renderer.transform.localScale = new Vector3 (texture.width, 1, texture.height);
        }

        public void DrawMesh(MeshData meshData, Texture2D texture)
        {
            MeshFilter.sharedMesh = meshData.CreateMesh();
            MeshCollider.sharedMesh = MeshFilter.sharedMesh;
            MeshRenderer.sharedMaterial = Material;
        }
    }
}
