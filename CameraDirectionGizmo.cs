using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirectionGizmo : MonoBehaviour
{
    public Transform target;                // �^�[�Q�b�g
    public Vector3 direction = new Vector3(0, -0.5f, 1);  // Direction�x�N�g��
    public float distance = 5f;             // ����

    private void OnDrawGizmos()
    {
        if (target == null) return;

        Vector3 start = target.position;
        Vector3 end = start - direction.normalized * distance;

        // ���ŕ\���i���������ύX�_�I�j
        GizmoUtils.DrawArrow(start, end, Color.green, 0.5f, 25f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(end, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(start, 0.1f);
    }
}