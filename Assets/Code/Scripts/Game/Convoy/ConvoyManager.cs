using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Convoy
{
    public class ConvoyManager : MonoBehaviour
    {
        public static List<Module> Modules;

        [Header("Settings")]
        [SerializeField] private int _maximumDurability;

        [Header("UI")]
        public Slider Gauge;
        public Image GaugeFill;
        public Color NominalColor;
        public Color WarningColor;
        public Color DangerColor;
        public Color CriticalColor;
        
        public float MaxDurability { get; set; }
        public float Durability
        {
            get => _durability;
            set => _durability = Mathf.Clamp(value, 0, _maximumDurability);
        }
        public float DurabilityRatio => Durability / MaxDurability;

        public bool Operational => Durability > 0;

        private float _durability;
        
        private void Awake()
        {
            MaxDurability = _maximumDurability;
            Durability = MaxDurability;
            Modules = new List<Module>(GetComponentsInChildren<Module>(true));
            UpdateUI();
        }
        
        private void OnDestroy()
        {
            DOTween.Kill(GaugeFill);
        }

        public void UpdateUI()
        {
            if (!Gauge) return;
            Gauge.maxValue = MaxDurability;
            Gauge.value = Durability;
        }

        public void TakeDamage(int damage)
        {
            Durability -= damage;

            switch (DurabilityRatio)
            {
                case <= 0f:
                    // Explosion FX
                    break;
                case <= .10f:
                    GaugeFill.color = CriticalColor;
                    GaugeFill.DOColor(Color.white, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    AudioManager.Instance.StartDurabilityLowCriticalAlarm();
                    break;
                case <= .20f:
                    GaugeFill.color = DangerColor;
                    GaugeFill.DOColor(Color.red, 1.2f).SetLoops(-1, LoopType.Yoyo);
                    AudioManager.Instance.StartDurabilityLowHeavyAlarm();
                    break;
                case <= .35f:
                    GaugeFill.color = WarningColor;
                    AudioManager.Instance.StartDurabilityLowModerateAlarm();
                    break;
                case > .35f:
                    GaugeFill.color = NominalColor;
                    AudioManager.Instance.StopConvoyFX();
                    break;
            }
            
            UpdateUI();
        }
    }
}
