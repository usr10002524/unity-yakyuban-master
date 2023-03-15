using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 統計情報クラス
/// </summary>
public class GameDiag
{
    // 攻撃側
    /// <summary>
    /// 打者数
    /// </summary>
    public int batters { get; private set; }
    /// <summary>
    /// 単打数
    /// </summary>
    public int hit1Bases { get; private set; }
    /// <summary>
    /// 2塁打数
    /// </summary>
    public int hit2Bases { get; private set; }
    /// <summary>
    /// 3塁打数
    /// </summary>
    public int hit3Bases { get; private set; }
    /// <summary>
    /// 本塁打数
    /// </summary>
    public int homeruns { get; private set; }

    // 守備側
    /// <summary>
    /// 投球数
    /// </summary>
    public int pitchCount { get; private set; }
    /// <summary>
    /// アウト数
    /// </summary>
    public int outs { get; private set; }
    /// <summary>
    /// 奪三振数
    /// </summary>
    public int struckOuts { get; private set; }
    /// <summary>
    /// 与四球数
    /// </summary>
    public int fourBalls { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GameDiag()
    {
        batters = 0;
        hit1Bases = 0;
        hit2Bases = 0;
        hit3Bases = 0;
        homeruns = 0;

        pitchCount = 0;
        outs = 0;
        struckOuts = 0;
        fourBalls = 0;
    }

    /// <summary>
    /// プレー結果を反映させる
    /// </summary>
    /// <param name="isBatting">攻撃側かどうか</param>
    /// <param name="result">プレー結果</param>
    /// <param name="isStruckOut">三振か</param>
    /// <param name="isFourBall">フォアボールか</param>
    public void ApplyResult(bool isBatting, Core.Result result, bool isStruckOut, bool isFourBall)
    {
        if (isBatting)
        {
            // 攻撃側
            switch (result)
            {
                case Core.Result.Hit1Base: hit1Bases++; break;
                case Core.Result.Hit2Base: hit2Bases++; break;
                case Core.Result.Hit3Base: hit3Bases++; break;
                case Core.Result.HomeRun: homeruns++; break;
            }

            switch (result)
            {
                case Core.Result.Hit1Base:
                case Core.Result.Hit2Base:
                case Core.Result.Hit3Base:
                case Core.Result.HomeRun:
                case Core.Result.Out:
                    batters++;
                    break;
            }
        }
        else
        {
            pitchCount++;

            // 守備側
            switch (result)
            {
                case Core.Result.Out: outs++; break;
            }

            if (isStruckOut)
            {
                struckOuts++;
            }
            if (isFourBall)
            {
                fourBalls++;
            }
        }
    }

    /// <summary>
    /// ヒット数を取得する
    /// </summary>
    /// <returns>ヒット数</returns>
    public int GetHitCount()
    {
        int hits = 0;
        hits += hit1Bases;
        hits += hit2Bases;
        hits += hit3Bases;
        hits += homeruns;

        return hits;
    }

    /// <summary>
    /// 与四球数を取得する
    /// </summary>
    /// <returns>与四球数</returns>
    public int GetFourBallCount()
    {
        return fourBalls;
    }

    /// <summary>
    /// 情報をログに出力する
    /// </summary>
    public void Logging()
    {
        //  string text = string.Format("[GameDiag] b:{0} 1BH:{1} 2BH{2} 3BH:{3} HR:{4} / p:{5} O:{6} K:{7} F{8}"
        //     , batters, hit1Bases, hit2Bases, hit3Bases, homeruns
        //     , pitchCount, outs, struckOuts, fourBalls);

        // Debug.Log(text);
    }
}
