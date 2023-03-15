using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ピッチャー制御クラス
/// </summary>
public class PitcherController : MonoBehaviour
{
    /// <summary>
    /// ボールオブジェクト
    /// </summary>
    [SerializeField] private GameObject ballObject;
    /// <summary>
    /// バッターオブジェクト
    /// </summary>
    [SerializeField] private GameObject batterObject;
    /// <summary>
    /// スライダーUI
    /// </summary>
    [SerializeField] private Slider slider;
    /// <summary>
    /// 投球コースの幅
    /// </summary>
    [SerializeField] private float xRange;
    /// <summary>
    /// 左右の移動速度
    /// </summary>
    [SerializeField] private float xSpeed;
    /// <summary>
    /// 投球速度の最小
    /// </summary>
    [SerializeField] private float minPower = 50;
    /// <summary>
    /// 投球速度の最大
    /// </summary>
    [SerializeField] private float maxPower = 100;
    /// <summary>
    /// 投球速度
    /// </summary>
    [SerializeField] private float pitchPower;
    /// <summary>
    /// 投球速度選択バーの速度
    /// </summary>
    [SerializeField] private float powerSpeed;
    /// <summary>
    /// 投球速度選択バーの移動方向
    /// </summary>
    [SerializeField] private bool powerDirection;
    /// <summary>
    /// 変化させるときの力
    /// </summary>
    [SerializeField] private float movingForce;
    /// <summary>
    /// 変化させるときの力の最大
    /// </summary>
    [SerializeField] private float movingLimit;
    /// <summary>
    /// 【デバッグ用】投球時の力を固定する
    /// </summary>
    [SerializeField] private float debugShotPower;

    /// <summary>
    /// 初期位置
    /// </summary>
    private Vector3 originalPos;
    /// <summary>
    /// 投球準備中か
    /// </summary>
    private bool readyPitch;
    /// <summary>
    /// 投球したか
    /// </summary>
    private bool ballShot;
    /// <summary>
    /// ボール制御クラス
    /// </summary>
    private BallController ballController;
    /// <summary>
    /// プレーヤーが操作しているか
    /// </summary>
    private bool controlByPlayer;
    /// <summary>
    /// CPUがテイクバック中か
    /// </summary>
    private bool cpuTakebacking;
    /// <summary>
    /// CPU投球用のコルーチン
    /// </summary>
    private Coroutine cpuTakebackCoroutine;

    /// <summary>
    /// マウス右ボタンがクリックされたか
    /// </summary>
    private bool mouseRButtonClicked;
    /// <summary>
    /// マウス右ボタンがクリックされた位置
    /// </summary>
    private Vector2 mouseRButtonClickedPosition;

    /// <summary>
    /// バッター制御クラス
    /// </summary>
    private BatterController batterController;

    /// <summary>
    /// 右ドラッグによるコース選択で反応する最小の距離
    /// </summary>
    static private readonly float minDragLength = 10.0f;

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        ballController = ballObject.GetComponent<BallController>();
        ballController.OnStart();

        batterController = batterObject.GetComponent<BatterController>();

