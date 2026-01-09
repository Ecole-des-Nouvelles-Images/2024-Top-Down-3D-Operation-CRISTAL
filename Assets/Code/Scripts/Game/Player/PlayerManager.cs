using Code.Scripts.Game.Convoy.Modules;
using Code.Scripts.Game.UI;
using Code.Scripts.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Game.Player
{
    public class PlayerManager: SingletonMonoBehaviour<PlayerManager>
    {
        [Header("Cheatsheet")]
        [SerializeField] private GameObject _playerCheatsheetBase;
        [SerializeField] private GameObject _cheatsheetContainer;
        
        
        [Header("Settings")]
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
            PlayerController controller = player.gameObject.GetComponent<PlayerController>();
            
            ++PlayerNumber;
            _convoyHead.UpdateMaximumControllers(PlayerNumber);
            GameManager.Instance.AddPlayerReadyIcon(player, PlayerNumber);
            controller.Cheatsheet = Instantiate(_playerCheatsheetBase, _cheatsheetContainer.transform).GetComponent<Cheatsheet>();
        }

        public void OnPlayerLeft(PlayerInput player)
        {
            --PlayerNumber;
            _convoyHead.UpdateMaximumControllers(PlayerNumber);
        }
    }
}
