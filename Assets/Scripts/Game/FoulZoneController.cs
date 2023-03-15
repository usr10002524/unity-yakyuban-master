using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ファウルゾーンクラス
/// </summary>
public class FoulZoneController : MonoBehaviour, IHitResult
{
    /// <summary>
    /// プレー結果
    /// </summary>
    [SerializeField] Core.Result hitResult;

    /// <summary>
    /// プレー結果を取得する
    /// </summary>
    /// <returns>プレー結果</returns>
    public Core.Result GetHitResult()
    {
        return hitResult;
    }
}
