using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// カットインベースクラス
/// </summary>
public class CutinBase : MonoBehaviour
{
    /// <summary>
    /// 制御するゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject cutinObject;

    /// <summary>
    /// オブジェクトに設定されているイージングのリスト
    /// </summary>
    private List<Easing> easeList;

    /// <summary>
    /// 初期化時の処理を行う。
    /// CutinController.OnInit() から1度だけ呼ばれる。
    /// </summary>
    public virtual void OnInit()
    {
        easeList = new List<Easing>();
        Easing[] easings = cutinObject.GetComponents<Easing>();
        foreach (var item in easings)
        {
            item.OnInit();
            easeList.Add(item);
        }
    }

    /// <summary>
    /// カットインを開始する。
    /// </summary>
    public virtual void StartCutin()
    {
        foreach (var item in easeList)
        {
            item.StartEasing();
        }
    }

    /// <summary>
    /// カットインが終了したかどうかチェックする。
    /// </summary>
    /// <returns>カットインが終了した場合はtrue、そうでない場合はfalseを返す。</returns>
    public virtual bool IsEndCutin()
    {
        foreach (var item in easeList)
        {
            if (item.IsInEasing())
            {
                return false;
            }
        }

        return true;
    }
}
