using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Convoy.Drones;
using Game.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Convoy
{
    public abstract class Module : MonoBehaviour, IDamageable
    {
        [Header("Player Management")] [SerializeField]
        protected int MaximumControllers = 1;

        [Header("Settings")] public bool Online;
        [SerializeField] protected int BatteryCapacity;
        [SerializeField] protected int ConsumptionPerSecond;

        [Header("Interface")]
        [SerializeField] protected Slider BatteryGauge;
        [SerializeField] public Image ChargeStatus;
        [SerializeField] protected Image ModuleIcon;

        public string Type => GetType().ToString();
        public int BatteryMaxCapacity => BatteryCapacity;
        public float BatteryCharge { get; set; }

        protected ConvoyManager Convoy;

        protected readonly List<PlayerController> Controllers = new();
        protected bool IsFull
        {
            get
            {
                if (Controllers.Count > MaximumControllers) throw new Exception($"Error: Module {name} registered too much players !");
                return Controllers.Count == MaximumControllers;
            }
        }
        protected bool IsOperated => Controllers.Count > 0;
        
        protected virtual void Awake()
        {
            Convoy = FindAnyObjectByType<ConvoyManager>();
            Online = true;
            BatteryCharge = BatteryCapacity;
            UpdateUIBatteryCharge();
        }

        protected virtual void Update()
        {
            if (!Online)
                Deactivate();
        }

        public virtual void Deactivate()
        {
            foreach (PlayerController player in Controllers.ToList())
            {
                ExitModule(player);
            }

            Online = false;
        }

        #region Actions

        public virtual bool EnterModule(PlayerController newController)
        {
            if (GameManager.InTutorial) return false;
            
            if (!Online || IsFull || Controllers.Contains(newController))
                return false;

            Controllers.Add(newController);
            newController.IsBusy = true;

            if (BatteryMaxCapacity > 0) {
                BatteryGauge.gameObject.SetActive(true);
                ModuleIcon.gameObject.SetActive(false);
            }

            if (MaximumControllers == 1)
            {
                BatteryGauge.transform.Find("Fill Area/Fill").GetComponent<Image>().color = newController.PlayerColor;
                ModuleIcon.color = newController.PlayerColor;
            }
            
            return true;
        }

        public virtual bool ExitModule(PlayerController currentController)
        {
            if (!Controllers.Contains(currentController))
                return false;

            Controllers.Remove(currentController);
            currentController.IsBusy = false;

            if (BatteryMaxCapacity > 0)
            {
                BatteryGauge.gameObject.SetActive(false);
                ModuleIcon.gameObject.SetActive(true);
            }
            
            BatteryGauge.transform.Find("Fill Area/Fill").GetComponent<Image>().color = Color.white;
            ModuleIcon.color = Color.white;
            
            return true;
        }

        public abstract void Operate(PlayerController currentController);

        public virtual void Interact(PlayerController currentController) {}

        public virtual void Aim(InputValue input) {}

        #endregion

        #region UI Utilities

        public void WakeUIBattery(bool enable)
        {
            // ChargeStatus.color = enable ? Color.yellow : Color.black;
            
            if (BatteryMaxCapacity > 0)
            {
                BatteryGauge.gameObject.SetActive(enable);
                ModuleIcon.gameObject.SetActive(!enable);
            }
        }

        public void UpdateUIBatteryCharge()
        {
            BatteryGauge.maxValue = BatteryCapacity;
            BatteryGauge.value = BatteryCharge;
        }

        public void ToggleChargeStatus(bool enable)
        {
            Color indicatorColor = enable ? Color.white : Color.black;
            
            ChargeStatus.color = indicatorColor;
        }

        protected void BatteryDepletedWarn()
        {
            if (DOTween.IsTweening(ChargeStatus, true))
                return;

            ChargeStatus.DOColor(Color.red, .7f).SetLoops(-1, LoopType.Yoyo);
        }

        #endregion
        
        #region IDamageable

        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        public bool IsTargetable => Online;

        public void TakeDamage(int damage)
        {
            Convoy.TakeDamage(damage);
        }
        
        #endregion
    }
}
