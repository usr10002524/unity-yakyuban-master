using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコア関連の定数定義
/// </summary>
public class ScoreConst
{
    /// <summary>
    /// レアリティ
    /// </summary>
    public enum Rarity
    {
        None,
        Common,
        HighCommon,
        Rare,
        HighRare,
    }

    /// <summary>
    /// タイプ
    /// </summary>
    public enum Type
    {
        None,

        Hit1Base,
        Hit2Base,
        Hit3Base,
        HomeRun,

        Out,
        StruckOut,

        Sayonara,
        Shutout,
        NoNo,
        PerfectGame,
        DiffRate,

        FastScore,
        OverScore,
        BigInning,
    }

    /// <summary>
    /// スコアデータ
    /// </summary>
    public class Data
    {
        public Type type;
        public string descr;
        public Rarity rarity;
        public int point;

        public Data(Type type, string descr, Rarity rarity, int point)
        {
            this.type = type;
            this.descr = descr;
            this.rarity = rarity;
            this.point = point;
        }
    }

    /// <summary>
    /// スコアデータ定義
    /// </summary>
    private static readonly Data[] constData = new Data[] {
        new Data(Type.Hit1Base, "ヒット", Rarity.Common, 10),
        new Data(Type.Hit2Base, "2塁打", Rarity.HighCommon, 30),
        new Data(Type.Hit3Base, "3塁打", Rarity.HighCommon, 50),
        new Data(Type.HomeRun, "本塁打", Rarity.Rare, 100),

        new Data(Type.Out, "アウト", Rarity.Common, 10),
        new Data(Type.StruckOut, "奪三振", Rarity.HighCommon, 50),

        new Data(Type.Sayonara, "サヨナラ勝ち", Rarity.Rare, 100),
        new Data(Type.Shutout, "完封", Rarity.Rare, 500),
        new Data(Type.NoNo, "ノーヒットノーラン", Rarity.HighRare, 1000),
        new Data(Type.PerfectGame, "完全試合", Rarity.HighRare, 3000),
        new Data(Type.DiffRate, "得点差x100", Rarity.Common, 100),

        new Data(Type.FastScore, "先取点", Rarity.HighCommon, 50),
        new Data(Type.OverScore, "勝ち越し", Rarity.HighCommon, 30),
        new Data(Type.BigInning, "ビッグイニング", Rarity.Rare, 100),
    };

    /// <summary>
    /// 指定したタイプのスコアデータを取得する
    /// </summary>
    /// <param name="type">タイプ</param>
    /// <returns>スコアデータ</returns>
    public static Data Get(Type type)
    {
        foreach (var item in constData)
        {
            if (item.type == type)
            {
                return item;
            }
        }
        return null;
    }

}