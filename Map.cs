// Map.cs Map�R���|�[�l���g�̍쐬
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Map : MonoBehaviour
{
    public GameObject BuildingPrefab;
    public GameObject Ground;
   
    public Vector3 Size = new Vector3(10, 1, 10);
    public Vector3Int GridSize = new Vector3Int(5, 1, 5);

    public void BuildMap()
    {
        Ground.transform.localScale = Size / 10 + Vector3.one * 0.5f;

        var buildingGridSize = GridSize - new Vector3Int(1, 0, 1);
        var massSize = new Vector3(
            Size.x / buildingGridSize.x,
            1,
            Size.z / buildingGridSize.z);
        var start = new Vector3(-Size.x / 2, 0, -Size.z / 2) + massSize * 0.5f;
        for (var z = 0; z < buildingGridSize.z; ++z)
        {
            for (var x = 0; x < buildingGridSize.x; ++x)
            {
                var inst = Object.Instantiate(BuildingPrefab, transform);
                inst.transform.localPosition = start + new Vector3(
                    x * Size.x / buildingGridSize.x,
                    0,
                    z * Size.z / buildingGridSize.z
                );
            }
        }
    }

    void Awake()
    {
        
    }

    public bool IsInMap(Vector3 pos)
    {
        var min = -Size / 2;
        var max = min + Size;
        return min.x <= pos.x && pos.x <= max.x
            && min.z <= pos.z && pos.z <= max.z;
    }

    public Vector3 ClampPos(Vector3 pos)
    {
        var min = -Size / 2;
        var max = min + Size;
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.z = Mathf.Clamp(pos.z, min.z, max.z);
        return pos;
    }
    
     public JoinableCharacter JoinableCharacterPrefab;
    [Range(0, 100)] public int InitJoinableCharacterCount = 20;
    public Vector3 SpawnOffset = new Vector3(0.5f, 0, 0.5f);
 
    /*public void InitJoinableCharacter()
    {
        var buildingGridSize = GridSize - new Vector3Int(1, 0, 1);
        var massSize = new Vector3(
            Size.x / buildingGridSize.x,
            1,
            Size.z / buildingGridSize.z);
        var start = new Vector3(-Size.x / 2, 0, -Size.z / 2);
        var oneLoopCount = (GridSize.x * GridSize.z);
        for (var i=0; i< InitJoinableCharacterCount; ++i)
        {
            var obj = Object.Instantiate(JoinableCharacterPrefab, transform);
 
            var xIndex = (i % oneLoopCount) % GridSize.x;
            var zIndex = (i % oneLoopCount) / GridSize.x;
            var loopCount = i / oneLoopCount;
            obj.transform.localPosition = new Vector3(
                start.x + massSize.x * xIndex,
                0,
                start.z + massSize.z * zIndex
            ) + SpawnOffset * loopCount;
        }
    }*/
    public Transform Player; // プレイヤーのTransformをインスペクターで設定してください
public float MinDistanceFromPlayer = 1.0f;

public void InitJoinableCharacter()
{
    var buildingGridSize = GridSize - new Vector3Int(1, 0, 1);
    var massSize = new Vector3(
        Size.x / buildingGridSize.x,
        1,
        Size.z / buildingGridSize.z);
    var start = new Vector3(-Size.x / 2, 0, -Size.z / 2);
    var oneLoopCount = (GridSize.x * GridSize.z);

    int createdCount = 0;
    int i = 0;
    while (createdCount < InitJoinableCharacterCount)
    {
        var xIndex = (i % oneLoopCount) % GridSize.x;
        var zIndex = (i % oneLoopCount) / GridSize.x;
        var loopCount = i / oneLoopCount;
        var candidatePos = new Vector3(
            start.x + massSize.x * xIndex,
            0,
            start.z + massSize.z * zIndex
        ) + SpawnOffset * loopCount;

        if (Player == null || Vector3.Distance(candidatePos, Player.position) >= MinDistanceFromPlayer)
        {
            var obj = Object.Instantiate(JoinableCharacterPrefab, transform);
            obj.transform.localPosition = candidatePos;
            createdCount++;
        }

        i++;
        // 無限ループ防止（マップが小さくて全ての位置がプレイヤーに近すぎるとき）
        if (i > oneLoopCount * 10) break;
    }
}


    public Enemy EnemyPrefab;
 
    public void InitEnemy()
    {
        var buildingGridSize = GridSize - new Vector3Int(1, 0, 1);
        var massSize = new Vector3(
            Size.x / buildingGridSize.x,
            1,
            Size.z / buildingGridSize.z);
        var start = new Vector3(-Size.x / 2, 0, -Size.z / 2);
        var oneLoopCount = (GridSize.x * GridSize.z);
 
        var rnd = new System.Random();
        var i = rnd.Next(oneLoopCount);
 
        var obj = Object.Instantiate(EnemyPrefab, transform);
        obj.Map = this;
        obj.DisplayName = "Enemy";
        var xIndex = i % GridSize.x;
        var zIndex = i / GridSize.x;
        obj.transform.localPosition = new Vector3(
            start.x + massSize.x * xIndex,
            2,
            start.z + massSize.z * zIndex
        );
    }
 
  public void InitMap()
    {
        // まず、JoinableCharacter の FollowedTarget を null にする
    foreach (Transform child in transform)
    {
        var joinable = child.GetComponent<JoinableCharacter>();
        if (joinable != null)
        {
            joinable.FollowedTarget = null;
        }
    }

    // その後に敵などを削除
    foreach (Transform child in transform)
    {
        if (child.gameObject == Ground) continue;
        Object.Destroy(child.gameObject);
    }
        BuildMap();
        InitJoinableCharacter();
        InitEnemy();
    }
   

}