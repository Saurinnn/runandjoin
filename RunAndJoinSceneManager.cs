// RunAndJoinSceneManager.cs RunAndJoinSceneManagerコンポーネント
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

class RunAndJoinSceneManager : MonoBehaviour
{
    [SerializeField] Canvas UI;
    [SerializeField] Canvas TitleUI;
    [SerializeField] TextMeshProUGUI PlayerMemberCountText;
    [SerializeField] TextMeshProUGUI EnemyMemberCountText;
    [SerializeField] Vector3 PlayerStartPos = new Vector3(0, 0, 0);
    [SerializeField] Canvas JudgeUI;
    [SerializeField] TextMeshProUGUI JudgeText;


    Player Player { get => Object.FindObjectOfType<Player>(); }
    Enemy Enemy { get => Object.FindObjectOfType<Enemy>(); }

    public void UpdateMemberCountUI()
    {
        var player = Player;
        PlayerMemberCountText.text = player != null ? player.MemberCount.ToString() : "--";

        var enemy = Enemy;
        EnemyMemberCountText.text = enemy != null ? enemy.MemberCount.ToString() : "--";
    }

    Coroutine _currentCoroutine;

    public void StartGame()
    {
        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        UI.gameObject.SetActive(true);
        TitleUI.gameObject.SetActive(false);
        _currentCoroutine = StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        var map = Object.FindObjectOfType<Map>();
        map.InitMap();
        var player = Player;
        player.transform.position = PlayerStartPos;
        player.MemberCount = 0;
        player.IsStop = false;

        while(true)
        {
            UpdateMemberCountUI();
            yield return null;
        }
    }

    public void StartTitle()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        TitleUI.gameObject.SetActive(true);
        UI.gameObject.SetActive(false);
        JudgeUI.gameObject.SetActive(false);
        _currentCoroutine = StartCoroutine(TitleLoop());
    }
 
    IEnumerator TitleLoop()
    {
        while (true)
        {
            UpdateMemberCountUI();
            yield return null;
        }
    }

    public bool IsFinishGame { get => JudgeUI.gameObject.activeSelf; }
    public void JudgeGame(CharacterBase a, CharacterBase b)
    {
        if (JudgeUI.gameObject.activeSelf || IsFinishGame || a.MemberCount == b.MemberCount) return;
 
        JudgeUI.gameObject.SetActive(true);
        JudgeText.text = $"{(a.MemberCount > b.MemberCount ? a.DisplayName : b.DisplayName)} Win!!";
        a.IsStop = true;
        b.IsStop = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartTitle();
    }
}