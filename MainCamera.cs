// Camera.cs Camera�R���|�[�l���g�̍쐬
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MainCamera : MonoBehaviour
{
    public Vector3 Direction = new Vector3(0, -0.5f, 1);
    [Range(0.1f, 100)] public float Distance = 5f;
    public void LookAt(Transform Target)
    {
        transform.position = Target.position - Direction.normalized * Distance;
        transform.LookAt(Target);
    }
}