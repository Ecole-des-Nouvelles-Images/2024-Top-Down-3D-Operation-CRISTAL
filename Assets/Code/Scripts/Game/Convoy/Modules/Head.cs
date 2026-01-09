using Code.Scripts.Game.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Game.Convoy.Modules
{
    public class Head : Module
    {
        public static bool InteractionReady = true;
        
        protected void Start()
        {
            MaximumControllers = 0;
        }

        public override bool EnterModule(PlayerController newController)
        {
            if (!InteractionReady) return false;

            base.EnterModule(newController);
                
            if (!GameManager.AsStartedOnce) 
                newController.ReadyPanel.GetComponentInChildren<Image>().color = Color.green;

            if (Controllers.Count != MaximumControllers) return true;
            
            switch (GameManager.Instance.IsInTransit)
            {
                case true:
                    GameManager.Instance.OnStopTransit.Invoke();
                    GameManager.AsStartedOnce = true;
                    break;
                case false:
                    GameManager.Instance.OnStartTransit.Invoke();
                    break;
            }

            Deactivate();
            Online = true;
            InteractionReady = false;

            return true;
        }

        public override bool ExitModule(PlayerController currentController)
        {
            if (!GameManager.AsStartedOnce)
                currentController.ReadyPanel.GetComponentInChildren<Image>().color = Color.red;
            
            return base.ExitModule(currentController);
        }

        public override void Operate(PlayerController currentController) {} // Does nothing;

        public void UpdateMaximumControllers(int currentControllersPlaying)
        {
            MaximumControllers = currentControllersPlaying;
        }
    }
}
