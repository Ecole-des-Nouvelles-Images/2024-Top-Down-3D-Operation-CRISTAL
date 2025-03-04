using UnityEngine;

namespace Editor.Utilities
{
    public static class Debug
    {
        public static void DrawCapsule(CapsuleCollider collider, Color color)
        {
            Gizmos.color = color;
            Gizmos.matrix = collider.transform.localToWorldMatrix;
 
            Vector3 offset = Vector3.zero;
            offset[collider.direction] = collider.height * 0.5f - collider.radius;
            DrawWireCapsule(collider.center + offset, collider.center - offset, collider.radius);
            Gizmos.color = Color.white;
        }
        
        public static void DrawWireCapsule(Vector3 p1, Vector3 p2, float radius)
        {
            // Special case when both points are in the same position
            if (p1 == p2)
            {
                // DrawWireSphere works only in gizmo methods
                Gizmos.DrawWireSphere(p1, radius);
                return;
            }
            using (new UnityEditor.Handles.DrawingScope(Gizmos.color, Gizmos.matrix))
            {
                Quaternion p1Rotation = Quaternion.LookRotation(p1 - p2);
                Quaternion p2Rotation = Quaternion.LookRotation(p2 - p1);
                
                // Check if capsule direction is collinear to Vector.up
                float c = Vector3.Dot((p1 - p2).normalized, Vector3.up);
                if (Mathf.Approximately(c, 1f) || Mathf.Approximately(c, -1f))
                {
                    // Fix rotation
                    p2Rotation = Quaternion.Euler(p2Rotation.eulerAngles.x, p2Rotation.eulerAngles.y + 180f, p2Rotation.eulerAngles.z);
                }
                // First side
                UnityEditor.Handles.DrawWireArc(p1, p1Rotation * Vector3.left,  p1Rotation * Vector3.down, 180f, radius);
                UnityEditor.Handles.DrawWireArc(p1, p1Rotation * Vector3.up,  p1Rotation * Vector3.left, 180f, radius);
                UnityEditor.Handles.DrawWireDisc(p1, (p2 - p1).normalized, radius);
                // Second side
                UnityEditor.Handles.DrawWireArc(p2, p2Rotation * Vector3.left,  p2Rotation * Vector3.down, 180f, radius);
                UnityEditor.Handles.DrawWireArc(p2, p2Rotation * Vector3.up,  p2Rotation * Vector3.left, 180f, radius);
                UnityEditor.Handles.DrawWireDisc(p2, (p1 - p2).normalized, radius);
                // Lines
                UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.down * radius, p2 + p2Rotation * Vector3.down * radius);
                UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.left * radius, p2 + p2Rotation * Vector3.right * radius);
                UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.up * radius, p2 + p2Rotation * Vector3.up * radius);
                UnityEditor.Handles.DrawLine(p1 + p1Rotation * Vector3.right * radius, p2 + p2Rotation * Vector3.left * radius);
            }
        }
    }
}
