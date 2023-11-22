using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ボールコントローラ
/// </summary>
public class BallController : MonoBehaviour
{
    /// <summary>
    /// 移動速度が下回った場合にボールを止めるスピード
    /// </summary>
    [SerializeField] float minMagunitude = 1.0f;
    /// <summary>
    /// 移動速度が下回った場合に摩擦係数を揚げる際のスピード
    /// </summary>
    [SerializeField] float lowMagunitude = 5.0f;
    /// <summary>
    /// バットにあたったか
    /// </summary>
    [SerializeField] bool hitted;
    /// <summary>
    /// ボールが停止したか
    /// </summary>
    [SerializeField] bool stopped;
    /// <summary>
    /// ボールがバットに当たった際に呼ばれるイベント
    /// </summary>
    [SerializeField] private UnityEvent hittedEvent;
    /// <summary>
    /// ボールを投げた際に呼ばれるイベント
    /// </summary>
    [SerializeField] private UnityEvent shotEvent;
    /// <summary>
    /// ボールが壁にあたった際に呼ばれるイベント
    /// </summary>
    [SerializeField] private UnityEvent boundEvent;

    /// <summary>
    /// ボールのリジッドボディ
    /// </summary>
    private Rigidbody ballRb;
    /// <summary>
    /// ボールの初期位置
    /// </summary>
    private Vector3 originalPos;
    /// <summary>
    /// ボールの初期摩擦係数
    /// </summary>
    private float originalDrag;
    /// <summary>
    /// 止まりやすくする際に使用する摩擦係数
    /// </summary>
    private float heavyDrag;
    /// <summary>
    /// プレー結果
    /// </summary>
    private Core.Result result;
    /// <summary>
    /// ボールが停止した際に結果を確定させるためのコルーチン
    /// </summary>
    private Coroutine fixResultCoroutine;
    /// <summary>
    /// ボールがバットにあたってからの経過時間
    /// </summary>
    private float afterHitTimer;
    /// <summary>
    /// コリジョンした際の対象タグのリスト
    /// </summary>
    private List<string> triggerEnterTags;

