using UnityEngine;
using UnityEngine.AI;

namespace Utilities {
    public class PathDebugger : MonoBehaviour {
        
        public bool DebugPath;
        public NavMeshAgent NavMeshAgent;

        private void OnDrawGizmos() {
            float pathWidth = 0.5f;

            if (NavMeshAgent.hasPath && DebugPath) {
                NavMeshPath path = NavMeshAgent.path;

                for (int i = 0; i < path.corners.Length - 1; i++) {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(path.corners[i] + Vector3.up * pathWidth, path.corners[i + 1]);
                    // Gizmos.DrawLine(path.corners[i] - Vector3.up * pathWidth, path.corners[i + 1] - Vector3.up * pathWidth);
                }
            }
        }

    }
}
