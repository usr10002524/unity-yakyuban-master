using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ゲームスコアクラス
/// </summary>
public class GameScoreController : MonoBehaviour
{
    /// <summary>
    /// イニングで「ビッグイニング」を獲得済みかどうか
    /// </summary>
    private bool alreadyBigInning;

    /// <summary>
    /// ノーヒットノーラン、完全試合が獲得できる最小イニング
    /// </summary>
    private static readonly int MinimumInnings = 5;

    /// <summary>
    /// イニングの初期化処理
    /// </summary>
    public void StartInning()
    {
        alreadyBigInning = false;
    }

    /// <summary>
    /// プレー結果を反映させる
    /// </summary>
    /// <param name="isBatting">攻撃中かどうか</param>
    /// <param name="result">プレー結果</param>
    /// <param name="isStruckOut">三振かどうか</param>
    public void SetPlayResult(bool isBatting, Core.Result result, bool isStruckOut)
    {
        if (isBatting)
        {
            // 攻撃中
            {
                ScoreConst.Data data = ScoreManager.Instance.MakeBattingResult(result);
                if (data != null)
                {
                    ScoreManager.Instance.Push(data);
                }
            }
        }
        else
        {
            // 守備中
            {
                ScoreConst.Data data = ScoreManager.Instance.MakePitchingResult(result);
                if (data != null)
                {
                    ScoreManager.Instance.Push(data);
                }
            }

            // 三振
            if (isStruckOut)
            {
                ScoreConst.Data data = ScoreManager.Instance.MakeStruckOut();
                if (data != null)
                {
                    ScoreManager.Instance.Push(data);
                }
            }
        }
    }

    /// <summary>
    /// イニングのシチュエーションによるスコア加算
    /// </summary>
    /// <param name="current">現在の得点</param>
    /// <param name="inningStart">イニング開始時点の得点</param>
    /// <param name="playBefore">プレー結果反映後の得点</param>
    /// <param name="other">相手チームの得点</param>
    public void SetInningSituation(int current, int inningStart, int playBefore, int other)
    {
        ScoreConst.Data data = ScoreManager.Instance.MakeInningSituation(current, inningStart, playBefore, other);
        if (data != null)
        {
            // BigInning はイニング１回のみ獲得可能
            if (data.type == ScoreConst.Type.BigInning)
            {
                if (alreadyBigInning)
                {
                    return;
                }
                alreadyBigInning = true;
            }

            ScoreManager.Instance.Push(data);
        }
    }

    /// <summary>
    /// ゲーム結果によるスコア加算
    /// </summary>
    /// <param name="own">自チームの統計情報</param>
    /// <param name="other">相手チームの統計情報</param>
    public void SetGameResult(GameDiag own, GameDiag other)
    {
        if (GameManager.Instance.IsWin())
        {
            int innings = GameManager.Instance.GetInnings();
            // 勝ち
            if (0 == other.GetHitCount() && innings >= MinimumInnings)
            {
                // 完全試合、ノーヒットノーランは5イニング以上設定のときに獲得可能
                if (0 == own.GetFourBallCount())
                {
                    // 完全試合
                    ScoreConst.Data data = ScoreManager.Instance.MakePerfectGame();
                    if (data != null)
                    {
                        ScoreManager.Instance.Push(data);
                    }
                }
                else
                {
                    // ノーヒットノーラン
                    ScoreConst.Data data = ScoreManager.Instance.MakeNoNo();
                    if (data != null)
                    {
                        ScoreManager.Instance.Push(data);
                    }
                }
            }
            else
            {
                int otherScore = GetOtherScore();
                if (0 == otherScore)
                {
                    // 完封
                    ScoreConst.Data data = ScoreManager.Instance.MakeShutOut();
                    if (data != null)
                    {
                        ScoreManager.Instance.Push(data);
                    }
                }
            }

            if (GameManager.Instance.IsSayonara())
            {
                //サヨナラ勝ち
                ScoreConst.Data data = ScoreManager.Instance.MakeSayonara();
                if (data != null)
                {
                    ScoreManager.Instance.Push(data);
                }
            }

            // 得点差
            {
                int ownScore = GetOwnScore();
                int otherScore = GetOtherScore();
                int diff = ownScore - otherScore;
                if (diff > 0)
                {
                    ScoreConst.Data data = ScoreManager.Instance.MakeWinScoreDiff(diff);
                    if (data != null)
                    {
                        ScoreManager.Instance.Push(data);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 自チームの得点を取得する
    /// </summary>
    /// <returns>自チームの得点</returns>
    private int GetOwnScore()
    {
        Core.Order order = GameManager.Instance.GetOrder();
        Score score = GameManager.Instance.GetScore();
        if (order == Core.Order.First)
        {
            return score.GetTotal(Core.Order.First);
        }
        else
        {
            return score.GetTotal(Core.Order.Second);
        }
    }

    /// <summary>
    /// 相手チームの得点を取得する
    /// </summary>
    /// <returns>相手チームの得点</returns>
    private int GetOtherScore()
    {
        Core.Order order = GameManager.Instance.GetOrder();
        Score score = GameManager.Instance.GetScore();
        if (order == Core.Order.First)
        {
            return score.GetTotal(Core.Order.Second);
        }
        else
        {
            return score.GetTotal(Core.Order.First);
        }
    }
}
