using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.POIs
{
    public class CrystalDeposit: POI
    {
        public Vector2Int CapacityRange;
        [Range(0, 10)] public float MiningSpeedMultiplier = 1;

        public int Capacity { get; private set; }
        public int CurrentCapacity {
            get => _currentCapacity;
            set => _currentCapacity = Mathf.Clamp(value, 0, Capacity);
        }
        private int _currentCapacity;
        private List<GameObject> _childsObjects = new ();
        private int _initialFragment;
        private int _fragmentNumber;

        private void Start()
        {
            Capacity = Random.Range(CapacityRange.x, CapacityRange.y + 1);
            CurrentCapacity = Capacity;

            foreach (Transform child in transform)
                _childsObjects.Add(child.gameObject);

            _initialFragment = _childsObjects.Count;
            _fragmentNumber = _childsObjects.Count;
        }

        private void Update()
        {
            if (CurrentCapacity <= 0) {
                Destroy(this.gameObject);
            }

            if (CurrentCapacity / (Capacity / _initialFragment) <= _fragmentNumber - 1)
            {
                int index = Random.Range(0, _childsObjects.Count);
                Destroy(_childsObjects[index]);
                _childsObjects.RemoveAt(index);
                _fragmentNumber -= 1;
            }
        }
    }
}
