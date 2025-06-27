// Player.cs Player�R���|�[�l���g�̍쐬
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Player : CharacterBase
{
    MainCamera Camera { get => Object.FindObjectOfType<MainCamera>(); }

    Coroutine _currentCoroutine;

    public void StartMove()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(MoveCoroutine());
    }
    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            var velocity = GetVelocity();
            Move(velocity);
            Camera.LookAt(transform);
            yield return null;
        }
    }

    Vector3 GetVelocity()
    {
        var move = Vector3.zero;
        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");
        move *= SpeedPerSeconds;
        return move;
    }

    protected override void Start()
    {
        DisplayName = "Player";
        StartMove();
    }
}