using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エイリアス
using Result = Core.Result;

/// <summary>
/// ランナー管理クラス
/// </summary>
public class Runners
{
    // ベース
    private enum Bases
    {
        First,
        Second,
        Third,
        Home,
        Max,
    }

    // 各種定数
    // 各ベースにランナーが居るかどうかをビットで管理する。
    private static readonly uint firstBaseBit = 1 << (int)Bases.First;
    private static readonly uint secondBaseBit = 1 << (int)Bases.Second;
    private static readonly uint thirdBaseBit = 1 << (int)Bases.Third;
    // 1～3塁までのビットマスク
    private static readonly uint maskBaseBit = (firstBaseBit | secondBaseBit | thirdBaseBit);
    private static readonly uint homeBaseBit = 1 << (int)Bases.Home;
    // 1～3塁までのビット＋それらが本塁までシフトした場合を考慮して7ビットを使用する
    private static readonly int maxBit = (int)Bases.Max + (int)Bases.Home;

    // 各ベースのランナー存在ビット
    uint baseBit;
    // ホームインした数をカウント
    int homeInCount;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Runners()
    {
        baseBit = 0;
        homeInCount = 0;
    }

    /// <summary>
    /// ランナーの状態をリセットする
    /// </summary>
    public void Reset()
    {
        baseBit = 0;
        homeInCount = 0;
    }

    /// <summary>
    /// プレー結果（ヒット、ホームラン）を反映する。
    /// </summary>
    /// <param name="result">プレー結果</param>
    public void ApplyResult(Result result)
    {
        uint prev = baseBit;

        switch (result)
        {
            case Result.Hit1Base:
                baseBit <<= 1;
                baseBit |= firstBaseBit;
                break;

            case Result.Hit2Base:
                baseBit <<= 2;
                baseBit |= secondBaseBit;
                break;

            case Result.Hit3Base:
                baseBit <<= 3;
                baseBit |= thirdBaseBit;
                break;

            case Result.HomeRun:
                baseBit <<= 4;
                baseBit |= homeBaseBit;
                break;
        }

        // 獲得した点
        homeInCount = GetHomeInCount(baseBit);
        // Bases.Home 以降のビットを下ろす
        baseBit = baseBit & maskBaseBit;
    }

    /// <summary>
    /// フォアボールを反映させる。
    /// </summary>
    public void ApplyFourBall()
    {
        uint prev = baseBit;

        // 下位ビットから舐めていって、最初に見つかったビットが立っていない場所を立てる
        for (int bit = 0; bit < maxBit; bit++)
        {
            if ((baseBit & (uint)(1 << bit)) == 0)
            {
                baseBit |= (uint)(1 << bit);
                break;
            }
        }

        // 獲得した点
        homeInCount = GetHomeInCount(baseBit);
        // Bases.Home 以降のビットを下ろす
        baseBit = baseBit & maskBaseBit;
    }

    /// <summary>
    /// ファーストにランナーが居るかチェックする。
    /// </summary>
    /// <returns>ファーストにランナーが居る場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsExistFirstRunner()
    {
        return ((baseBit & firstBaseBit) != 0);
    }

    /// <summary>
    /// セカンドにランナーが居るかチェックする。
    /// </summary>
    /// <returns>セカンドにランナーが居る場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsExistSecondRunner()
    {
        return ((baseBit & secondBaseBit) != 0);
    }

    /// <summary>
    /// サードにランナーが居るかチェックする。
    /// </summary>
    /// <returns>サードにランナーが居る場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsExistThirdRunner()
    {
        return ((baseBit & thirdBaseBit) != 0);
    }

    /// <summary>
    /// ホームインした回数を取得する。
    /// </summary>
    /// <returns>ホームインした回数</returns>
    public int GetHomeInCount()
    {
        return homeInCount;
    }

    /// <summary>
    /// 指定したランナービットは何回ホームインしたかを取得する。
    /// </summary>
    /// <param name="_baseBit">ランナービット</param>
    /// <returns>ホームインした回数</returns>
    private int GetHomeInCount(uint _baseBit)
    {
        // Bases.Home 以降で立っているビットが獲得したスコアとなる
        int homeIn = 0;
        for (int bit = (int)Bases.Home; bit < maxBit; bit++)
        {
            if ((_baseBit & (uint)(1 << bit)) != 0)
            {
                homeIn++;
            }
        }
        return homeIn;
    }
}