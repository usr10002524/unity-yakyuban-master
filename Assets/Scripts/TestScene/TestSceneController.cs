using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// テストシーン制御クラス
/// </summary>
public class TestSceneController : MonoBehaviour
{
    /// <summary>
    /// イニング数テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI textInning;
    /// <summary>
    /// スコアテキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI textScore;
    /// <summary>
    /// 自チーム名テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI textOwnTeam;
    /// <summary>
    /// 相手チーム名テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI textOtherTeam;
    /// <summary>
    /// 時刻テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI textClock;

    /// <summary>
    /// イニング数
    /// </summary>
    private int inning;
    /// <summary>
    /// スコア1桁目
    /// </summary>
    private int scoreNum1;
    /// <summary>
    /// スコア2桁目
    /// </summary>
    private int scoreNum2;
    /// <summary>
    /// スコア3桁目
    /// </summary>
    private int scoreNum3;
    /// <summary>
    /// スコア4桁目
    /// </summary>
    private int scoreNum4;
    /// <summary>
    /// スコア5桁目
    /// </summary>
    private int scoreNum5;
    /// <summary>
    /// チーム名クラス
    /// </summary>
    private TeamName teamName;
    /// <summary>
    /// 現在時刻
    /// </summary>
    private DateTime dateTime;

    /// <summary>
    /// イニング数を加算
    /// </summary>
    public void IncInning()
    {
        switch (inning)
        {
            case 1: inning = 3; break;
            case 3: inning = 5; break;
            case 5: inning = 7; break;
            case 7: inning = 9; break;
            case 9: inning = 1; break;
            default: inning = 1; break;
        }
        RedrawInning();
    }

    /// <summary>
    /// イニング数を減産
    /// </summary>
    public void DecInning()
    {
        switch (inning)
        {
            case 1: inning = 9; break;
            case 3: inning = 1; break;
            case 5: inning = 3; break;
            case 7: inning = 5; break;
            case 9: inning = 7; break;
            default: inning = 1; break;
        }
        RedrawInning();
    }

    /// <summary>
    /// 指定した桁の数字をインクリメントする
    /// </summary>
    /// <param name="keta">桁数</param>
    public void IncScore(int keta)
    {
        switch (keta)
        {
            case 1: scoreNum1 = (scoreNum1 + 1) % 10; break;
            case 2: scoreNum2 = (scoreNum2 + 1) % 10; break;
            case 3: scoreNum3 = (scoreNum3 + 1) % 10; break;
            case 4: scoreNum4 = (scoreNum4 + 1) % 10; break;
            case 5: scoreNum5 = (scoreNum5 + 1) % 10; break;
        }
        RedrawScore();
    }

    /// <summary>
    /// 指定した桁の数字をデクリメントする
    /// </summary>
    /// <param name="keta">桁数</param>
    public void DecScore(int keta)
    {
        switch (keta)
        {
            case 1: scoreNum1 = (scoreNum1 + 10 - 1) % 10; break;
            case 2: scoreNum2 = (scoreNum2 + 10 - 1) % 10; break;
            case 3: scoreNum3 = (scoreNum3 + 10 - 1) % 10; break;
            case 4: scoreNum4 = (scoreNum4 + 10 - 1) % 10; break;
            case 5: scoreNum5 = (scoreNum5 + 10 - 1) % 10; break;
        }
        RedrawScore();
    }

    /// <summary>
    /// 自チーム名を再設定する
    /// </summary>
    public void RerollOwnTeam()
    {
        teamName.SetFirstTeam();
        RedrawOwnTeam();
    }

    /// <summary>
    /// 相手チーム名を再設定する
    /// </summary>
    public void RerollOhterTeam()
    {
        teamName.SetSecondTeam();
        RedrawOtherTeam();
    }

    /// <summary>
    /// 時刻を再取得する
    /// </summary>
    public void RerollClock()
    {
        dateTime = DateTime.Now;
        RedrawClock();
    }

    /// <summary>
    /// スコアボードを表示する
    /// </summary>
    public void ShowScoreBoard()
    {
        AtsumaruAPI.Instance.DisplayScoreBoard(inning);
    }

    /// <summary>
    /// プレーヤーデータを保存する
    /// </summary>
    public void SaveData()
    {
#if false
        // if (!AtsumaruAPI.Instance.IsValid())
        // {
        //     return;
        // }

        int score = ToScore();

        // スコアを保存
        {
            AtsumaruAPI.Instance.SaveScore(inning, score);
        }

        // 自己ベストを記録し、保存
        {
            GameManager gameManager = GameManager.Instance;

            string ownTeamName = teamName.GetFirstTeam();
            string otherTeamName = teamName.GetSecondTeam();

            long unixTime = EpochTime.ToUnixTime(dateTime);

            // 自己ベストを更新
            RecordData recordData = new RecordData(ownTeamName, otherTeamName, score, unixTime);
            RecordManager.Instance.AddData(inning, recordData);

            MyRecords myRecords = RecordManager.Instance.Create();

            AtsumaruAPI.ServerDataItems serverDataItems = new AtsumaruAPI.ServerDataItems();
            serverDataItems.data = new AtsumaruAPI.DataItem[1];

            AtsumaruAPI.DataItem item = new AtsumaruAPI.DataItem();
            item.key = MyRecords.key;
            item.value = JsonUtility.ToJson(myRecords);
            Debug.Log(string.Format("{0}", item.value));
            serverDataItems.data[0] = item;

            string json = JsonUtility.ToJson(serverDataItems);
            Debug.Log(string.Format("{0}", json));
            AtsumaruAPI.Instance.SaveServerData(json);
        }
#else
        if (AtsumaruAPI.Instance.IsValid())
        {
            SaveWithAtsumaruAPI();
        }
        else
        {
            SaveWithNoAtsumaruAPI();
        }
#endif
    }

