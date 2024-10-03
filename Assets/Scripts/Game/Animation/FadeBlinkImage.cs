using System;
using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;

namespace Game.Animation
{
    public class FadeBlinkImage : MonoBehaviour
    {
        public Image Image;
        public float AnimationDuration;
        
        void Start()
        {
            DOTween.Init();
            Image.DOFade(0, AnimationDuration).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDestroy()
        {
            StopFade();
        }

        public void StopFade()
        {
            DOTween.Kill(Image);
        }
    }
}
