using Game.Convoy.Modules;
using UnityEngine;

namespace Game.Animation
{
    public class LaserBeam : MonoBehaviour
    {
        public bool EnableBeam;
        [Space]
        public Laser Module;
        public int LineVertices;
        
        private LineRenderer Renderer { get; set; }
        private Vector3 LaserEndPoint { get; set; }
        private Vector3[] Line { get; set; }

        #region Debug

        void OnDrawGizmos()
        {
            if (!EnableBeam) return;
            
            Vector3[] localLinePoints = new Vector3[Line.Length];

            for (int i = 0; i < Line.Length; i++) {
                localLinePoints[i] = transform.InverseTransformPoint(Line[i]);
            }

            for (int i = 0; i < localLinePoints.Length - 1; i++) {
                Gizmos.DrawLine(localLinePoints[i], localLinePoints[i + 1]);
            }
        }

        #endregion
        
        private void Awake()
        {
            Renderer = GetComponent<LineRenderer>();
            Line = new Vector3[LineVertices];
            Line[0] = transform.InverseTransformPoint(transform.position);
        }

        private void Start()
        {
            EnableBeam = false;
            Renderer.positionCount = LineVertices;
        }

        void Update()
        {
            if (EnableBeam)
            {
                Renderer.enabled = true;
                Vector3 rayOrigin = Module.Canon.position - new Vector3(0, Module.Canon.position.y, 0);

                if (Physics.SphereCast(rayOrigin, Module.SphereCastRadius, Module.Canon.forward, out RaycastHit hit, Module.Range, 1 << LayerMask.NameToLayer("Xenolith")))
                    LaserEndPoint = hit.point;
                else
                    LaserEndPoint = Module.Canon.forward * Module.Range;

                Vector3 localEndPoint = transform.InverseTransformPoint(LaserEndPoint);
                
                for (int i = 1; i < LineVertices - 1; i++) {
                    float t = (float)i / (LineVertices - 2);
                    Line[i] = Vector3.Lerp(Vector3.zero, localEndPoint, t);
                }
                Line[LineVertices - 1] = localEndPoint;
                
                Renderer.SetPositions(Line);
            }
            else
            {
                Renderer.enabled = false;
            }
        }

        public void Activate()
        {
            EnableBeam = true;
        }
        
        public void Disable()
        {
            EnableBeam = false;
        }
    }
}
