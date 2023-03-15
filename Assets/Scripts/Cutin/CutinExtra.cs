using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ExtraType用カットインクラス
/// </summary>
public class CutinExtra : CutinBase
{
    public enum ExtraType
    {
        None,
        PlayBall,
        Win,
        Lose,
        Draw,
    }

    [SerializeField] private ExtraType extraType = ExtraType.None;

    /// <summary>
    /// カットインのタイプを取得する
    /// </summary>
    /// <returns>カットインのタイプ</returns>
    public virtual ExtraType GetExtraType() { return extraType; }
}
