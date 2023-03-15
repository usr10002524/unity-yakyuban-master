using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1レコードあたりのクラス
/// </summary>
[System.Serializable]
public class RecordData : IEquatable<RecordData>, IComparable<RecordData>
{
    /// <summary>
    /// 自チーム名
    /// </summary>
    public string ownTeamName;
    /// <summary>
    /// 相手チーム名
    /// </summary>
    public string otherTeamName;
    /// <summary>
    /// スコア
    /// </summary>
    public int score;
    /// <summary>
    /// エポックタイム
    /// </summary>
    public long epochTime;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public RecordData()
    {
        ownTeamName = "";
        otherTeamName = "";
        score = 0;
        epochTime = 0;
    }

    /// <summary>
    /// コピーコンストラクタ
    /// </summary>
    /// <param name="recordData">コピーもと</param>
    public RecordData(RecordData recordData)
    {
        ownTeamName = recordData.ownTeamName;
        otherTeamName = recordData.otherTeamName;
        score = recordData.score;
        epochTime = recordData.epochTime;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ownTeamName">自チーム名</param>
    /// <param name="otherTeamName">相手チーム名</param>
    /// <param name="score">スコア</param>
    /// <param name="epochTime">エポックタイム</param>
    public RecordData(string ownTeamName, string otherTeamName, int score, long epochTime)
    {
        this.ownTeamName = ownTeamName;
        this.otherTeamName = otherTeamName;
        this.score = score;
        this.epochTime = epochTime;
    }

    /// <summary>
    /// レコードの比較
    /// </summary>
    /// <param name="other">他方のデータ</param>
    /// <returns>自身を先にする場合は<0、あとにする場合は>0、同値である場合は=0を返す</returns>
    public int CompareTo(RecordData other)
    {
        if (other == null) { return 1; }

        if (score == other.score)
        {
            if (epochTime == other.epochTime)
            {
                return 0;
            }
            else
            {
                return (epochTime > other.epochTime) ? -1 : 1;
            }
        }
        else
        {
            return (score > other.score) ? -1 : 1;
        }
    }

    /// <summary>
    /// レコードの比較
    /// </summary>
    /// <param name="other">他方のデータ</param>
    /// <returns>一致する場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool Equals(RecordData other)
    {
        if (other == null) { return false; }
        if (score != other.score) { return false; }
        if (epochTime != other.epochTime) { return false; }

        return true;
    }

    /// <summary>
    /// ログに出力する
    /// </summary>
    public void Logging()
    {
        string text = string.Format(" ownTeamName:{0} otherTeamName:{1} score:{2} epochItme:{3}", ownTeamName, otherTeamName, score, epochTime);
        Debug.Log(text);
    }
}

/// <summary>
/// 上位5件を格納するクラス
/// </summary>
[System.Serializable]
public class BestRecords
{
    /// <summary>
    /// 配列の最大件数
    /// </summary>
    public static readonly int maxRecordCount = 5;

    /// <summary>
    /// 5件分のレコード配列
    /// </summary>
    public RecordData[] bestRecords;
}

/// <summary>
/// 自己ベストレコードクラス
/// </summary>
[System.Serializable]
public class MyRecords
{
    public static readonly string key = "MyRecords";

    public BestRecords inning1;
    public BestRecords inning3;
    public BestRecords inning5;
    public BestRecords inning7;
    public BestRecords inning9;

    public MyRecords()
    {
        inning1 = new BestRecords();
        inning3 = new BestRecords();
        inning5 = new BestRecords();
        inning7 = new BestRecords();
        inning9 = new BestRecords();
    }
}

