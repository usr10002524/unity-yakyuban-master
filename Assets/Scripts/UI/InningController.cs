using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// イニング表示制御クラス
/// </summary>
public class InningController : MonoBehaviour
{
    /// <summary>
    /// イニング数テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI inningText;

    /// <summary>
    /// 再描画処理
    /// </summary>
    public void Redraw()
    {
        RedrawInning();
    }

    /// <summary>
    /// イニング数を再描画する
    /// </summary>
    private void RedrawInning()
    {
        int inning = GameManager.Instance.GetCurrentInning();
        Core.Order order = GameManager.Instance.GetCurrentOrder();

        string text = SetupText(inning, order);
        inningText.text = text;
    }

    /// <summary>
    /// 表示するテキストを生成する
    /// </summary>
    /// <param name="inning">イニング数</param>
    /// <param name="order">表orウラ</param>
    /// <returns></returns>
    private string SetupText(int inning, Core.Order order)
    {
        string strInning = string.Format("{0} 回 {1}", inning + 1, (order == Core.Order.First) ? "表" : "ウラ");
        return strInning;
    }
}
