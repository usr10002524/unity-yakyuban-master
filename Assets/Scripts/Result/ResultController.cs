using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 結果表示制御クラス
/// </summary>
public class ResultController : MonoBehaviour
{
    /// <summary>
    /// 先攻チーム名
    /// </summary>
    [SerializeField] private TextMeshProUGUI firstName;
    /// <summary>
    /// 先攻チームのスコア
    /// </summary>
    [SerializeField] private TextMeshProUGUI firstScore;
    /// <summary>
    /// 後攻チーム名
    /// </summary>
    [SerializeField] private TextMeshProUGUI secondName;
    /// <summary>
    /// 後攻チームのスコア
    /// </summary>
    [SerializeField] private TextMeshProUGUI secondScore;
    /// <summary>
    /// タイトルに戻るボタンのゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject returnTitleButton;

    /// <summary>
    /// 先攻チーム名
    /// </summary>
    private string firstTeamName = "YOU";
    /// <summary>
    /// 後攻チーム名
    /// </summary>
    private string secondTeamName = "CPU";

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void OnInit()
    {
        firstTeamName = GameManager.Instance.GetFirstTeamName();
        secondTeamName = GameManager.Instance.GetSecondTeamName();
        Show(false);
        returnTitleButton.SetActive(false);
    }

    /// <summary>
    /// 再描画する
    /// </summary>
    public void OnRedraw()
    {
        RedrawName();
        RedrawScore();
    }

    /// <summary>
    /// 表示/非表示をセットする
    /// </summary>
    /// <param name="flag">表示フラグ</param>
    public void Show(bool flag)
    {
        gameObject.SetActive(flag);
    }

    /// <summary>
    /// タイトルに戻るボタンを表示する
    /// </summary>
    public void ShowReturnTitleButton()
    {
        returnTitleButton.SetActive(true);
    }

    /// <summary>
    /// タイトルに戻る処理を行う
    /// </summary>
    public void ReturnTitle()
    {
        Initiate.Fade("TitleScene", FadeConst.sceneTransitColor, FadeConst.fadeDump, FadeConst.sceneTransitSortOrder);
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

}
