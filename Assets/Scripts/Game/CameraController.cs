using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラ管理クラス
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// カメラのタイプ
    /// </summary>
    public enum CameraType
    {
        BattingCamera,
        PitchingCamera,
        FieldingCamera,
    }

    /// <summary>
    /// バッター用カメラのゲームオブジェクト
    /// </summary>
    [SerializeField] GameObject battingCameraObject;
    /// <summary>
    /// ピッチャー用カメラのゲームオブジェクト
    /// </summary>
    [SerializeField] GameObject pitchingCameraObject;
    /// <summary>
    /// 全体を見る用カメラのゲームオブジェクト
    /// </summary>
    [SerializeField] GameObject fieldingCameraObject;

    /// <summary>
    /// 現在のカメラタイプ
    /// </summary>
    private CameraType cameraType = CameraType.BattingCamera;

    /// <summary>
    /// カメラの状態をリセットする。
    /// </summary>
    /// <param name="isBatting">プレーヤー攻撃中かどうか</param>

    public void OnReady(bool isBatting)
    {
        if (isBatting)
        {
            // プレーヤー攻撃中はバッター用カメラ
            SetCamera(CameraType.BattingCamera);
        }
        else
        {
            // プレーヤー守備中はピッチャー用カメラ
            SetCamera(CameraType.PitchingCamera);
        }
    }

    /// <summary>
    /// 指定したタイプのカメラに切り替える
    /// </summary>
    /// <param name="type">カメラタイプ</param>
    public void SetCamera(CameraType type)
    {
        cameraType = type;

        // 一旦全て非アクティブにする
        battingCameraObject.SetActive(false);
        pitchingCameraObject.SetActive(false);
        fieldingCameraObject.SetActive(false);

        // 必要なものだけアクティブにする
        switch (cameraType)
        {
            case CameraType.BattingCamera: battingCameraObject.SetActive(true); break;
            case CameraType.PitchingCamera: pitchingCameraObject.SetActive(true); break;
            case CameraType.FieldingCamera: fieldingCameraObject.SetActive(true); break;
        }
    }
}
