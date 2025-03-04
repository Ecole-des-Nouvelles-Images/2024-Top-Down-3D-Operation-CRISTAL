using TMPro;
using UnityEngine;

using DG.Tweening;

namespace Game.Animation
{
    public class FadeBlinkText : MonoBehaviour
    {
        public TMP_Text Text;
        public float AnimationDuration;
        
        void Start()
        {
            DOTween.Init();
            Text.DOFade(0, AnimationDuration).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            StopFade();
        }

        public void StopFade()
        {
            DOTween.Kill(Text);
        }
    }
}