    /// <summary>
    /// AtumaruAPIを使用してデータを保存する
    /// </summary>
    private void SaveWithAtsumaruAPI()
    {
        int score = ToScore();

        // スコアを保存
        {
            AtsumaruAPI.Instance.SaveScore(inning, score);
        }

        // 自己ベストを記録し、保存
        {
            GameManager gameManager = GameManager.Instance;

            string ownTeamName = teamName.GetFirstTeam();
            string otherTeamName = teamName.GetSecondTeam();

            long unixTime = EpochTime.ToUnixTime(dateTime);

            // 自己ベストを更新
            RecordData recordData = new RecordData(ownTeamName, otherTeamName, score, unixTime);
            RecordManager.Instance.AddData(inning, recordData);

            MyRecords myRecords = RecordManager.Instance.Create();

            AtsumaruAPI.ServerDataItems serverDataItems = new AtsumaruAPI.ServerDataItems();
            serverDataItems.data = new AtsumaruAPI.DataItem[1];

            AtsumaruAPI.DataItem item = new AtsumaruAPI.DataItem();
            item.key = MyRecords.key;
            item.value = JsonUtility.ToJson(myRecords);
            Debug.Log(string.Format("SaveWithAtsumaruAPI {0}", item.value));
            serverDataItems.data[0] = item;

            string json = JsonUtility.ToJson(serverDataItems);
            Debug.Log(string.Format("SaveWithAtsumaruAPI {0}", json));
            AtsumaruAPI.Instance.SaveServerData(json);
        }
    }

    /// <summary>
    /// AtsumaruAPIを使用せずデータを保存する
    /// </summary>
    private void SaveWithNoAtsumaruAPI()
    {
        int score = ToScore();

        // スコアを保存
        {
            // なにもしない
        }

        // 自己ベストを記録し、保存
        {
            GameManager gameManager = GameManager.Instance;

            string ownTeamName = teamName.GetFirstTeam();
            string otherTeamName = teamName.GetSecondTeam();

            long unixTime = EpochTime.ToUnixTime(dateTime);

            // 自己ベストを更新
            RecordData recordData = new RecordData(ownTeamName, otherTeamName, score, unixTime);
            RecordManager.Instance.AddData(inning, recordData);

            MyRecords myRecords = RecordManager.Instance.Create();

            AtsumaruAPI.ServerDataItems serverDataItems = new AtsumaruAPI.ServerDataItems();
            serverDataItems.data = new AtsumaruAPI.DataItem[1];

            AtsumaruAPI.DataItem item = new AtsumaruAPI.DataItem();
            item.key = MyRecords.key;
            item.value = JsonUtility.ToJson(myRecords);
            Debug.Log(string.Format("SaveWithNoAtsumaruAPI {0}", item.value));
            serverDataItems.data[0] = item;

            string json = JsonUtility.ToJson(serverDataItems);
            Debug.Log(string.Format("SaveWithNoAtsumaruAPI {0}", json));
            LocalStorageAPI.Instance.SaveLocalData(json);
        }
    }

    /// <summary>
    /// データシーンへ遷移する
    /// </summary>
    public void TransitDataScene()
    {
        SceneManager.LoadScene("DataScene");
    }


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        inning = 1;
        scoreNum1 = 0;
        scoreNum2 = 0;
        scoreNum3 = 0;
        scoreNum4 = 0;
        scoreNum5 = 0;

        teamName = new TeamName();
        teamName.SetFirstTeam();
        teamName.SetSecondTeam();

        dateTime = DateTime.Now;
    }

    /// <summary>
    /// Start();
    /// </summary>
    private void Start()
    {
        RedrawInning();
        RedrawScore();
        RedrawOwnTeam();
        RedrawOtherTeam();
        RedrawClock();
    }

    /// <summary>
    /// 各桁数を組み合わせてスコアを生成する
    /// </summary>
    /// <returns>スコア</returns>
    private int ToScore()
    {
        int ret = 0;
        ret += scoreNum5 * 10000;
        ret += scoreNum4 * 1000;
        ret += scoreNum3 * 100;
        ret += scoreNum2 * 10;
        ret += scoreNum1 * 1;
        return ret;
    }

    /// <summary>
    /// イニング数を再描画する
    /// </summary>
    private void RedrawInning()
    {
        textInning.text = string.Format("{0}", inning);
    }

    /// <summary>
    /// スコアを再描画する
    /// </summary>
    private void RedrawScore()
    {
        int score = ToScore();
        textScore.text = string.Format("{0}", score);
    }

    /// <summary>
    /// 自チーム名を再描画する
    /// </summary>
    private void RedrawOwnTeam()
    {
        textOwnTeam.text = teamName.GetFirstTeam();
    }

    /// <summary>
    /// 相手チーム名を再描画する
    /// </summary>
    private void RedrawOtherTeam()
    {
        textOtherTeam.text = teamName.GetSecondTeam();
    }

    /// <summary>
    /// 時刻を再描画する
    /// </summary>
    private void RedrawClock()
    {
        string text = string.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}"
            , dateTime.Year, dateTime.Month, dateTime.Day
            , dateTime.Hour, dateTime.Minute, dateTime.Second);
        textClock.text = text;
    }
}
