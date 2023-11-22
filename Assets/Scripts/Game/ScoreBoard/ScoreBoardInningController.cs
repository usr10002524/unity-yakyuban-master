using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// スコアボード各イニングの制御
/// </summary>
public class ScoreBoardInningController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inningText;
    [SerializeField] private TextMeshProUGUI firstScoreText;
    [SerializeField] private TextMeshProUGUI secondScoreText;

    /// <summary>
    /// 指定したイニングのスコアを再描画する
    /// </summary>
    /// <param name="inning">イニング数</param>
    /// <param name="firstScore">先攻のスコア（文字列）</param>
    /// <param name="secondScore">後攻のスコア（文字列）</param>
    public void Redraw(string inning, string firstScore, string secondScore)
    {
        if (inningText != null)
        {
            inningText.text = inning;
        }
        if (firstScoreText != null)
        {
            firstScoreText.text = firstScore;
        }
        if (secondScoreText != null)
        {
            secondScoreText.text = secondScore;
        }
    }

    /// <summary>
    /// スコアを再描画する
    /// </summary>
    /// <param name="firstScore"></param>
    /// <param name="secondScore"></param>
    public void RedrawScore(string firstScore, string secondScore)
    {
        if (firstScoreText != null)
        {
            firstScoreText.text = firstScore;
        }
        if (secondScoreText != null)
        {
            secondScoreText.text = secondScore;
        }
    }
}
