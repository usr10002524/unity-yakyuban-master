using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 座標イージング
/// </summary>
public class EasePosition : Easing
{
    /// <summary>
    /// 開始地点
    /// </summary>
    [SerializeField] private Vector3 startPosition;
    /// <summary>
    /// 終了地点
    /// </summary>
    [SerializeField] private Vector3 endPosition;

    /// <summary>
    /// オリジナルの座標
    /// </summary>
    private Vector3 origPosition;

    /// <summary>
    /// イージングの更新処理
    /// </summary>
    /// <param name="t">進行割合</param>
    protected override void UpdateEasing(float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 pos = Vector3.Lerp(startPosition, endPosition, t);

        if (easeObject != null)
        {
            easeObject.transform.position = pos;
        }
    }

    /// <summary>
    /// 初期化時の処理
    /// </summary>
    public override void OnInit()
    {
        if (easeObject != null)
        {
            origPosition = easeObject.transform.position;
        }
    }

    /// <summary>
    /// イージングコルーチン起動時の処理
    /// </summary>
    protected override void OnEasingInit()
    {
        if (easeObject != null)
        {
            easeObject.transform.position = origPosition;
        }
    }
}
