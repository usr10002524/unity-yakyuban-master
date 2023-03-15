using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Angle = UnityUtility.Angle;

/// <summary>
/// バッター管理クラス
/// </summary>
public class BatterController : MonoBehaviour
{
    /// <summary>
    /// バッター回転軸のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject axisObject;
    /// <summary>
    /// バットのゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject batObject;
    /// <summary>
    /// ピッチャーのゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject pitcherObject;
    /// <summary>
    /// スウィング判定用のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject predictionObject;
    /// <summary>
    /// 回転の最小値
    /// </summary>
    [SerializeField] float minAngle;
    /// <summary>
    /// 回転の最大値
    /// </summary>
    [SerializeField] float maxAngle;
    /// <summary>
    /// 回転速度
    /// </summary>
    [SerializeField] float rotSpeed;
    /// <summary>
    /// ボールを打つ際の力
    /// </summary>
    [SerializeField] float baseSwingPower;
    /// <summary>
    /// スイングスピードのアニメーションカーブ
    /// </summary>
    [SerializeField] AnimationCurve swingSpeed;
    /// <summary>
    /// スイングパワーのアニメーションカーブ
    /// </summary>
    [SerializeField] AnimationCurve swingPower;
    /// <summary>
    /// 現在の回転角度
    /// </summary>
    [SerializeField] float curAngle;

    /// <summary>
    /// 回転角度の初期値
    /// </summary>
    private Quaternion originalRot;

    /// <summary>
    /// スイングする角度の幅
    /// </summary>
    private float angleRange;
    /// <summary>
    /// 初期位置からの相対角度
    /// </summary>
    private float angleAdd;

    /// <summary>
    /// スイング開始からの経過時間
    /// </summary>
    private float swingTimer;

    /// <summary>
    /// バッターの操作対象フラグ。プレーヤーが操作している際はtrue、CPUの場合はfalse。
    /// </summary>
    private bool controlByPlayer;
    /// <summary>
    /// テイクバック中フラグ
    /// </summary>
    private bool takeback;
    /// <summary>
    /// バットがボールにあたったフラグ
    /// </summary>
    private bool hitted;
    /// <summary>
    /// スイングを開始したフラグ
    /// </summary>
    private bool swinged;

    /// <summary>
    /// ピッチャー管理クラス
    /// </summary>
    private PitcherController pitcherController;
    /// <summary>
    /// スウィング判定管理クラス
    /// </summary>
    private PredictionContoller predictionContoller;
    /// <summary>
    /// CPUによってテイクバックされているか
    /// </summary>
    private bool cpuTakebacking;
    /// <summary>
    /// CPUのバッティング処理コルーチン
    /// </summary>
    private Coroutine cpuTakebackCoroutine;
    /// <summary>
    /// CPUがスウィング判定を開始するフラグ
    /// </summary>
    private bool cpuPredictionSecondary;
    /// <summary>
    /// CPUが行ったストライクorボールの判定結果
    /// </summary>
    private Core.Result cpuPredictionResult;

    // スイングがトップスピードになるまでの時間
    private static readonly float swingAccellTime = 0.16f;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        pitcherController = pitcherObject.GetComponent<PitcherController>();
        predictionContoller = predictionObject.GetComponent<PredictionContoller>();
        InitCpu();
        InitRotation();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if (GameManager.Instance.IsGameSet())
        {
            return; // ゲーム終了したので何もしない
        }

