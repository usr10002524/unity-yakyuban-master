using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カットインテスト用クラス
/// </summary>
public class CutinTest : MonoBehaviour
{
    /// <summary>
    /// 表示するカットインを指定する
    /// </summary>
    [SerializeField] private Core.Result result = Core.Result.None;
    /// <summary>
    /// 表示するカットインを指定する
    /// </summary>
    [SerializeField] private CutinExtra.ExtraType extraType = CutinExtra.ExtraType.None;
    /// <summary>
    /// カットインが終了したかどうかのフラグ
    /// </summary>
    [SerializeField] private bool isEndCutin;

    /// <summary>
    /// カットインコントローラ
    /// </summary>
    private CutinController cutinController;

    private void Awake()
    {
        cutinController = GetComponent<CutinController>();
    }

    private void Update()
    {
        if (cutinController != null)
        {
            isEndCutin = cutinController.IsEndCutin();
        }
    }

    /// <summary>
    /// カットインを開始する
    /// </summary>
    [ContextMenu("StartCutin")]
    void StartCutin()
    {
        if (cutinController == null)
        {
            return;
        }

        if (result != Core.Result.None)
        {
            cutinController.StartCutin(result);
        }
        else if (extraType != CutinExtra.ExtraType.None)
        {
            cutinController.StartCutin(extraType);
        }
    }

    /// <summary>
    /// カットインを停止させる。
    /// </summary>
    [ContextMenu("OffCutin")]
    void OffCutin()
    {
        if (cutinController == null)
        {
            return;
        }

        if (result != Core.Result.None)
        {
            cutinController.OffCutin(result);
        }
        else if (extraType != CutinExtra.ExtraType.None)
        {
            cutinController.OffCutin(extraType);
        }
    }

}
