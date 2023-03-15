using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カットインコントローラクラス
/// </summary>
public class CutinController : MonoBehaviour
{
    /// <summary>
    /// Core.Resultを使用するカットインオブジェクトリスト
    /// </summary>
    [SerializeField] private List<GameObject> cutinObjects;
    /// <summary>
    /// ExtraTypeを使用するカットインオブジェクトリスト
    /// </summary>
    [SerializeField] private List<GameObject> extraObjects;

    /// <summary>
    /// Core.Resultを使用するカットインクラスのマップ
    /// </summary>
    private Dictionary<Core.Result, Cutin> cutinMap;
    /// <summary>
    /// ExtraTypeを使用するカットインクラスのマップ
    /// </summary>
    private Dictionary<CutinExtra.ExtraType, CutinExtra> extraMap;


    /// <summary>
    /// 初期化時の処理を行う。
    /// </summary>
    public void OnInit()
    {
        // 一旦全てOffにする。
        AllOff();

        // CoreResult用のマップを作成する
        cutinMap = new Dictionary<Core.Result, Cutin>();

        foreach (var item in cutinObjects)
        {
            Cutin cutin = item.GetComponent<Cutin>();
            if (cutin != null)
            {
                cutin.OnInit();

                Core.Result result = cutin.GetResult();
                if (result != Core.Result.None)
                {
                    cutinMap.Add(result, cutin);
                }

            }
        }

        // ExtraType用のマップを作成する
        extraMap = new Dictionary<CutinExtra.ExtraType, CutinExtra>();

        foreach (var item in extraObjects)
        {
            CutinExtra cutin = item.GetComponent<CutinExtra>();
            if (cutin != null)
            {
                cutin.OnInit();

                CutinExtra.ExtraType type = cutin.GetExtraType();
                if (type != CutinExtra.ExtraType.None)
                {
                    extraMap.Add(type, cutin);
                }

            }
        }
    }

    /// <summary>
    /// 指定したタイプのカットインを開始する。
    /// </summary>
    /// <param name="result">カットインのタイプ</param>
    public void StartCutin(Core.Result result)
    {
        Cutin cutin = null;

        if (result != Core.Result.None)
        {
            if (cutinMap.ContainsKey(result))
            {
                cutin = cutinMap[result];
            }
        }

        if (cutin != null)
        {
            if (!cutin.gameObject.activeInHierarchy)
            {
                cutin.gameObject.SetActive(true);
            }
            cutin.StartCutin();
        }
    }

    /// <summary>
    /// 指定したタイプのカットインを開始する。
    /// </summary>
    /// <param name="result">カットインのタイプ</param>
    public void StartCutin(CutinExtra.ExtraType type)
    {
        CutinExtra cutin = null;

        if (type != CutinExtra.ExtraType.None)
        {
            if (extraMap.ContainsKey(type))
            {
                cutin = extraMap[type];
            }
        }

        if (cutin != null)
        {
            if (!cutin.gameObject.activeInHierarchy)
            {
                cutin.gameObject.SetActive(true);
            }
            cutin.StartCutin();
        }
    }

    /// <summary>
    /// カットインが終了したかチェックする。
    /// </summary>
    /// <param name="result">カットインが終了した場合はtrue、そうでない場合はfalseを返す。</param>
    public bool IsEndCutin()
    {
        foreach (var item in cutinMap)
        {
            if (!item.Value.IsEndCutin())
            {
                return false;
            }
        }
        foreach (var item in extraMap)
        {
            if (!item.Value.IsEndCutin())
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 指定したカットインオブジェクトを非アクティブにする。
    /// </summary>
    /// <param name="result">カットインのタイプ</param>
    public void OffCutin(Core.Result result)
    {
        Cutin cutin = null;

        if (result != Core.Result.None)
        {
            if (cutinMap.ContainsKey(result))
            {
                cutin = cutinMap[result];
            }
        }

        if (cutin != null)
        {
            cutin.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 指定したカットインオブジェクトを非アクティブにする。
    /// </summary>
    /// <param name="result">カットインのタイプ</param>
    public void OffCutin(CutinExtra.ExtraType type)
    {
        CutinExtra cutin = null;

        if (type != CutinExtra.ExtraType.None)
        {
            if (extraMap.ContainsKey(type))
            {
                cutin = extraMap[type];
            }
        }

        if (cutin != null)
        {
            cutin.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// すべてのカットインオブジェクトを非アクティブにする。
    /// </summary>
    private void AllOff()
    {
        if (cutinMap != null)
        {
            foreach (var item in cutinMap)
            {
                item.Value.gameObject.SetActive(false);
            }
        }

        if (extraMap != null)
        {
            foreach (var item in extraMap)
            {
                item.Value.gameObject.SetActive(false);
            }
        }
    }
}
