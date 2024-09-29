using Game.Animation;
using Game.Foes.FSM;
using Game.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Convoy.Modules
{
    public class Laser: Module
    {
        [Header("Laser specifics")]
        [SerializeField] private Transform _turret;
        public Transform Canon;
        [SerializeField] private float _torque;
        public int Range;
        [SerializeField] private int _damagePerSecond;
        
        [Header("Raycast settings")]
        [SerializeField] private LayerMask _targetLayers;
        [Tooltip("Keenness of the rays below the original one.\n/!\\ Increase the number of raycast made until the floor is reached.")]
        [SerializeField] [Range(0.001f, 1)] private float _raySliceKeenness = 0.1f;
        public float SphereCastRadius = 5f;

        [Header("Rendering")]
        public LaserBeam BeamRenderer;

        #region Debug

        private void OnDrawGizmos()
        {
            if (Physics.SphereCast(Canon.position, SphereCastRadius, Canon.forward, out RaycastHit hit, Range, 1 << LayerMask.NameToLayer("Xenolith")))
            {
                Gizmos.color = Color.green;
                Vector3 sphereCastMidpoint = Canon.position + (Canon.forward * hit.distance);
                Gizmos.DrawWireSphere(sphereCastMidpoint, SphereCastRadius);
                Gizmos.DrawSphere(hit.point, 0.1f);
                Debug.DrawLine(Canon.position, sphereCastMidpoint, Color.green);
            }
            else
            {
                Gizmos.color = Color.red;
                Vector3 sphereCastMidpoint = Canon.position + (Canon.forward * (Range - SphereCastRadius));
                Gizmos.DrawWireSphere(sphereCastMidpoint, SphereCastRadius);
                Debug.DrawLine(Canon.position, sphereCastMidpoint, Color.red);
            }
        }
        
        /* private void DrawLaser()
        {
            if (_range <= 0) {
                Debug.LogWarning("Range must be greater than 0");
                return;
            }

            Vector3 endPoint = _canon.position + _canon.forward * _range;
            Debug.DrawLine(_canon.position, endPoint, Color.red, Time.deltaTime);
        } */

        #endregion
        
        private bool _firing;

        protected override void Awake()
        {
            base.Awake();
            _firing = false;
        }
        
        protected override void Update()
        {
            if (!Online) return;
            
            if (BatteryCharge <= 0) {
                _firing = false;
                BeamRenderer.Disable();
                return;
            }

            if (_firing) {
                Fire();
                BeamRenderer.Activate();
                BatteryGauge.value = BatteryCharge;
            }
            else
            {
                BeamRenderer.Disable();
            }
        }

        #region Logic

        private void Fire()
        {
            Xenolith unit = RaycastForwardTarget();
            if (unit)
                unit.TakeDamage(_damagePerSecond * Time.deltaTime);

            BatteryCharge -= ConsumptionPerSecond * Time.deltaTime;
        }

        private Xenolith RaycastForwardTarget()
        {
            float closestDistance = Mathf.Infinity;
            Collider closestCollider = null;

            for (float rayHeight = Canon.position.y; rayHeight > 0; rayHeight -= _raySliceKeenness)
            {
                Vector3 rayOrigin = Canon.position - new Vector3(0, rayHeight, 0);

                if (Physics.SphereCast(rayOrigin, SphereCastRadius,Canon.forward, out RaycastHit hit, Range, _targetLayers))
                {
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;
                        closestCollider = hit.collider;
                    }
                }
            }
            
            return closestCollider ? closestCollider.gameObject.GetComponent<Xenolith>() : null;
        }
        
        #endregion

        #region Actions

        public override void Operate(PlayerController currentController)
        {
            _firing = !_firing;
        }

        public override void Aim(InputValue input)
        {
            Vector2 value = input.Get<Vector2>();
            
            _turret.LookAt(_turret.position + new Vector3(value.x,90, value.y));
        }

        public override bool ExitModule(PlayerController currentController)
        {
            _firing = false;
            
            return base.ExitModule(currentController);
        }

        #endregion
    }
}