        originalPos = transform.position;
        ResetPitch();
        ResetPosition();
    }

    /// <summary>
    /// Update
    /// </summary>
    void Update()
    {
        if (GameManager.Instance.IsGameSet())
        {
            return;
        }

        PowerSelect();
        CourseSelect();

        UpdateBall();
    }

    /// <summary>
    /// スライダーの初期化
    /// </summary>
    private void SetupSlider()
    {
        slider.minValue = 0.0f;
        slider.maxValue = 1.0f;
        slider.value = 0.0f;
    }

    /// <summary>
    /// 投球コースの選択を行う
    /// </summary>
    private void CourseSelect()
    {
        if (!controlByPlayer)
        {
            return;
        }

        if (readyPitch)
        {
            return;
        }

        float horizontalInput = GetHorizontalInput();

        float xPos = transform.position.x;
        xPos += horizontalInput * Time.deltaTime * xSpeed;
        if (xPos < -xRange) { xPos = -xRange; }
        if (xPos > xRange) { xPos = xRange; }

        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// 投球スピードの選択を行う
    /// </summary>
    private void PowerSelect()
    {
        if (CheckTakeBacking())
        {
            if (!readyPitch)
            {
                // 投球準備に入る
                readyPitch = true;
                if (controlByPlayer)
                {
                    // 投球速度選択スライダーを表示する
                    slider.gameObject.SetActive(true);
                }
                else
                {
                    // CPUの場合は非表示にする
                    slider.gameObject.SetActive(false);
                }

                // ランダムで初期位置を方向をセットする
                pitchPower = Random.Range(0.0f, 1.0f);
                powerDirection = (0 == (Random.Range(0, 1024) % 2)) ? true : false;
            }


            // 投球速度選択スライダーを上下させる
            if (powerDirection)
            {
                pitchPower += (powerSpeed * Time.deltaTime);
                if (pitchPower > 1.0f)
                {
                    pitchPower = 1.0f;
                    powerDirection = !powerDirection;
                }
            }
            else
            {
                pitchPower -= (powerSpeed * Time.deltaTime);
                if (pitchPower < 0.0f)
                {
                    pitchPower = 0.0f;
                    powerDirection = !powerDirection;
                }
            }

            slider.value = pitchPower;
        }
        else if (CheckRelease())
        {
            // 投球を行う
            if (readyPitch && !ballShot)
            {
                ballShot = true;
                float range = maxPower - minPower;
                float power = minPower + range * pitchPower;
                slider.value = pitchPower;

                ballController.Shot(power);
                slider.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// テイクバック中か判定する
    /// </summary>
    /// <returns>テイクバック中はtrue、そうでない場合はfalseを返す。</returns>
    private bool CheckTakeBacking()
    {
        if (controlByPlayer)
        {
            // プレーヤーの場合は入力を見る
            bool input = Input.GetMouseButton(0);
            input |= Input.GetKey(KeyCode.Space);

            return input;
        }
        else
        {
            return cpuTakebacking;
        }
    }

    /// <summary>
    /// ボールをリリースしたか
    /// </summary>
    /// <returns>リリースした場合はtrue、そうでない場合はfalseを返す。</returns>
    private bool CheckRelease()
    {
        if (controlByPlayer)
        {
            bool input = Input.GetMouseButtonUp(0);
            input |= Input.GetKeyUp(KeyCode.Space);

            return input;
        }
        else
        {
            return !cpuTakebacking;
        }
    }

    /// <summary>
    /// ピッチャーの状態をリセットする
    /// </summary>
    private void ResetPitch()
    {
        readyPitch = false;
        ballShot = false;
        powerDirection = true;
        pitchPower = 0.0f;
        slider.gameObject.SetActive(false);
    }

    /// <summary>
    /// ピッチャーの位置をリセットする
    /// </summary>
    private void ResetPosition()
    {
        transform.position = originalPos;
        ballController.ResetPosition();
    }

    /// <summary>
    /// 投球後、ある程度まではボールを左右に変化させる
    /// </summary>
    private void UpdateBall()
    {
        // 入力によって多少変化させる
        if (!InPitching())
        {
            return;
        }
        if (!controlByPlayer)
        {
            return;
        }

        Vector3 length = ballObject.transform.position - transform.position;
        if (length.magnitude < movingLimit)
        {
            // float horizontalInput = Input.GetAxis("Horizontal");
            float horizontalInput = GetHorizontalInput();
            float force = horizontalInput * movingForce;
            ballController.Moving(force);
        }
    }

    /// <summary>
    /// 投球中かどうかをチェックする
    /// </summary>
    /// <returns>投球中の場合はtrue、そうでない場合はfalseを返す</returns>
    private bool InPitching()
    {
        if (!ballShot)
        {
            return false;
        }
        if (ballController.IsHitted())
        {
            return false;
        }
        if (ballController.GetResult() != Core.Result.None)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// CPUの投球用コルーチンを開始する
    /// </summary>
    private void StartCpuTakebackCroutine()
    {
        // コルーチンがすでに動いていれば止める
        if (cpuTakebackCoroutine != null)
        {
            StopCoroutine(cpuTakebackCoroutine);
            cpuTakebackCoroutine = null;
        }

        if (!controlByPlayer)
        {
            cpuTakebackCoroutine = StartCoroutine(CpuTakebackCoroutine());
        }
    }

    /// <summary>
    /// CPUの投球用コルーチン
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator CpuTakebackCoroutine()
    {
        yield return new WaitForSeconds(1.0f);

        // コース選択の時間をランダムで決める
        float timer = 0.0f;
        float courseSelectTime = Random.Range(0.5f, 1.5f);
        int dir = ((Random.Range(0, 1023) % 2) == 0) ? 1 : -1;

        // 左右に動いてコースを決める
        while (timer < courseSelectTime)
        {
            yield return null;
            timer += Time.deltaTime;

            float xPos = transform.position.x;
            xPos += dir * Time.deltaTime * xSpeed;
            if (xPos < -xRange)
            {
                xPos = -xRange;
                dir *= -1;
            }
            if (xPos > xRange)
            {
                xPos = xRange;
                dir *= -1;
            }

            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }

        // 投球開始
        cpuTakebacking = true;
        // バッターの準備ができるまで待つ
        while (!batterController.IsTakeback())
        {
            yield return null;
        }

        // ランダム時間待って投球を行う
        timer = Random.Range(0.5f, 2.0f);

        while (timer > 0.0f)
        {
            yield return null;
            timer -= Time.deltaTime;
        }

        cpuTakebacking = false;
    }

    /// <summary>
    /// 左右キーの入力を見る
    /// </summary>
    /// <returns>左右キーの入力</returns>
    private float GetHorizontalInput()
    {
        float horizontalInput = 0.0f;
        bool rButtonInput = Input.GetMouseButton(1);
        if (mouseRButtonClicked)
        {
            if (rButtonInput)
            {
                Vector2 mousePos = Input.mousePosition;
                Vector2 dir = mousePos - mouseRButtonClickedPosition;
                if (dir.x < -minDragLength)
                {
                    horizontalInput = -xRange * 0.1f;
                }
                else if (dir.x > minDragLength)
                {
                    horizontalInput = xRange * 0.1f;
                }
            }
            else
            {
                mouseRButtonClicked = false;
                mouseRButtonClickedPosition = Vector2.zero;
            }
        }
        else
        {
            if (rButtonInput)
            {
                mouseRButtonClicked = true;
                mouseRButtonClickedPosition = Input.mousePosition;
            }
            else
            {
                horizontalInput = Input.GetAxis("Horizontal");
            }
        }
        return horizontalInput;
    }


    /// <summary>
    /// ピッチャーの状態を初期状態に戻す
    /// </summary>
    /// <param name="isPlayer">プレーヤーが操作するかどうか</param>
    public void OnReady(bool isPlayer)
    {
        ResetPitch();
        ResetPosition();
        controlByPlayer = isPlayer;
        StartCpuTakebackCroutine();
    }

    /// <summary>
    /// 投球準備中かどうかをチェックする
    /// </summary>
    /// <returns>投球準備中の場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsReady()
    {
        return readyPitch;
    }

    /// <summary>
    /// 投球を行ったかどうかをチェックする
    /// </summary>
    /// <returns>投球を行った場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsShot()
    {
        return ballShot;
    }

    /// <summary>
    /// 投球速度を取得する
    /// </summary>
    /// <returns>投球速度</returns>
    public float GetPitchPower()
    {
        return pitchPower;
    }

}
