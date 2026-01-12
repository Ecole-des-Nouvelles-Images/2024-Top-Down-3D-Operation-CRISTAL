using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Code.Scripts.Game.POIs
{
    public class CrystalDeposit: POI
    {
        public Image DepositGauge;
        public Vector2Int CapacityRange;
        [Range(0, 10)] public float MiningSpeedMultiplier = 1;

        public int Capacity { get; private set; }
        public int CurrentCapacity {
            get => _currentCapacity;
            set
            {
                _currentCapacity = Mathf.Clamp(value, 0, Capacity);
                DepositGauge.fillAmount = (float) CurrentCapacity / Capacity;
            }
        }
        private int _currentCapacity;
        private List<GameObject> _childsObjects = new ();
        private int _initialFragment;
        private int _fragmentNumber;

        private void Awake()
        {
            DepositGauge = transform.Find("DepositGauge/Fill").GetComponent<Image>();
        }

        private void Start()
        {
            Capacity = Random.Range(CapacityRange.x, CapacityRange.y + 1);
            CurrentCapacity = Capacity;

            foreach (Transform child in transform)
            {
                if (!child.GetComponentInChildren<Canvas>())
                    _childsObjects.Add(child.gameObject);
            }
            
            _initialFragment = _childsObjects.Count;
            _fragmentNumber = _childsObjects.Count;
        }

        private void Update()
        {
            if (CurrentCapacity <= 0) {
                Destroy(this.gameObject);
            }

            float ratio = (float)CurrentCapacity / Capacity;
            int targetFragmentNumber = Mathf.Max(1, Mathf.FloorToInt(ratio * _initialFragment));

            if (_fragmentNumber > targetFragmentNumber && _childsObjects.Count > 0)
            {
                int index = Random.Range(0, _childsObjects.Count);
                Destroy(_childsObjects[index]);
                _childsObjects.RemoveAt(index);
                _fragmentNumber -= 1;
            }
        }
    }
}
