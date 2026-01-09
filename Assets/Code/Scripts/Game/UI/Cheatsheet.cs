using Code.Scripts.Game.Convoy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Game.UI
{
    public class Cheatsheet : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text _playerNumber;
        [SerializeField] private Image _icon;
    
        [Header("Bindings panels")]
        [SerializeField] private GameObject _moduleNone;
        [SerializeField] private GameObject _moduleHead;
        [SerializeField] private GameObject _moduleDrones;
        [SerializeField] private GameObject _moduleGenerator;
        [SerializeField] private GameObject _moduleLaser;
        [SerializeField] private GameObject _moduleShield;

        [Header("Icons")]
        [SerializeField] private Sprite _iconNone;
        [SerializeField] private Sprite _iconHead;
        [SerializeField] private Sprite _iconDrones;
        [SerializeField] private Sprite _iconGenerator;
        [SerializeField] private Sprite _iconLaser;
        [SerializeField] private Sprite _iconShield;

        private GameObject _activeModule;

        private void Start()
        {
            _activeModule = _moduleNone;
        }

        public void Init(int playerID, Color playerColor)
        {
            _playerNumber.text = $"J{playerID}";
            _playerNumber.color = playerColor;
        }

        public void ChangeCheatsheet(Module moduleType)
        {
            _activeModule.SetActive(false);

            switch (moduleType.GetType().Name)
            {
                case "Head":
                    _activeModule = _moduleHead;
                    _moduleHead.SetActive(true);
                    _icon.sprite = _iconHead;
                    break;
                case "DroneController":
                    _activeModule = _moduleDrones;
                    _moduleDrones.SetActive(true);
                    _icon.sprite = _iconDrones;
                    break;
                case "Generator":
                    _activeModule = _moduleGenerator;
                    _moduleGenerator.SetActive(true);
                    _icon.sprite = _iconGenerator;
                    break;
                case "Laser":
                    _activeModule = _moduleLaser;
                    _moduleLaser.SetActive(true);
                    _icon.sprite = _iconLaser;
                    break;
                case "Shield":
                    _activeModule = _moduleShield;
                    _moduleShield.SetActive(true);
                    _icon.sprite = _iconShield;
                    break;
                default:
                    _activeModule = _moduleNone;
                    _moduleNone.SetActive(true);
                    _icon.sprite = _iconNone;
                    break;
            }
        }

        public void ChangeToDefaultCheatsheet()
        {
            _activeModule.SetActive(false);
            _moduleNone.SetActive(true);
            _icon.sprite = _iconNone;
            _activeModule = _moduleNone;
        }
    }
}