    /// <summary>
    /// ボールがバットに当たったあと、停止判定を行わない期間
    /// </summary>
    private static readonly float stopNoCheckTime = 0.1f;

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        triggerEnterTags = new List<string>();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if (stopped)
        {
            StartFixResult();
        }
    }

    /// <summary>
    /// LastUpdate
    /// </summary>
    private void LateUpdate()
    {
        CheckPitchingResult();

        // 打球の加速度が一定以下になった場合はボールを止める
        if (hitted)
        {
            afterHitTimer += Time.deltaTime;

            if (!stopped && afterHitTimer > stopNoCheckTime)
            {
                if (ballRb.velocity.magnitude < minMagunitude)
                {
                    ballRb.velocity = Vector3.zero;
                    ballRb.angularVelocity = Vector3.zero;
                    stopped = true;
                    // Debug.Log("BallController.LateUpdate() ball stopped.");
                }
                else if (ballRb.velocity.magnitude < lowMagunitude)
                {
                    ballRb.drag = heavyDrag;
                }
            }
        }
    }

    /// <summary>
    /// 投球結果(Strike or Ball)を確定させる
    /// </summary>
    private void CheckPitchingResult()
    {
        Core.Result pitchingResult = Core.Result.None;

        // ストライクを優先して判定する。
        if (triggerEnterTags.Contains("Strike"))
        {
            pitchingResult = Core.Result.Strike;
        }
        else if (triggerEnterTags.Contains("NotStrike"))
        {
            pitchingResult = Core.Result.Ball;
        }
        // コリジョン対象リストをクリア
        triggerEnterTags.Clear();

        if (hitted)
        {
            return; // バットにあたったあとなので判定は行わない
        }
        if (result != Core.Result.None)
        {
            return; // 結果確定後なので判定は行わない
        }

        // ストライクまたはボールのエリアに入っていれば結果を確定させる
        if (pitchingResult != Core.Result.None)
        {
            SetResult(pitchingResult);
        }
    }

    /// <summary>
    /// 物理の衝突判定
    /// </summary>
    /// <param name="other">衝突対象</param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bat"))
        {
            // バットにあたった
            // Debug.Log("OnCollisionEnter. hit with " + other.gameObject.name);

            // バッターの回転軸
            Transform axis = other.gameObject.transform.parent;
            if (axis == null)
            {
                return;
            }
            // バッターの大元
            Transform parent = axis.parent;
            if (parent != null)
            {
                BatterController batter = parent.gameObject.GetComponent<BatterController>();
                if (batter != null)
                {
                    // 回転方向によってベクトルを変更する
                    bool takeback = batter.IsTakeback();
                    float direction = (takeback) ? 1.0f : -1.0f;

                    // スイング強度を取得し、ボールに力を加える
                    float swingPower = batter.GetSwingPower();
                    Vector3 forceVec = Quaternion.Euler(0, axis.rotation.eulerAngles.y, 0) * (Vector3.forward * direction);
                    ballRb.AddForce(forceVec * swingPower, ForceMode.Impulse);

                    // Debug.Log(string.Format("swignPower:{0}", swingPower));
                    // Debug.Log(string.Format("forceVec x:{0} y:{1} z:{2}", forceVec.x, forceVec.y, forceVec.z));

                    // バッター側のヒット処理を呼ぶ
                    batter.Hitted();
                    // バットにあたった際のイベントを実行
                    hittedEvent.Invoke();

                    // ヒット後経過時間のタイマーをリセット
                    afterHitTimer = 0.0f;
                    // ヒットフラグを立てる
                    hitted = true;
                }
            }
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            // 壁にあたった
            // Debug.Log("OnCollisionEnter. hit with " + other.gameObject.name);
            // 壁にあった多彩のイベントを実行
            boundEvent.Invoke();
        }
    }

    /// <summary>
    /// 被物理の滞在判定
    /// </summary>
    /// <param name="other">滞在している対象</param>
    private void OnTriggerStay(Collider other)
    {
        if (stopped)
        {
            // ボール停止
            if (result != Core.Result.None)
            {
                return; // すでに結果が確定していれば何もしない
            }

            if (other.CompareTag("HitZone"))
            {
                // ヒットゾーンに止まった
                HitZoneController hitZoneController = other.GetComponent<HitZoneController>();
                if (hitZoneController != null)
                {
                    // 結果を設定
                    SetResult(hitZoneController.GetHitResult());
                }
            }
            else if (other.CompareTag("OutZone"))
            {
                // アウトゾーンに止まった
                OutZoneController outZoneController = other.GetComponent<OutZoneController>();
                if (outZoneController != null)
                {
                    // 結果を設定
                    SetResult(outZoneController.GetHitResult());
                }
            }
            else if (other.CompareTag("FoulZone"))
            {
                // ファウルゾーンに止まった
                FoulZoneController outZoneController = other.GetComponent<FoulZoneController>();
                if (outZoneController != null)
                {
                    // 結果を設定
                    SetResult(outZoneController.GetHitResult());
                }
            }
            else if (other.CompareTag("FoulField"))
            {
                // ファウルエリアに止まった
                FoulZoneController outZoneController = other.GetComponent<FoulZoneController>();
                if (outZoneController != null)
                {
                    // 結果を設定
                    SetResult(outZoneController.GetHitResult());
                }
            }
        }
        else
        {
            // ボール移動中
            if (other.CompareTag("HitZone") ||
                other.CompareTag("OutZone") ||
                other.CompareTag("FoulZone"))
            {
                // ヒットゾーン、アウトゾーン、ファウルゾーンに入った際は摩擦係数を上げる。
                if (hitted)
                {
                    ballRb.drag = heavyDrag;
                }
            }

        }
    }

    /// <summary>
    /// 被物理の衝突判定
    /// </summary>
    /// <param name="other">衝突対象</param>
    private void OnTriggerEnter(Collider other)
    {
        if (result != Core.Result.None)
        {
            return; // すでに結果が確定していれば何もしない
        }

        // ストライクorボールのエリアにあたっていれば対象のタグをリストに追加
        if (other.CompareTag("Strike"))
        {
            // SetResult(Core.Result.Strike);
            triggerEnterTags.Add(other.tag);
        }
        else if (other.CompareTag("NotStrike"))
        {
            // SetResult(Core.Result.Ball);
            triggerEnterTags.Add(other.tag);
        }
    }
    private void SetResult(Core.Result result)
    {
        this.result = result;
        // Debug.Log(string.Format("BallCantoroller result:{0}", this.result));
    }

    /// <summary>
    /// 結果確定コルーチンを起動する
    /// </summary>
    private void StartFixResult()
    {
        if (!hitted)
        {
            return; // まだ打たれていない
        }
        if (!stopped)
        {
            return; // まだ停止していない
        }
        if (result != Core.Result.None)
        {
            return; // 結果確定済み
        }
        if (fixResultCoroutine != null)
        {
            return; // コルーチン起動済み
        }
        fixResultCoroutine = StartCoroutine(FixResult());
    }

    /// <summary>
    /// 結果確定コルーチンを停止させる
    /// </summary>
    private void StopFixResult()
    {
        if (fixResultCoroutine != null)
        {
            StopCoroutine(fixResultCoroutine);
            fixResultCoroutine = null;
        }
    }

    /// <summary>
    /// 結果確定コルーチン
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator FixResult()
    {
        bool isEnd = false;
        float timer = 0.0f;
        float timeoutTime = 0.1f;

        // Debug.Log(string.Format("FixResult() start."));

        while (!isEnd)
        {
            yield return null;

            if (result != Core.Result.None)
            {
                break;  // 結果が確定したので処理を抜ける
            }

            // タイムアウト時間まで経過したが結果が確定しなかったので、アウトとする
            timer += Time.deltaTime;
            if (timer >= timeoutTime)
            {
                SetResult(Core.Result.Out);
                isEnd = true;
            }
        }

        // Debug.Log(string.Format("FixResult() end."));
        fixResultCoroutine = null;
    }

    /// <summary>
    /// スタート時の処理
    /// </summary>
    public void OnStart()
    {
        ballRb = GetComponent<Rigidbody>();
        originalPos = transform.position;
        originalDrag = ballRb.drag;
        heavyDrag = originalDrag * 2.0f;
        afterHitTimer = 0.0f;
    }

    /// <summary>
    /// ボールの状態を初期状態に戻す
    /// </summary>
    public void ResetPosition()
    {
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
        ballRb.drag = originalDrag;
        transform.position = originalPos;
        hitted = false;
        stopped = false;
        result = Core.Result.None;
        StopFixResult();
    }

    /// <summary>
    /// ボールを投球する
    /// </summary>
    /// <param name="shotPower">ボールを投げる際の力</param>
    public void Shot(float shotPower)
    {
        shotEvent.Invoke();
        ballRb.AddForce(Vector3.forward * shotPower, ForceMode.Impulse);
        // Debug.Log(string.Format("add force."));
    }

    /// <summary>
    /// ボールを左右に変化させる
    /// </summary>
    /// <param name="movingPower">ボールに加える力</param>
    public void Moving(float movingPower)
    {
        ballRb.AddForce(Vector3.left * -movingPower);
    }

    /// <summary>
    /// ボールがバットに当たったかチェックする
    /// </summary>
    /// <returns>あたっている場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsHitted()
    {
        return hitted;
    }

    /// <summary>
    /// ボールが停止したかチェックする
    /// </summary>
    /// <returns>停止している場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsStopped()
    {
        return stopped;
    }

    /// <summary>
    /// プレー結果を取得する
    /// </summary>
    /// <returns>プレー結果</returns>
    public Core.Result GetResult()
    {
        return result;
    }
}
