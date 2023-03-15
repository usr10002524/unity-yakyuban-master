using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エイリアス
using Result = Core.Result;

/// <summary>
/// プレー結果インターフェース
/// </summary>
public interface IHitResult
{
    /// <summary>
    /// プレー結果を取得する
    /// </summary>
    /// <returns>プレー結果</returns>
    public Result GetHitResult();
}
