using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スケールイージング
/// </summary>
public class EaseScale : Easing
{
    /// <summary>
    /// 開始スケーリング
    /// </summary>
    [SerializeField] private Vector3 startScale;
    /// <summary>
    /// 終了スケーリング
    /// </summary>
    [SerializeField] private Vector3 endScale;

    /// <summary>
    /// オリジナルのスケーリング
    /// </summary>
    private Vector3 origScale;

    /// <summary>
    /// イージングの更新処理
    /// </summary>
    /// <param name="t">進行割合</param>
    protected override void UpdateEasing(float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 scale = Vector3.Lerp(startScale, endScale, t);

        if (easeObject != null)
        {
            easeObject.transform.localScale = scale;
        }
    }

    /// <summary>
    /// 初期化時の処理
    /// </summary>
    public override void OnInit()
    {
        if (easeObject != null)
        {
            origScale = easeObject.transform.localScale;
        }
    }

    /// <summary>
    /// イージングコルーチン起動時の処理
    /// </summary>
    protected override void OnEasingInit()
    {
        if (easeObject != null)
        {
            easeObject.transform.localScale = origScale;
        }
    }
}
