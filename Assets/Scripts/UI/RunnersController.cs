using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ランナー表示管理クラス
/// </summary>
public class RunnersController : MonoBehaviour
{
    /// <summary>
    /// ランナーオブジェクトリスト
    /// </summary>
    [SerializeField] private List<GameObject> runnersObject;

    /// <summary>
    /// 再描画処理
    /// </summary>
    public void Redraw()
    {
        RedrawRunners();
    }

    /// <summary>
    /// ランナー表示を再描画する
    /// </summary>
    private void RedrawRunners()
    {
        Runners runners = GameManager.Instance.GetRunners();

        runnersObject[0].SetActive(runners.IsExistFirstRunner());
        runnersObject[1].SetActive(runners.IsExistSecondRunner());
        runnersObject[2].SetActive(runners.IsExistThirdRunner());
    }

}
