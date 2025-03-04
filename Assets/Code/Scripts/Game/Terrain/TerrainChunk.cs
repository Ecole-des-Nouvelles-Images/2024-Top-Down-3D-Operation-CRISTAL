using UnityEngine;
using Unity.AI.Navigation;

using Game.Foes;
using Game.Terrain.Procedural;

namespace Game.Terrain
{
    public class TerrainChunk: MonoBehaviour
    {
        public Side PlayableSide;
        public WaveManager WaveManager;
        
        public MapGenerator Terrain { get; set; }
        public NavMeshSurface NavMesh { get; set; }

        private void Awake()
        {
            Terrain = GetComponentInChildren<MapGenerator>();
            NavMesh = GetComponentInChildren<NavMeshSurface>();
            // GameManager.Instance.OnStartTransit += DisableNavMesh; // TODO: Enable when using flat terrain over MapGenerator;
        }

        private void OnDestroy()
        {
            // GameManager.Instance.OnStartTransit -= DisableNavMesh; // TODO: Enable when using flat terrain over MapGenerator;
        }
        
        // TODO: Unused with flat terrain / Avoid overheads
        public void SetupChunk()
        {
            Terrain.GenerateMap();
        }

        private void DisableNavMesh()
        {
            NavMesh.enabled = false;
        }
    }
}
