using UnityEngine;

namespace Game.Terrain.Procedural
{
    public class MeshData
    {
        public readonly Vector3[] Vertices;
        public readonly int[] Triangles;
        public readonly Vector2[] UVs;

        private int _triangleIndex;

        public MeshData(int meshWidth, int meshHeight)
        {
            Vertices = new Vector3[meshWidth * meshHeight];
            Triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
            UVs = new Vector2[meshWidth * meshHeight];
        }

        public void AddTriangle(int v1, int v2, int v3)
        {
            Triangles[_triangleIndex] = v1;
            Triangles[_triangleIndex + 1] = v2;
            Triangles[_triangleIndex + 2] = v3;
            _triangleIndex += 3;
        }

        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh {
                vertices = Vertices,
                triangles = Triangles,
                uv = UVs
            };

            mesh.RecalculateNormals();
            return mesh;
        }
    }
}