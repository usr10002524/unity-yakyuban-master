using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ピッチングカウント表示制御クラス
/// </summary>
public class CountController : MonoBehaviour
{
    /// <summary>
    /// ボールオブジェクトリスト
    /// </summary>
    [SerializeField] private List<GameObject> ballsObject;
    /// <summary>
    /// ストライクオブジェクトリスト
    /// </summary>
    [SerializeField] private List<GameObject> strikesObject;
    /// <summary>
    /// アウトオブジェクトリスト
    /// </summary>
    [SerializeField] private List<GameObject> outsObject;

    /// <summary>
    /// 再描画を行う
    /// </summary>
    public void Redraw()
    {
        RedrawBalls();
        RedrawStrikes();
        RedrawOuts();
    }

    /// <summary>
    /// ボールカウントを再描画する
    /// </summary>
    private void RedrawBalls()
    {
        PitchingCount count = GameManager.Instance.GetPitchingCount();
        int balls = count.GetBalls();

        for (int i = 0; i < ballsObject.Count; i++)
        {
            if (i < balls)
            {
                ballsObject[i].SetActive(true);
            }
            else
            {
                ballsObject[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// ストライクカウントを再描画する
    /// </summary>
    private void RedrawStrikes()
    {
        PitchingCount count = GameManager.Instance.GetPitchingCount();
        int strikes = count.GetStrikes();

        for (int i = 0; i < strikesObject.Count; i++)
        {
            if (i < strikes)
            {
                strikesObject[i].SetActive(true);
            }
            else
            {
                strikesObject[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// アウトカウントを再描画する
    /// </summary>
    private void RedrawOuts()
    {
        PitchingCount count = GameManager.Instance.GetPitchingCount();
        int outs = count.GetOuts();

        for (int i = 0; i < outsObject.Count; i++)
        {
            if (i < outs)
            {
                outsObject[i].SetActive(true);
            }
            else
            {
                outsObject[i].SetActive(false);
            }
        }
    }
}
