using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Core.Result用カットインクラス
/// </summary>
public class Cutin : CutinBase
{
    [SerializeField] private Core.Result result = Core.Result.None;

    /// <summary>
    /// カットインのタイプを取得する
    /// </summary>
    /// <returns>カットインのタイプ</returns>
    public virtual Core.Result GetResult() { return result; }
}
