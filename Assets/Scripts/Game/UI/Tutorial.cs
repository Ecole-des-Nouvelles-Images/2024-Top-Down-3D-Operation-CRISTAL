using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.UI
{
    public class Tutorial: MonoBehaviour
    {
        [Header("Music")] [SerializeField] private AudioSource _music;
        
        [Header("Animations")]
        [SerializeField] private Animator _welcomeAnimator;
        [SerializeField] private float _fadeTime = 0.5f;
        [SerializeField] private float _delayBetweenPanel = 0.5f;
        [SerializeField] private float _endFadeTime = 3f;

        [Header("Panels")]
        [SerializeField] private CanvasGroup _welcome;
        [SerializeField] private CanvasGroup _subtitle1;
        [SerializeField] private CanvasGroup _subtitle2;
        [Space(10)]
        [SerializeField] private CanvasGroup _convoyWhole;
        [SerializeField] private CanvasGroup _convoyHead;
        [SerializeField] private CanvasGroup _convoyDrones;
        [SerializeField] private CanvasGroup _convoyLasers;
        [SerializeField] private CanvasGroup _convoyShield;
        [SerializeField] private CanvasGroup _convoyGenerator;
        [SerializeField] private CanvasGroup _pauseRestartNotice;
        [SerializeField] private CanvasGroup _root;
        
        [Header("GameOverlays")]
        [SerializeField] private CanvasGroup _gameOverlay;
        
        private Gamepad _mainGamepad;
        private int _tutorialIndex;
        private bool _coroutineRunning;

        private void Start()
        {
            AudioManager.Instance.gameObject.SetActive(false);
            _music.Play();
        }

        private void Update()
        {
            if (_mainGamepad == null) {
                _mainGamepad = Gamepad.current;
                return;
            }
            
            if (_mainGamepad.buttonSouth.wasPressedThisFrame && !_coroutineRunning) {
                _tutorialIndex++;
                ProgressTutorial();
            }

            if (_mainGamepad.buttonEast.wasPressedThisFrame && !_coroutineRunning)
            {
                StartCoroutine(EndTutorial());
            }
        }

        private void ProgressTutorial()
        {
            Debug.Log($"Tutorial: transitioning from panel {_tutorialIndex - 1} to {_tutorialIndex}");
            switch (_tutorialIndex)
            {
                case 0:
                    //Animate welcome Panel;
                    break;
                case 1:
                    StartCoroutine(SwitchPanels(_welcome, _convoyWhole));
                    break;
                case 2:
                    StartCoroutine(SwitchPanels(_convoyWhole, _convoyHead));
                    break;
                case 3:
                    StartCoroutine(SwitchPanels(_convoyHead, _convoyDrones));
                    break;
                case 4:
                    StartCoroutine(SwitchPanels(_convoyDrones, _convoyLasers));
                    break;
                case 5:
                    StartCoroutine(SwitchPanels(_convoyLasers, _convoyShield));
                    break;
                case 6:
                    StartCoroutine(SwitchPanels(_convoyShield, _convoyGenerator));
                    break;
                case 7:
                    StartCoroutine(SwitchPanels(_convoyGenerator, _pauseRestartNotice));
                    break;
                case 8:
                    StartCoroutine(EndTutorial());
                    break;
                default:
                    throw new Exception($"Tutorial isn't supposed to handle case index {_tutorialIndex}");
            }
        }

        private IEnumerator SwitchPanels(CanvasGroup outPanel, CanvasGroup inPanel)
        {
            _coroutineRunning = true;
            
            float t = 0f;

            while (t <= 1)
            {
                t += Time.deltaTime / _fadeTime;
                if (outPanel)
                    outPanel.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }

            if (!inPanel) yield break;
            
            yield return new WaitForSeconds(_delayBetweenPanel);
            t = 0;
            
            while (t <= 1)
            {
                t += Time.deltaTime / _fadeTime;
                inPanel.alpha = Mathf.Lerp(0, 1, t);
                yield return null;
            }

            _coroutineRunning = false;
        }
        
        private IEnumerator EndTutorial()
        {
            Debug.Log("Tutorial ending...");
            float t = 0f;
            float initVolume = _music.volume;

            while (t <= 1)
            {
                t += Time.deltaTime / _endFadeTime;
                _root.alpha = Mathf.Lerp(1, 0, t);
                _music.volume = Mathf.Lerp(initVolume, 0, t);
                yield return null;
            }
            
            yield return new WaitForSeconds(_delayBetweenPanel);
            t = 0;
            
            while (t <= 1)
            {
                t += Time.deltaTime / _endFadeTime;
                _gameOverlay.alpha = Mathf.Lerp(initVolume, 1, t);
                yield return null;
            }
            
            GameManager.InTutorial = false;
            _music.Stop();
            AudioManager.Instance.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
