using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirectionGizmo : MonoBehaviour
{
    public Transform target;                // ターゲット
    public Vector3 direction = new Vector3(0, -0.5f, 1);  // Directionベクトル
    public float distance = 5f;             // 距離

    private void OnDrawGizmos()
    {
        if (target == null) return;

        Vector3 start = target.position;
        Vector3 end = start - direction.normalized * distance;

        // 矢印で表示（←ここが変更点！）
        GizmoUtils.DrawArrow(start, end, Color.green, 0.5f, 25f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(end, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(start, 0.1f);
    }
}