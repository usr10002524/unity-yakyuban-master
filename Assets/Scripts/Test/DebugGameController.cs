using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム進行制御クラス
/// </summary>
public partial class GameController : MonoBehaviour
{
    /// <summary>
    /// デバッグ結果設定（ストライク）
    /// </summary>
    [ContextMenu("PitchingStrike")]
    void PitchingStrike()
    {
        DebugSetPlayResult(Core.Result.Strike);
    }

    /// <summary>
    /// デバッグ結果設定（ボール）
    /// </summary>
    [ContextMenu("PitchingBall")]
    void PitchingBall()
    {
        DebugSetPlayResult(Core.Result.Ball);
    }

    /// <summary>
    /// デバッグ結果設定（アウト）
    /// </summary>
    [ContextMenu("BattingOut")]
    void BattingOut()
    {
        DebugSetPlayResult(Core.Result.Out);
    }

    /// <summary>
    /// デバッグ結果設定（ファウル）
    /// </summary>
    [ContextMenu("BattingFoul")]
    void BattingFoul()
    {
        DebugSetPlayResult(Core.Result.Foul);
    }

    /// <summary>
    /// デバッグ結果設定（ヒット）
    /// </summary>
    [ContextMenu("Batting1BH")]
    void Batting1BH()
    {
        DebugSetPlayResult(Core.Result.Hit1Base);
    }

    /// <summary>
    /// デバッグ結果設定（2塁打）
    /// </summary>
    [ContextMenu("Batting2BH")]
    void Batting2BH()
    {
        DebugSetPlayResult(Core.Result.Hit2Base);
    }

    /// <summary>
    /// デバッグ結果設定（3塁打）
    /// </summary>
    [ContextMenu("Batting3BH")]
    void Batting3BH()
    {
        DebugSetPlayResult(Core.Result.Hit3Base);
    }

    /// <summary>
    /// デバッグ結果設定（本塁打）
    /// </summary>
    [ContextMenu("BattingHomeRun")]
    void BattingHomeRun()
    {
        DebugSetPlayResult(Core.Result.HomeRun);
    }

    /// <summary>
    /// デバッグで結果を設定する
    /// </summary>
    /// <param name="result">設定する結果</param>
    private void DebugSetPlayResult(Core.Result result)
    {
        if (step == Step.UPDATE_PLAY)
        {
            this.result = result;
            SetPlayResult(result);
            ChangeStep(Step.START_RESULT);
        }
    }
}
