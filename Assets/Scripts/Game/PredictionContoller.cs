using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コース予測制御クラス
/// </summary>
public class PredictionContoller : TriggerDetect
{
    /// <summary>
    /// コース判定タイミングオブジェクト
    /// </summary>
    [SerializeField] private GameObject predictionObject;
    /// <summary>
    /// コース判定オブジェクト
    /// </summary>
    [SerializeField] private List<GameObject> judgeObjects;

    /// <summary>
    /// トリガー判定クラス
    /// </summary>
    private TriggerDetect triggerDetect;
    /// <summary>
    /// ストライクコースを通過したか
    /// </summary>
    private bool stayStrike;
    /// <summary>
    /// ボールコースを通過したか
    /// </summary>
    private bool stayBall;


    /// <summary>
    /// 初期化を行う
    /// </summary>
    /// <param name="trigger">コールバックオブジェクト</param>
    /// <param name="tag">判定対象のタグ</param>
    public void OnInit(OnTriggerDelegate trigger, string tag)
    {
        // 自身のTriggerExitコールバックに設定
        SetTriggerExitDelegate(trigger, tag);

        // タイミング判定オブジェクトのコールバックを設定
        triggerDetect = predictionObject.GetComponent<TriggerDetect>();
        if (triggerDetect)
        {
            triggerDetect.SetTriggerExitDelegate(PridictionExitCallback, tag);
        }

        // コース判定オブジェクトのコールバックを設定
        foreach (var item in judgeObjects)
        {
            TriggerDetect trig = item.GetComponent<TriggerDetect>();
            if (trig != null)
            {
                trig.SetTriggerEnterDelegate(TriggerEnterCallback, tag);
                trig.SetTriggerStayDelegate(TriggerStayCallback, tag);
                trig.SetTriggerExitDelegate(TriggerExitCallback, tag);
            }
        }
    }

    /// <summary>
    /// 初期状態にリセットする
    /// </summary>
    public void OnReady()
    {
        stayStrike = false;
        stayBall = false;
    }

    /// <summary>
    /// 外部から基底クラスを呼ばせないようにスコープをprivateに変更する。
    /// </summary>
    /// <param name="trigger">コールバック</param>
    /// <param name="tag">対象のタグ</param>
    private new void SetTriggerEnterDelegate(OnTriggerDelegate trigger, string tag)
    {
        base.SetTriggerEnterDelegate(trigger, tag);
    }

    /// <summary>
    /// 外部から基底クラスを呼ばせないようにスコープをprivateに変更する。
    /// </summary>
    /// <param name="trigger">コールバック</param>
    /// <param name="tag">対象のタグ</param>
    private new void SetTriggerStayDelegate(OnTriggerDelegate trigger, string tag)
    {
        base.SetTriggerStayDelegate(trigger, tag);
    }

    /// <summary>
    /// 外部から基底クラスを呼ばせないようにスコープをprivateに変更する。
    /// </summary>
    /// <param name="trigger">コールバック</param>
    /// <param name="tag">対象のタグ</param>
    private new void SetTriggerExitDelegate(OnTriggerDelegate trigger, string tag)
    {
        base.SetTriggerExitDelegate(trigger, tag);
    }

    /// <summary>
    /// コース予測オブジェクトTriggerEnterコールバック
    /// </summary>
    /// <param name="result">結果</param>
    private void TriggerEnterCallback(Core.Result result)
    {
        SetResult(result);
    }

    /// <summary>
    /// コース予測オブジェクトTriggerStayコールバック
    /// </summary>
    /// <param name="result">結果</param>
    private void TriggerStayCallback(Core.Result result)
    {
        SetResult(result);

    }

    /// <summary>
    /// コース予測オブジェクトTriggerExitコールバック
    /// </summary>
    /// <param name="result">結果</param>
    private void TriggerExitCallback(Core.Result result)
    {
        SetResult(result);
    }

    /// <summary>
    /// タイミング予測オブジェクトからのコールバックが来たら、予測結果をコールバックする
    /// </summary>
    /// <param name="result">結果</param>
    private void PridictionExitCallback(Core.Result result)
    {
        // Debug.Log(string.Format("PredicationController.PridictionExitCallback() stayStrike:{0} stayBall:{1}", stayStrike, stayBall));
        if (triggerExitHandler != null)
        {
            Core.Result triggerResult = Core.Result.None;
            if (stayStrike)
            {
                triggerResult = Core.Result.Strike;
            }
            else if (stayBall)
            {
                triggerResult = Core.Result.Ball;
            }
            else
            {
                triggerResult = Core.Result.Strike;
            }

            // Debug.Log(string.Format("PredicationController.PridictionExitCallback() triggerResult:{0} ", triggerResult));
            triggerExitHandler.Invoke(triggerResult);
        }
    }

    /// <summary>
    /// コース結果予測をセットする
    /// </summary>
    /// <param name="result">コース結果予測</param>
    private void SetResult(Core.Result result)
    {
        switch (result)
        {
            case Core.Result.Strike: stayStrike = true; break;
            case Core.Result.Ball: stayBall = true; break;
        }
    }
}
