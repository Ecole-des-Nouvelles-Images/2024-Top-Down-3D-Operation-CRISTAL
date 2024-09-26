using UnityEngine;
using UnityEngine.InputSystem;

using Internal;
using Game.Convoy.Modules;

namespace Game.Player
{
    public class PlayerManager: SingletonMonoBehaviour<PlayerManager>
    {
        [HideInInspector] public int PlayerNumber;
        // public List<Transform> PlayerOriginPositions;

        private Head _convoyHead;

        private void Start()
        {
            PlayerNumber = 0;
            _convoyHead = FindAnyObjectByType<Head>();
        }

        public void OnPlayerJoined(PlayerInput player)
        {
            ++PlayerNumber;
            _convoyHead.UpdateMaximumControllers(PlayerNumber);
            GameManager.Instance.AddPlayerReadyIcon(player, PlayerNumber);
        }

        public void OnPlayerLeft(PlayerInput player)
        {
            --PlayerNumber;
            _convoyHead.UpdateMaximumControllers(PlayerNumber);
        }
    }
}
