using System;
using Unity.AI.Navigation;
using UnityEngine;

namespace Game.Terrain.Procedural
{
    public enum GenerationMode
    {
        NoiseMap,
        Mesh
    }
    
    [RequireComponent(typeof(MapDisplay))]
    public class MapGenerator : MonoBehaviour
    {
        [Header("Components")]
        public MapDisplay MapDisplay;

        [Header("Map Scale")]
        [Range(0, 6)] public int LODReduction;
        public float NoiseScale;
        public float HeightScale;
        public AnimationCurve HeightScaleDistributionCurve;

        [Header("Noise Precision")]
        public int Octaves;
        [Range(0, 1)] public float Persistence;
        public float Lacunarity;

        [Header("Randomness")]
        public int Seed;
        public Vector2 Offset;

        [Header("Generation")]
        public GenerationMode DrawMode;
        public bool AutoUpdate;
        
        public const int ChunkSize = 241;

        private void Start() {
            GameManager.Instance.OnStartTransit += OnStartTransit;
        }

        private void OnStartTransit()
        {
            NavMeshSurface navMeshSurface = GetComponent<NavMeshSurface>();
            if (!navMeshSurface) return;
            navMeshSurface.enabled = false;
        }

        private void OnValidate()
        {
            if (Lacunarity < 1) Lacunarity = 1;
            if (NoiseScale <= 0) NoiseScale = 0.001f;
            if (Octaves <= 0) Octaves = 1;
        }

        public void GenerateMap()
        {
            float[,] noiseMap = Noise.GenerateNoiseMap(ChunkSize, ChunkSize, Seed, NoiseScale, Octaves, Persistence, Lacunarity, Offset);

            switch (DrawMode)
            {
                case GenerationMode.NoiseMap:
                    MapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap, MapDisplay.FilterMode));
                    break;
                case GenerationMode.Mesh:
                    MapDisplay.DrawMesh(MeshGenerator.GenerateMesh(noiseMap, HeightScale, HeightScaleDistributionCurve, LODReduction), TextureGenerator.TextureFromHeightMap(noiseMap, MapDisplay.FilterMode));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            GameManager.Instance.OnStartTransit -= OnStartTransit;
        }
    }
}
