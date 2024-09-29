using Game.Player;
using UnityEngine;

namespace Game.Convoy.Modules
{
    public class Shield: Module
    {
        [Header("Shield specifics")]
        public GameObject BarrierRight;
        public GameObject BarrierLeft;

        private bool IsActive => BarrierLeft.activeSelf || BarrierRight.activeSelf;

        // OnModuleOperate action triggers 2 times: prevents "cancelled" call instead of touching the input.
        private bool _ignoreInputInteraction;
        
        #region Actions

        private void FixedUpdate()
        {
            if (!Online || BatteryCharge<= 0)
            {
                if (BatteryCharge <= 0)
                    BatteryDepletedWarn();
                
                BarrierLeft.SetActive(false);
                BarrierRight.SetActive(false);
                return;
            }

            if (IsActive)
            {
                BatteryCharge -= ConsumptionPerSecond * Time.fixedDeltaTime;
                UpdateUIBatteryCharge();
            }
                
        }

        public override void Operate(PlayerController currentController)
        {
            if (!_ignoreInputInteraction)
            {
                if (!Online || BatteryCapacity <= 0) return;
                
                if (IsActive)
                    BarrierLeft.SetActive(false);
                
                BarrierRight.SetActive(!BarrierRight.activeSelf);
                _ignoreInputInteraction = true;
            }
            else 
            {
                _ignoreInputInteraction = false;
            }
        }

        public override void Interact(PlayerController currentController)
        {
            if (!Online || BatteryCapacity <= 0) return;
            
            if (IsActive)
                BarrierRight.SetActive(false);
            
            BarrierLeft.SetActive(!BarrierLeft.activeSelf);
        }
        
        #endregion
    }
}
