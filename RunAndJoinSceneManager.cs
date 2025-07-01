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
    [SerializeField] TextMeshProUGUI DisplayTime;


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

        float timeRemaining = 60f; // 60秒からスタート

        while(timeRemaining > 0f)
        {
            UpdateMemberCountUI();
            // 時間を更新して表示（MM:SS形式）
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        DisplayTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        timeRemaining -= Time.deltaTime;
            
            yield return null;
        }
         // 残り時間が0になったら、00:00を表示してゲーム終了処理
    DisplayTime.text = "00:00";

    // プレイヤーと敵を止める
    player.IsStop = true;
    Enemy.IsStop = true;

    // 勝敗判定
    JudgeGame(Player, Enemy);
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
    if (JudgeUI.gameObject.activeSelf || IsFinishGame) return;

    JudgeUI.gameObject.SetActive(true);

    if (a.MemberCount > b.MemberCount)
    {
        JudgeText.text = $"{a.DisplayName} Win!!";
    }
    else if (a.MemberCount < b.MemberCount)
    {
        JudgeText.text = $"{b.DisplayName} Win!!";
    }
    else
    {
        JudgeText.text = "Draw!";
    }

    a.IsStop = true;
    b.IsStop = true;
}

    // Start is called before the first frame update
    void Start()
    {
        StartTitle();
    }
}