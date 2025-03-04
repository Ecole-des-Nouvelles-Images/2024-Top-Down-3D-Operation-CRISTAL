using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Animation
{
    public class UIButtonFeedBack : MonoBehaviour , ISelectHandler, IDeselectHandler
    {
        [SerializeField] private float _endScale = 1.2f;
        [SerializeField] private Vector3 _endRotation = new Vector3(0, 0, 3f);
        [SerializeField] private float _aniamtionTime = 0.2f;
        [SerializeField] private AnimationCurve _animationCurve = AnimationCurve.EaseInOut(0,0,1,1);

        public void OnSelect(BaseEventData eventData)
        {

            transform.DOPause();
            transform.DOScale(_endScale, _aniamtionTime).SetEase(_animationCurve);
            transform.DORotate(_endRotation, _aniamtionTime).SetEase(_animationCurve);
        
        }

        public void OnDeselect(BaseEventData eventData)
        {
            transform.DOPause();
            transform.DOScale(1, _aniamtionTime).SetEase(_animationCurve);
            transform.DORotate(Vector3.zero, _aniamtionTime).SetEase(_animationCurve);
        }
    }
}
