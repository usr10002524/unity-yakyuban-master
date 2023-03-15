using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 情報UI制御クラス
/// </summary>
public class InfomationUIController : MonoBehaviour
{
    /// <summary>
    /// 初期化イベント
    /// </summary>
    [SerializeField] private UnityEvent initEvent;
    /// <summary>
    /// 再描画イベント
    /// </summary>
    [SerializeField] private UnityEvent redrawEvent;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void OnInit()
    {
        initEvent.Invoke();
        OnRedraw();
    }

    /// <summary>
    /// 再描画処理
    /// </summary>
    public void OnRedraw()
    {
        redrawEvent.Invoke();
    }

    /// <summary>
    /// 表示/非表示設定
    /// </summary>
    /// <param name="flag">表示フラグ</param>
    public void Show(bool flag)
    {
        gameObject.SetActive(flag);
    }
}
