// Enemy.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Enemy : CharacterBase
{
    public Map Map;

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
        var buildingGridSize = Map.GridSize - new Vector3Int(1, 0, 1);
        var massSize = new Vector3(
            Map.Size.x / buildingGridSize.x,
            1,
            Map.Size.z / buildingGridSize.z);
        var start = new Vector3(-Map.Size.x / 2, 0, -Map.Size.z / 2);
        var oneLoopCount = (Map.GridSize.x * Map.GridSize.z);

        var offset = new Vector3(1, 0, 1);
        var rnd = new System.Random();
        while (true)
        {
            var startXIndex = (int)((transform.position.x - start.x + massSize.x * 0.5f) / massSize.x);
            var startZIndex = (int)((transform.position.z - start.z + massSize.z * 0.5f) / massSize.z);

            var nextXIndex = startXIndex;
            var nextZIndex = startZIndex;
            var i = rnd.Next(4);
            switch(i)
            {
                case 0: nextXIndex--; break;
                case 1: nextXIndex++; break;
                case 2: nextZIndex--; break;
                case 3: nextZIndex++; break;
            }
            nextXIndex = Mathf.Clamp(nextXIndex, 0, Map.GridSize.x-1);
            nextZIndex = Mathf.Clamp(nextZIndex, 0, Map.GridSize.z-1);
            var targetPos = new Vector3(
                start.x + massSize.x * nextXIndex,
                0,
                start.z + massSize.z * nextZIndex
            );

            yield return null;

            while (true)
            {
                var vec = (targetPos - transform.position);
                vec.y = 0;
                if (vec.magnitude < 1f) break;
                Move(vec.normalized * SpeedPerSeconds);
                yield return null;
            }
        }
    }

    protected override void Start()
    {
        StartMove();
    }
}