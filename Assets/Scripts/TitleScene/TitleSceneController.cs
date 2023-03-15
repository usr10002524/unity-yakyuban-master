using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// タイトルシーン制御クラス
/// </summary>
public class TitleSceneController : MonoBehaviour
{
    /// <summary>
    /// 先攻or後攻テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI textOrder;
    /// <summary>
    /// イニング数テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI textInnings;
    /// <summary>
    /// 自チーム名テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI textOwnTeam;
    /// <summary>
    /// 相手チーム名テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI textOtherTeam;
    /// <summary>
    /// 成績ボタンオブジェクト
    /// </summary>
    [SerializeField] private GameObject recordButtonObject;


    /// <summary>
    /// チーム名クラス
    /// </summary>
    private TeamName teamName;
    /// <summary>
    /// 選択中の先攻or後攻インデックス
    /// </summary>
    private int indexOrder;
    /// <summary>
    /// 選択中のイニング数インデックス
    /// </summary>
    private int indexInnings;
    /// <summary>
    /// 選択中のコールドスコアインデックス
    /// </summary>
    private int indexCalledScore;

    /// <summary>
    /// 先攻or後攻の選択項目
    /// </summary>
    private static readonly Core.Order[] settingOrder = new Core.Order[2] {
        Core.Order.First,
        Core.Order.Second
    };

    /// <summary>
    /// イニング数の選択項目
    /// </summary>
    private static readonly int[] settingInnings = new int[5] { 1, 3, 5, 7, 9 };
    /// <summary>
    /// コールドスコアの選択コム
    /// </summary>
    /// <value></value>
    private static readonly int[] settingCalledScore = new int[1] { 10 };


    /// <summary>
    /// 先攻or後攻の項目を次に変更する
    /// </summary>
    public void NextOrder()
    {
        indexOrder++;
        indexOrder %= settingOrder.Length;
        RedrawOrder();
    }

    /// <summary>
    /// 先攻or後攻の項目を前に変更する
    /// </summary>
    public void PrevOrder()
    {
        indexOrder += settingOrder.Length;
        indexOrder--;
        indexOrder %= settingOrder.Length;
        RedrawOrder();
    }

    /// <summary>
    /// イニング数の項目を次に変更する
    /// </summary>
    public void NextInnings()
    {
        indexInnings++;
        indexInnings %= settingInnings.Length;
        RedrawInnings();
    }

    /// <summary>
    /// イニング数の項目を前に変更する
    /// </summary>
    public void PrevInnings()
    {
        indexInnings += settingInnings.Length;
        indexInnings--;
        indexInnings %= settingInnings.Length;
        RedrawInnings();
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
    public void RerollOtherTeam()
    {
        teamName.SetSecondTeam();
        RedrawOtherTeam();
    }

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void StartGame()
    {
        Core.Order order = settingOrder[indexOrder];
        GameSettings.Instance.SetOrder(order);

        int innings = settingInnings[indexInnings];
        GameSettings.Instance.SetInnings(innings);

        int calledScore = settingCalledScore[indexCalledScore];
        GameSettings.Instance.SetCalledScore(calledScore);

        if (order == Core.Order.First)
        {
            GameSettings.Instance.SetFirstTeamName(teamName.GetFirstTeam());
            GameSettings.Instance.SetSecondTeamName(teamName.GetSecondTeam());
        }
        else
        {
            GameSettings.Instance.SetFirstTeamName(teamName.GetSecondTeam());
            GameSettings.Instance.SetSecondTeamName(teamName.GetFirstTeam());
        }

        Initiate.Fade("MainScene", FadeConst.sceneTransitColor, FadeConst.fadeDump, FadeConst.sceneTransitSortOrder);
    }

    /// <summary>
    /// データシーンへ遷移する
    /// </summary>
    public void ShowScore()
    {
        Initiate.Fade("DataScene", FadeConst.sceneTransitColor, FadeConst.fadeDump, FadeConst.sceneTransitSortOrder);
    }


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        indexOrder = 0;
        indexInnings = 1;
        indexCalledScore = 0;

        // order = settingOrder[indexOrder];
        // innings = settingInnings[indexInnings];

        teamName = new TeamName();
        teamName.SetFirstTeam();
        teamName.SetSecondTeam();
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        RedrawOrder();
        RedrawInnings();
        RedrawOwnTeam();
        RedrawOtherTeam();
    }

    /// <summary>
    /// 先行or後攻の項目を再描画する
    /// </summary>
    private void RedrawOrder()
    {
        Core.Order order = settingOrder[indexOrder];

        string text = "";
        switch (order)
        {
            case Core.Order.First: text = "先攻"; break;
            case Core.Order.Second: text = "後攻"; break;
        }

        textOrder.text = text;
    }

    /// <summary>
    /// イニング数の項目を再描画する
    /// </summary>
    private void RedrawInnings()
    {
        int innigs = settingInnings[indexInnings];
        string text = string.Format("{0}回", innigs);

        textInnings.text = text;
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
}
