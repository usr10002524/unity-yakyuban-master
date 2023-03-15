using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームオーディオクラス
/// </summary>
public class GameAudioController : MonoBehaviour
{
    /// <summary>
    /// オーディオの状態をリセットする
    /// </summary>
    public void OnReady()
    {
        // 歓声が再生成中であれば、フェードアウトさせる
        ApplauseManager.Instance.FadeStopSe(0.25f);
    }

    /// <summary>
    /// イニング開始の処理
    /// </summary>
    public void StartInning()
    {
        // プレーヤーが攻撃中か守備中かにおうじてBGMを変更する
        if (GameManager.Instance.IsBatting())
        {
            BgmManager.Instance.PlayBgm(BgmType.bgm01);
        }
        else
        {
            BgmManager.Instance.PlayBgm(BgmType.bgm02);
        }
    }

    public void Shot()
    {
        SeManager.Instance.PlaySe(SeType.sePitching);
    }

    public void Hitted()
    {
        SeManager.Instance.PlaySe(SeType.seHit);
        ApplauseManager.Instance.PlaySe(SeType.seApplause, 0.5f);
    }

    public void Bound()
    {
        SeManager.Instance.PlaySe(SeType.seBound);
    }

    public void StartResult(Core.Result result)
    {
        switch (result)
        {
            case Core.Result.Hit1Base:
            case Core.Result.Hit2Base:
            case Core.Result.Hit3Base:
                ApplauseManager.Instance.PlaySe(SeType.seApplause, 0.75f);
                break;

            case Core.Result.HomeRun:
                ApplauseManager.Instance.PlaySe(SeType.seApplause, 1.0f);
                break;

            default:
                ApplauseManager.Instance.FadeStopSe(0.25f);
                break;
        }
    }

    public void Change()
    {
        BgmManager.Instance.FadeStopBgm(0.5f);
    }

    public void GameSet()
    {
        BgmManager.Instance.FadeStopBgm(0.5f);
        SeManager.Instance.PlaySe(SeType.seGameSet);
    }
}
