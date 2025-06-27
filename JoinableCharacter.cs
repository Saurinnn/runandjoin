// JoinableCharacter.cs JoinableCharacterコンポーネントの作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class JoinableCharacter : CharacterBase
{
    public enum State
    {
        Free,
        Joined,
    }

    [SerializeField] CharacterBase _followedTarget;
    [SerializeField] State CurrentState;

    [Range(0.5f, 15f)] public float FreeMoveSeconds = 3f;
    [Range(0.5f, 15f)] public float FreeWaitSeconds = 3f;

    public CharacterBase FollowedTarget
    {
        get => _followedTarget;
        set
        {
            if (_followedTarget == value) return;
            _followedTarget = value;
            if(value == null)
            {
                StartFree();
            }
            else
            {
                ChangeTeamColor(_followedTarget.TeamColor);
                StartJoined();
            }
        }
    }

    public bool IsJoinable(CharacterBase other)
    {
        return other != FollowedTarget && !(other is JoinableCharacter);
    }

    Coroutine _currentCoroutine;

    public void StartFree()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        CurrentState = State.Free;
        _currentCoroutine = StartCoroutine(FreeCoroutine());
    }
    IEnumerator FreeCoroutine()
    {
        var rnd = new System.Random(gameObject.GetInstanceID() + System.DateTime.Now.Millisecond);
        while (true)
        {
            var t = 0f;
            var vec = Vector3.zero;
            vec.x = (float)rnd.NextDouble() * 2 - 1;
            vec.z = (float)rnd.NextDouble() * 2 - 1;
            vec.Normalize();
            vec *= SpeedPerSeconds;
            while (t < FreeMoveSeconds)
            {
                t += Time.deltaTime;
                Move(vec);
                yield return null;
            }

            yield return new WaitForSeconds(FreeWaitSeconds);
        }
    }

    public void StartJoined()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        CurrentState = State.Joined;
        _currentCoroutine = StartCoroutine(JoinedCoroutine());
    }
    IEnumerator JoinedCoroutine()
    {
        while (true)
        {
            var vec = (FollowedTarget.transform.position - transform.position);
            vec.y = 0;
            Move(vec.normalized * SpeedPerSeconds);
            yield return null;
        }
    }

    protected override void Start()
    {
        StartFree();
    }

}