using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エイリアス
using Result = Core.Result;

/// <summary>
/// ピッチングカウント管理クラス
/// </summary>
public class PitchingCount
{
    // 各種定数
    // 三振に必要なストライク数
    public static readonly int StrikesForOut = 3;
    // ファールで加算されるストライクの最大数
    public static readonly int StrikesForFoulMax = 2;
    // フォアボールに必要なボール数
    public static readonly int BallsForFourBall = 4;
    // チェンジに必要なアウト数
    public static readonly int OutsForChange = 3;

    // ストライク
    private int strikes;
    // ボール
    private int balls;
    // アウト
    private int outs;
    // カウントリセット予約フラグ
    private bool reserveCountReset;

    /// <summary>
    /// コンスとラクタ
    /// </summary>
    public PitchingCount()
    {
        strikes = 0;
        balls = 0;
        outs = 0;
        reserveCountReset = false;
    }

    /// <summary>
    /// 投球の結果を反映させる。
    /// Strike,Ball 以外は無視されます。 
    /// </summary>
    /// <param name="result">投球結果</param>
    public void ApplyResult(Result result)
    {
        switch (result)
        {
            case Result.Strike:
                strikes += 1;
                break;

            case Result.Ball:
                balls += 1;
                break;
        }
    }

    /// <summary>
    /// ファウルを反映させる。
    /// </summary>
    public void ApplyFoul()
    {
        if (strikes < StrikesForFoulMax)
        {
            strikes += 1;
        }
    }

    /// <summary>
    /// アウトを追加する
    /// </summary>
    public void AddOut()
    {
        outs += 1;
        ReserveCountReset();
    }

    /// <summary>
    /// カウントリセットフラグを立てる
    /// </summary>
    public void ReserveCountReset()
    {
        reserveCountReset = true;
    }

    /// <summary>
    /// 三振かどうかをチェックする。
    /// </summary>
    /// <returns>三振の場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsStruckOut()
    {
        return (strikes >= StrikesForOut);
    }

    /// <summary>
    /// フォアボールかどうかをチェックする。
    /// </summary>
    /// <returns>フォアボールの場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsFourBall()
    {
        return (balls >= BallsForFourBall);
    }

    /// <summary>
    /// チェンジどうかをチェックする。
    /// </summary>
    /// <returns>チェンジの場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsChange()
    {
        int outCount = outs;
        return (outs >= OutsForChange);
    }

    /// <summary>
    /// カウントを更新する。
    /// </summary>
    public void UpdateCount()
    {
        if (IsStruckOut())
        {
            strikes = 0;
            balls = 0;
        }
        if (IsFourBall())
        {
            strikes = 0;
            balls = 0;
        }
        if (IsChange())
        {
            strikes = 0;
            balls = 0;
            outs = 0;
        }
        if (reserveCountReset)
        {
            strikes = 0;
            balls = 0;
        }
        reserveCountReset = false;
    }

    /// <summary>
    /// ストライク数を取得する。
    /// </summary>
    /// <returns>ストライク数</returns>
    public int GetStrikes()
    {
        return strikes;
    }

    /// <summary>
    /// ボール数を取得する。
    /// </summary>
    /// <returns>ボール数</returns>
    public int GetBalls()
    {
        return balls;
    }

    /// <summary>
    /// アウト数を取得する。
    /// </summary>
    /// <returns>アウト数</returns>
    public int GetOuts()
    {
        return outs;
    }
}