        if (CheckTakeBacking())
        {
            // テイクバック中
            angleAdd = 0.0f;

            curAngle += (rotSpeed * Time.deltaTime);
            if (curAngle >= minAngle)
            {
                curAngle = minAngle;
            }
            axisObject.transform.rotation = Quaternion.AngleAxis(curAngle, Vector3.up);

            if (hitted)
            {
                hitted = false;
                SetBatColliderTrigger(false);
            }

            takeback = true;
        }
        else
        {
            // スウィング中
            if (takeback)
            {
                // スイングし始めでタイマーをリセット
                swingTimer = 0.0f;
                angleAdd = 0.0f;
            }
            takeback = false;

            // 現在の加速度を算出
            swingTimer += Time.deltaTime;
            float time = Mathf.Clamp(swingTimer / swingAccellTime, 0.0f, 1.0f);
            float t = swingSpeed.Evaluate(time);

            float add = (-rotSpeed * t * Time.deltaTime);
            curAngle += add;
            if (curAngle <= maxAngle)
            {
                curAngle = maxAngle;
                angleAdd = 0.0f;
            }
            else
            {
                angleAdd += Mathf.Abs(add);
            }
            axisObject.transform.rotation = Quaternion.AngleAxis(curAngle, Vector3.up);
            swinged = true;
        }
    }

    /// <summary>
    /// テクバック中かチェックする
    /// </summary>
    /// <returns>テイクバック中の場合はtrue、そうでない場合はfalseを返す</returns>
    private bool CheckTakeBacking()
    {
        if (controlByPlayer)
        {
            // プレーヤー操作中は入力を見る
            bool input = Input.GetMouseButton(0);
            input |= Input.GetKey(KeyCode.Space);

            return input;
        }
        else
        {
            // CPU操作中はフラグを見る
            return cpuTakebacking;
        }
    }

    /// <summary>
    /// バットを初期位置に戻す
    /// </summary>
    private void InitRotation()
    {
        originalRot = Quaternion.AngleAxis(maxAngle, Vector3.up);
        axisObject.transform.rotation = originalRot;
        curAngle = 0.0f;

        angleRange = Mathf.Abs(maxAngle - minAngle);
        angleAdd = 0.0f;

        swingTimer = 0.0f;
    }

    /// <summary>
    /// バットのコリジョンのトリガーフラグを経こうする
    /// </summary>
    /// <param name="trigger">トリガーフラグ</param>
    private void SetBatColliderTrigger(bool trigger)
    {
        Collider collider = batObject.GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = trigger;
        }
        else
        {
            Debug.Log("Collider is null.");
        }
    }

    /// <summary>
    /// バッターの状態をリセットする
    /// </summary>
    private void ResetBatter()
    {
        takeback = false;
        swinged = false;
        InitRotation();
    }

    /// <summary>
    /// CPUの初期化を行う
    /// </summary>
    private void InitCpu()
    {
        predictionContoller.OnInit(CpuPredictionSecondary, "Ball");
    }

    /// <summary>
    /// CPUの状態をリセットする
    /// </summary>
    private void ResetCpu()
    {
        predictionContoller.OnReady();
    }

    /// <summary>
    /// CPUのスウィング処理コルーチンを開始する
    /// </summary>
    private void StartCpuTakebackCroutine()
    {
        // コルーチンがすでに動いていれば止める
        if (cpuTakebackCoroutine != null)
        {
            StopCoroutine(cpuTakebackCoroutine);
            cpuTakebackCoroutine = null;
        }

        // コルーチンを起動する
        if (!controlByPlayer)
        {
            cpuPredictionSecondary = false;
            cpuPredictionResult = Core.Result.None;
            cpuTakebackCoroutine = StartCoroutine(CpuTakebackCoroutine());
        }
    }

    /// <summary>
    /// CPUのスイング処理コルーチン
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator CpuTakebackCoroutine()
    {
        // テイクバック中フラグを立てる
        cpuTakebacking = true;

        // 判定1段階目：このボールを見逃すかどうか
        bool isMissed = CpuManager.Instance.IsBallMissed();
        // Debug.Log(string.Format("BatterController.CpuTakebackCoroutine() IsBallMissed:{0}", isMissed));
        if (isMissed)
        {
            yield break;    // 見逃すのでコルーチンを終了
        }

        // 判定2段階目になるまで待機
        while (!cpuPredictionSecondary)
        {
            yield return null;
        }
        // 判定2段階目：ストライクかボールかによってスイングを行うかどうか判定する
        isMissed = CpuManager.Instance.IsBallMissedSecondary(cpuPredictionResult);
        // Debug.Log(string.Format("BatterController.CpuTakebackCoroutine() cpuPredictionResult:{0} IsBallMissedSecondary:{0}", cpuPredictionResult, isMissed));
        if (isMissed)
        {
            yield break;   // 見逃すのでコルーチンを終了
        }
        cpuPredictionSecondary = false;

        // 投球速度によって、スイング開始タイミングを抽選
        float pitchPower = pitcherController.GetPitchPower();
        float startMin = CpuManager.Instance.GetSwingStartMin(pitchPower);
        float startMax = CpuManager.Instance.GetSwingStartMax(pitchPower);
        float timer = Random.Range(startMin, startMax);
        // Debug.Log(string.Format("cpu min:{0} max:{1} wait:{2}", startMin, startMax, timer));
        // スウィングタイミングになるまで待機
        yield return new WaitForSeconds(timer);

        // スウィング開始
        cpuTakebacking = false;
    }

    /// <summary>
    /// CPUのスウィング判定2段階目開始のを行う際のコールバック。
    /// PrecictionControllerから呼ばれる。
    /// </summary>
    /// <param name="result">投球コースの判定結果</param>
    private void CpuPredictionSecondary(Core.Result result)
    {
        cpuPredictionSecondary = true;
        cpuPredictionResult = result;
        // Debug.Log(string.Format("CpuPredictionSecondary() secondaryTick:{0} during:{1}", secondaryTick, secondaryTick - primaryTick));
    }

    /// <summary>
    /// バッターの状態を初期状態に戻す
    /// </summary>
    /// <param name="isPlayer">プレーヤーが操作するかどうか</param>
    public void OnReady(bool isPlayer)
    {
        ResetBatter();
        ResetCpu();
        controlByPlayer = isPlayer;
        StartCpuTakebackCroutine();
    }

    /// <summary>
    /// テイクバック中かチェックする
    /// </summary>
    /// <returns>テイクバック中の場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsTakeback()
    {
        return takeback;
    }

    /// <summary>
    /// バットがボールにあたった場合の処理を行う
    /// </summary>
    public void Hitted()
    {
        hitted = true;
        SetBatColliderTrigger(true);
    }

    /// <summary>
    /// バッターがスウィングしたかどうかチェックする。
    /// </summary>
    /// <returns>スウィングした場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsSwinged()
    {
        return swinged;
    }

    /// <summary>
    /// スイングパワーを取得する。
    /// </summary>
    /// <returns>スイングパワー</returns>
    public float GetSwingPower()
    {
        float t = GetSwingPowerRatio();
        float power = baseSwingPower * t;

        return power;
    }

    /// <summary>
    /// スイングパワー係数を取得する
    /// </summary>
    /// <returns>スイングパワー係数</returns>
    public float GetSwingPowerRatio()
    {
        // 回転可動域に対する回転角度に応じてスイングパワーの係数を算出する
        float time = angleAdd / angleRange;
        float t = swingPower.Evaluate(time);

        return t;
    }
}
