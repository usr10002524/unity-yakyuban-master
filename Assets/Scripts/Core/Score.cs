using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エイリアス
using Order = Core.Order;

/// <summary>
/// スコア管理クラス
/// </summary>
public class Score
{
    // 先攻の各イニングのスコア
    public int[] first;
    // 後攻の各イニングのスコア
    public int[] second;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="innings">イニング数</param>
    public Score(int innings)
    {
        first = new int[innings];
        second = new int[innings];

        for (int i = 0; i < innings; i++)
        {
            first[i] = 0;
            second[i] = 0;
        }
    }

    /// <summary>
    /// 指定したイニング、チームの獲得したスコアを取得する。
    /// </summary>
    /// <param name="inning">イニング</param>
    /// <param name="order">先攻or後攻</param>
    /// <returns>スコア</returns>
    public int Get(int inning, Order order)
    {
        switch (order)
        {
            case Order.First:
                if (inning >= 0 && inning < first.Length)
                {
                    return first[inning];
                }
                break;

            case Order.Second:
                if (inning >= 0 && inning < second.Length)
                {
                    return second[inning];
                }
                break;
        }

        throw new System.ArgumentOutOfRangeException("inning", inning, "");
    }

    /// <summary>
    /// 指定したイニング、チームにスコアを加算する。
    /// </summary>
    /// <param name="inning">イニング</param>
    /// <param name="order">先攻or後攻</param>
    /// <param name="score">加算するスコア</param>
    public void Add(int inning, Order order, int score)
    {
        switch (order)
        {
            case Order.First:
                if (inning >= 0 && inning < first.Length)
                {
                    first[inning] += score;
                }
                break;

            case Order.Second:

                if (inning >= 0 && inning < second.Length)
                {
                    second[inning] += score;
                }
                break;
        }
    }

    /// <summary>
    /// 指定したチームの合計点を取得する
    /// </summary>
    /// <param name="order">先攻orこうこう</param>
    /// <returns>合計点</returns>
    public int GetTotal(Order order)
    {
        int total = 0;
        switch (order)
        {
            case Order.First:
                foreach (var item in first)
                {
                    total += item;
                }
                break;

            case Order.Second:
                foreach (var item in second)
                {
                    total += item;
                }
                break;
        }
        return total;
    }

    /// <summary>
    /// 指定したイニングまでに取得した合計点を取得する。
    /// </summary>
    /// <param name="inning">イニング</param>
    /// <param name="order">先攻or後攻</param>
    /// <returns>指定したイニングまでに取得した合計点</returns>
    public int GetInningStart(int inning, Order order)
    {
        int total = 0;
        switch (order)
        {
            case Order.First:
                for (int i = 0; i < inning; i++)
                {
                    total += first[i];
                }
                break;

            case Order.Second:
                for (int i = 0; i < inning; i++)
                {
                    total += second[i];
                }
                break;
        }
        return total;
    }

    /// <summary>
    /// サヨナラが成立したかどうかをチェックする。
    /// </summary>
    /// <returns>サヨナラが成立した場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsSayonara()
    {
        if (second.Length <= 0)
        {
            return false;   // 未初期化
        }

        int firstScore = GetTotal(Order.First);
        int scondScore = GetTotal(Order.Second);
        int lastInning = second.Length - 1;

        // 後攻が勝ち、かつ、最終イニングで得点がある場合、サヨナラ勝ち
        if ((scondScore > firstScore) &&
            (second[lastInning] > 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 最終イニングかどうかをチェックする。
    /// </summary>
    /// <param name="inning">最終イニングの場合はtrue、そうでない場合はfalseを返す。</param>
    /// <returns></returns>
    private bool IsLastInning(int inning)
    {
        return (second.Length - 1 == inning);
    }
}
