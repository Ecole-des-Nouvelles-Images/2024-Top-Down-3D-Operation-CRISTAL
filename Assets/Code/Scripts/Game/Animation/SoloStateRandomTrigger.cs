using UnityEngine;

namespace Game.Animation
{
    public class SoloStateRandomTrigger : MonoBehaviour
    {
        [Header("Fixed Random")]
        [Range(0.01f, 0.99f)] public float Probability = 0.4f;
        public float MinimumRepeatDelay = 2f;

        [Header("Multi-value Random")]
        public bool UseRandomIntervals;
        public Vector2 ProbabilityInterval = new (0.2f, 0.5f);
        public Vector2 MinimumRepeatDelayInterval = new (1, 2);
        
        private Animator _animator;
        private float _internalTimer = 0f;
        private static readonly int Sway = Animator.StringToHash("Sway");

        private void OnValidate()
        {
            if (ProbabilityInterval.x is <= 0 or >= 1)
            {
                Debug.Log($"{gameObject.name}: Random animation probabilities have to be between 0 and 1");
                ProbabilityInterval.x = 0.01f;
            }

            if (ProbabilityInterval.y is <= 0 or >= 1)
            {
                Debug.Log($"{gameObject.name}: Random animation probabilities have to be between 0 and 1");
                ProbabilityInterval.y = 0.99f;
            }

            if (ProbabilityInterval.x > ProbabilityInterval.y)
            {
                Debug.Log($"{gameObject.name}: Random animation probabilities have to be set like so: (minimum ; maximum)");
                (ProbabilityInterval.x, ProbabilityInterval.y) = (ProbabilityInterval.y, ProbabilityInterval.x);
            }

            if (MinimumRepeatDelayInterval.x <= 0.1f)
            {
                Debug.LogWarning($"{gameObject.name}: Animation repeat delay shouldn't be so low and can't be below 0");
                MinimumRepeatDelayInterval = new Vector2(0.5f, 0.5f);
            }

            if (MinimumRepeatDelay < 0.5f || MinimumRepeatDelayInterval.x < 0.5f)
                Debug.LogWarning($"{gameObject.name}: Low repeat delay could cause problems in the animation.\nDouble check the interval or you should decrease the Probability");

            if (MinimumRepeatDelayInterval.x > MinimumRepeatDelayInterval.y)
            {
                Debug.Log($"{gameObject.name}: Animation repeat delay have to be set like so: (minimum ; maximum)");
                (MinimumRepeatDelayInterval.x, MinimumRepeatDelayInterval.y) = (MinimumRepeatDelayInterval.y, MinimumRepeatDelayInterval.x);
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (UseRandomIntervals)
                IntervalRandomTrigger();
            else
                FixedRandomTrigger();

            _internalTimer += Time.deltaTime;
        }

        private void FixedRandomTrigger()
        {
            if (_internalTimer > MinimumRepeatDelay)
            {
                if (Random.Range(0f, 1f) <= Probability)
                    _animator.SetTrigger(Sway);
                
                _internalTimer = 0f;
            }
        }

        private void IntervalRandomTrigger()
        {
            if (_internalTimer > Random.Range(MinimumRepeatDelayInterval.x, MinimumRepeatDelayInterval.y))
            {
                if (Random.Range(0f, 1f) <= Random.Range(ProbabilityInterval.x, ProbabilityInterval.y))
                    _animator.SetTrigger(Sway);

                _internalTimer = 0f;
            }
        }
    }
}
