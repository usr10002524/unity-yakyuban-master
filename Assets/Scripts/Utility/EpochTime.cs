using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エポックタイムユーティリティ
/// </summary>
public class EpochTime
{
    /// <summary>
    /// UNIXのエポック開始時間
    /// </summary>
    private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);

    /// <summary>
    /// DateTimeをUnix時間に変換する
    /// </summary>
    /// <param name="targetTime">DateTime型の時間</param>
    /// <returns>UNIX_EPOCHからの経過秒数</returns>
    public static long ToUnixTime(DateTime targetTime)
    {
        // UTC時間に変換
        targetTime = targetTime.ToUniversalTime();

        // UNIX_EPOCH からの経過時間を取得
        TimeSpan elapsedTime = targetTime - UNIX_EPOCH;
        return (long)elapsedTime.TotalSeconds;
    }

    /// <summary>
    /// Unix時間からDataTimeを作成する
    /// </summary>
    /// <param name="unixTime">Unix時間</param>
    /// <returns>DataTime型の時間</returns>
    public static DateTime ToDateTime(long unixTime)
    {
        // UNIX時間をTickに変換
        long ticks = unixTime * TimeSpan.TicksPerSecond;
        // UNIX_EPOCHのTickを加算
        ticks += UNIX_EPOCH.Ticks;
        // DateTimeを生成
        DateTime dateTime = new DateTime(ticks);
        // ローカル時間に変換
        dateTime = dateTime.ToLocalTime();
        return dateTime;
    }
}
