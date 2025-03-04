using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;
using Unity.Cinemachine;

using Internal;
using Game.Terrain.Procedural;
using Game.Convoy;
using Game.Convoy.Modules;
using Random = UnityEngine.Random;

namespace Game.Terrain
{
    public class TerrainManager: SingletonMonoBehaviour<TerrainManager>
    {
        [Header("References")]
        public GameObject BrakeVFX;
        public GameObject ConvoyTrails;
        [SerializeField] private CinemachineCamera _transitCamera;
        [SerializeField] private Transform _chunksRoot;
        [SerializeField] private GameObject _chunkTransitPrefab;
        [SerializeField] private List<GameObject> _stopsPrefabs;
        [Range(2, 5)] [SerializeField] private int _chunksQueueSize = 3;
        
        [Header("Transit phase")]
        public bool EnableTransit;
        [SerializeField] private float _scrollSpeed = 10f;
        [SerializeField] private float _offsetBetweenChunks = 240f;
        public float RepeatDistance = 240f;
        public float ScrolledDistance;
        public AnimationCurve ScrolledDistanceCurve; // x is the speed and y should be the Distance to the stop Zone
        
        private Queue<TerrainChunk> Chunks { get; set; }
        private TerrainChunk _lastEnqueued;

        private Transform _convoy;
        CinemachineBasicMultiChannelPerlin _camNoise;
        
        // Stops Zones
        private GameObject _currentStopZone;
        private MapGenerator _generator;
        private NavMeshSurface _navMesh;

        private void Start()
        {
            Chunks = new Queue<TerrainChunk>(_chunksQueueSize);
            _camNoise = _transitCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            _convoy = FindAnyObjectByType<ConvoyManager>().transform;
            
            float position = -_offsetBetweenChunks;
            
            for (int chunkIndex = -1; chunkIndex < _chunksQueueSize; chunkIndex++)
            {
                position += _offsetBetweenChunks;
                _lastEnqueued = CreateChunk(_chunksRoot, position);
                Chunks.Enqueue(_lastEnqueued);
            }
        }

        private void FixedUpdate()
        {
            if (!EnableTransit) return;
            
            MoveChunks(_scrollSpeed, Time.fixedDeltaTime);
            
            if (ScrolledDistance >= RepeatDistance)
            {
                UpdateRoad();
                ScrolledDistance -= ScrolledDistance;
            }
        }
        
        #region Chunk Management
        
        private void MoveChunks(float speed, float timeDelta)
        {
            Vector3 direction = -_chunksRoot.right;
            
            foreach (TerrainChunk chunk in Chunks) {
                chunk.transform.Translate(direction * (speed * timeDelta),Space.World);
            }

            ScrolledDistance += speed * timeDelta;
        }
        
        private void UpdateRoad()
        {
            _lastEnqueued = CreateChunk(_chunksRoot, _lastEnqueued.transform.localPosition.x + _offsetBetweenChunks);
            Chunks.Enqueue(_lastEnqueued);
            Destroy(Chunks.Dequeue().gameObject);
        }

        private TerrainChunk CreateChunk(Transform root, float positionScalar)
        {
            GameObject instance = Instantiate(_chunkTransitPrefab, root);
            TerrainChunk chunk = instance.GetComponent<TerrainChunk>();

            Vector3 position = _chunksRoot.transform.right * positionScalar;
            instance.transform.position = position;
            
            // chunk.SetupChunk(); // TODO: Disable to avoid overheads when using flat terrain over the MapGenerator;

            return chunk;
        }
        
        #endregion

        #region Phase Transitions

        public void GeneratePlayzone()
        {
            GameObject prefab = _stopsPrefabs[Random.Range(0, _stopsPrefabs.Count)];
            Vector3 position = _chunksRoot.transform.right * (_lastEnqueued.transform.localPosition.x + _offsetBetweenChunks);

            _currentStopZone = Instantiate(prefab, _chunksRoot);
            _currentStopZone.transform.position = position;

            TerrainChunk stopZone = _currentStopZone.GetComponent<TerrainChunk>();
            // stopZone.SetupChunk(); // TODO: Disable to avoid overheads when using flat terrain over the MapGenerator;
            
            Chunks.Enqueue(stopZone);
            _lastEnqueued = stopZone;
        }

        public void FinishTransit()
        {
            StartCoroutine(ReachStopZone());
        }

        private IEnumerator ReachStopZone()
        {
            float initialDistance = _currentStopZone.transform.position.x;
            float initialShakeAmplitude = _camNoise.AmplitudeGain;
            
            float actualDistance;
            float normalizeDistance;
            float currentSpeed;
            float stopThreshold = 0;
            
            BrakeVFX.SetActive(true);
            
            while (_currentStopZone.transform.position.x > _convoy.position.x + stopThreshold)
            {
                actualDistance = _currentStopZone.transform.position.x;
                normalizeDistance = (actualDistance - 1) / initialDistance;
                
                float shakeMultiplier = normalizeDistance;
                
                currentSpeed = ScrolledDistanceCurve.Evaluate(normalizeDistance);
                _camNoise.AmplitudeGain = Mathf.Lerp(initialShakeAmplitude, 0, 1 - shakeMultiplier);
                
                MoveChunks(currentSpeed, Time.deltaTime);

                yield return null;
            }

            TerrainChunk stopZoneChunk = _currentStopZone.GetComponent<TerrainChunk>();
            CameraManager.Instance.SwitchCameraFocus(false, stopZoneChunk.PlayableSide);
            GameManager.Instance.OnStopZoneReached.Invoke(stopZoneChunk);
            yield return new WaitForSeconds(0.5f);
            Head.InteractionReady = true;
            GameManager.Instance.IsInTransit = false;
            BrakeVFX.SetActive(false);
            ConvoyTrails.SetActive(false);
            AudioManager.Instance.SwitchToStopZone(AudioManager.Instance.TransitionDuration);
        }
        
        public void RestartTransit()
        {
            foreach (TerrainChunk chunk in Chunks.ToList())
            {
                if (chunk == _lastEnqueued) continue;

                Destroy(Chunks.Dequeue().gameObject);
            }

            StartCoroutine(AccelerateConvoy());
        }

        private IEnumerator AccelerateConvoy()
        {
            TerrainChunk startingChunk = CreateChunk(_chunksRoot, _lastEnqueued.transform.localPosition.x + _offsetBetweenChunks);
            float currentSpeed;
            
            GameManager.Instance.IsInTransit = true;
            CameraManager.Instance.SwitchCameraFocus(true, Side.None);
            ConvoyTrails.SetActive(true);
            AudioManager.Instance.SwitchToTransit(AudioManager.Instance.TransitionDuration);
            
            Chunks.Enqueue(startingChunk);
            _lastEnqueued = startingChunk;
            
            ScrolledDistance = 0f;

            while (ScrolledDistance < 240)
            {
                float multiplier = (ScrolledDistance / 240) + 0.01f ; // TODO: Starting speed controllable with 'ScrolledDistance' starting offset
                currentSpeed = Mathf.Clamp(_scrollSpeed * multiplier, 0, _scrollSpeed);
                _camNoise.AmplitudeGain = 0.08f * multiplier;
                
                MoveChunks(currentSpeed, Time.deltaTime);

                yield return null;
            }
            
            EnableTransit = true;
            GameManager.Instance.IsInTransit = true;
            yield return new WaitForSeconds(1f);
            Head.InteractionReady = true;
        }

        #endregion
    }
}
