using UnityEngine;

public static class GizmoUtils
{
    public static void DrawArrow(Vector3 from, Vector3 to, Color color, float headLength = 0.25f, float headAngle = 20.0f)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(from, to);

        Vector3 direction = to - from;
        if (direction == Vector3.zero) return;

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + headAngle, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - headAngle, 0) * Vector3.forward;

        Gizmos.DrawLine(to, to + right * headLength);
        Gizmos.DrawLine(to, to + left * headLength);
    }
}
