using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 共通の定義
/// </summary>
namespace Core
{
    /// <summary>
    /// 先攻or後攻
    /// </summary>
    public enum Order
    {
        First,
        Second,
    }

    /// <summary>
    ///  プレー結果
    /// </summary>
    public enum Result
    {
        None = 0,
        Strike,
        Ball,

        Out,
        Foul,
        Hit1Base,
        Hit2Base,
        Hit3Base,
        HomeRun,

        Change,
        GameSet,
    }
}
