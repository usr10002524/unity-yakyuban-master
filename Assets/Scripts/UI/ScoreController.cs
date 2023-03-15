using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// スコア表示制御クラス
/// </summary>
public class ScoreController : MonoBehaviour
{
    /// <summary>
    /// 自チーム名テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI firstName;
    /// <summary>
    /// 自チームのスコアテキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI firstScore;
    /// <summary>
    /// 相手チーム名テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI secondName;
    /// <summary>
    /// 相手チームスコアテキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI secondScore;
    /// <summary>
    /// 先攻攻撃中表示オブジェクト
    /// </summary>
    [SerializeField] private GameObject firstInning;
    /// <summary>
    /// 後攻攻撃中オブジェクト
    /// </summary>
    [SerializeField] private GameObject secondInning;

    private string firstTeamName = "YOU";
    private string secondTeamName = "CPU";

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init()
    {
        firstTeamName = GameManager.Instance.GetFirstTeamName();
        secondTeamName = GameManager.Instance.GetSecondTeamName();
    }

    /// <summary>
    /// 再描画処理
    /// </summary>
    public void Redraw()
    {
        RedrawName();
        RedrawScore();
        RedrawInning();
    }

    /// <summary>
    /// チーム名を再描画する
    /// </summary>
    private void RedrawName()
    {
        firstName.text = firstTeamName;
        secondName.text = secondTeamName;
    }

    /// <summary>
    /// スコアを再描画する
    /// </summary>
    private void RedrawScore()
    {
        Score score = GameManager.Instance.GetScore();

        firstScore.text = string.Format("{0}", score.GetTotal(Core.Order.First));
        secondScore.text = string.Format("{0}", score.GetTotal(Core.Order.Second));
    }

    /// <summary>
    /// 攻撃中チーム表示を再描画する
    /// </summary>
    private void RedrawInning()
    {
        Core.Order order = GameManager.Instance.GetCurrentOrder();

        if (order == Core.Order.First)
        {
            firstInning.SetActive(true);
            secondInning.SetActive(false);
        }
        else
        {
            firstInning.SetActive(false);
            secondInning.SetActive(true);
        }
    }
}
