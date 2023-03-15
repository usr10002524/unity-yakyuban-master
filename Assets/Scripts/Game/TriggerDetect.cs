using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コリジョンの結果をコールバックするクラス
/// </summary>
public class TriggerDetect : MonoBehaviour
{
    /// <summary>
    /// コリジョン結果
    /// </summary>
    [SerializeField] private Core.Result triggerResult = Core.Result.None;

    /// <summary>
    /// コールバック関数定義
    /// </summary>
    public delegate void OnTriggerDelegate(Core.Result result);

    /// <summary>
    /// コライダー
    /// </summary>
    protected BoxCollider boxCollider;
    /// <summary>
    /// TriggerEnter判定対象のタグ
    /// </summary>
    protected string triggerEnterInvokeTag;
    /// <summary>
    /// TriggerEnterのコールバック先
    /// </summary>
    protected OnTriggerDelegate triggerEnterHandler;
    /// <summary>
    /// TriggerStay判定対象のタグ
    /// </summary>
    protected string triggerStayInvokeTag;
    /// <summary>
    /// TriggerStayのコールバック先
    /// </summary>
    protected OnTriggerDelegate triggerStayHandler;
    /// <summary>
    /// TriggerExit判定対象のタグ
    /// </summary>
    protected string triggerExitInvokeTag;
    /// <summary>
    /// TriggerExitのコールバック先
    /// </summary>
    protected OnTriggerDelegate triggerExitHandler;


    /// <summary>
    /// TriggerEnterのコールバック先と、対象をセットする
    /// </summary>
    /// <param name="trigger">コールバック先</param>
    /// <param name="tag">対象</param>
    virtual public void SetTriggerEnterDelegate(OnTriggerDelegate trigger, string tag)
    {
        triggerEnterHandler = trigger;
        triggerEnterInvokeTag = tag;
    }

    /// <summary>
    /// TriggerStayのコールバック先と、対象をセットする
    /// </summary>
    /// <param name="trigger">コールバック先</param>
    /// <param name="tag">対象</param>
    virtual public void SetTriggerStayDelegate(OnTriggerDelegate trigger, string tag)
    {
        triggerStayHandler = trigger;
        triggerStayInvokeTag = tag;
    }

    /// <summary>
    /// TriggerExitのコールバック先と、対象をセットする
    /// </summary>
    /// <param name="trigger">コールバック先</param>
    /// <param name="tag">対象</param>
    virtual public void SetTriggerExitDelegate(OnTriggerDelegate trigger, string tag)
    {
        triggerExitHandler = trigger;
        triggerExitInvokeTag = tag;
    }


    /// <summary>
    /// TriggerEnter発生時の処理
    /// </summary>
    /// <param name="other">衝突相手のコライダー</param>
    protected void OnTriggerEnter(Collider other)
    {
        if (triggerEnterHandler != null &&
            other.CompareTag(triggerEnterInvokeTag))
        {
            triggerEnterHandler.Invoke(triggerResult);
        }
    }

    /// <summary>
    /// TriggerStay発生時の処理
    /// </summary>
    /// <param name="other">衝突相手のコライダー</param>
    protected void OnTriggerStay(Collider other)
    {
        if (triggerStayHandler != null &&
            other.CompareTag(triggerStayInvokeTag))
        {
            triggerStayHandler.Invoke(triggerResult);
        }
    }

    /// <summary>
    /// TriggerExit発生時の処理
    /// </summary>
    /// <param name="other">衝突相手のコライダー</param>
    protected void OnTriggerExit(Collider other)
    {
        if (triggerExitHandler != null &&
            other.CompareTag(triggerExitInvokeTag))
        {
            triggerExitHandler.Invoke(triggerResult);
        }
    }

}
