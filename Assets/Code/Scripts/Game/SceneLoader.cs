using System.Collections;
using Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        public Animator Animator;
        public float AnimationDuration = 2f;

        private int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;
        
        private static readonly int SwipeIn = Animator.StringToHash("SwipeIn");
        private static readonly int SwipeOut = Animator.StringToHash("SwipeOut");

        private void Start()
        {
            if (CurrentSceneIndex != 0)
            {
                Animator.gameObject.SetActive(true);
                Animator.SetTrigger(SwipeOut);
            }
                
        }

        public void LoadScene(int buildIndex)
        {
            StartCoroutine(TransitionToScene(buildIndex));
        }

        private IEnumerator TransitionToScene(int buildIndex)
        {
            Animator.SetTrigger(SwipeIn);
            
            yield return new WaitForSecondsRealtime(AnimationDuration);
            
            SceneManager.LoadScene(buildIndex, LoadSceneMode.Single);
            Time.timeScale = 1;
        }

        public void ExitApplication()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
            #endif
            Application.Quit();
        }
    }
}
