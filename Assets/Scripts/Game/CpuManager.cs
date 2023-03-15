using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CPU管理クラス
/// </summary>
public class CpuManager : MonoBehaviour
{
    /// <summary>
    /// CPUの強さ
    /// </summary>
    public enum Difficulty
    {
        Low,
        Midium,
        High,

        Max,
    }

    /// <summary>
    /// CPUの強さ
    /// </summary>
    [SerializeField] Difficulty difficulty;

    /// <summary>
    /// シングルトン用インスタンス
    /// </summary>
    public static CpuManager Instance { get; private set; }

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// 有効な値がセットされているか確認する
    /// </summary>
    /// <returns>有効な場合はtrue、そうでない場合はfalseを返す。</returns>
    private bool IsValid()
    {
        switch (difficulty)
        {
            case Difficulty.Low:
            case Difficulty.Midium:
            case Difficulty.High:
                return true;
            default:
                return false;
        }
    }

    /// <summary>
    /// 難易度に応じたインデックスを返す
    /// </summary>
    /// <returns>難易度に応じたインデックス</returns>
    private int GetDifficultyIndex()
    {
        if (IsValid())
        {
            return (int)difficulty;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 各難易度における判定1段階目の見逃し率
    /// </summary>
    private static readonly float[] missRate = new float[] { 0.0f, 0.0f, 0.0f };
    /// <summary>
    /// 各難易度のストライク時における判定2段階目の見逃し率
    /// </summary>
    private static readonly float[] missStrikeRate = new float[] { 0.3f, 0.3f, 0.2f };
    /// <summary>
    /// 各難易度のボール時における判定2段階目の見逃し率
    /// </summary>
    private static readonly float[] missBallRate = new float[] { 0.3f, 0.5f, 0.8f };

    /// <summary>
    /// 判定1段階目でボールを見逃すかどうかをチェックする
    /// </summary>
    /// <returns>ボールを見逃す場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsBallMissed()
    {
        // #if false    //@@@ always missed
        //         return true;
        // #else
        int difficultyIndex = GetDifficultyIndex();
        float rate = missRate[difficultyIndex];
        return (Random.Range(0.0f, 1.0f) < rate);
        // #endif
    }

    /// <summary>
    /// 判定2段階目でボールを見逃すかどうかをチェックする
    /// </summary>
    /// <param name="result">ボールを見逃す場合はtrue、そうでない場合はfalseを返す。</param>
    /// <returns></returns>
    public bool IsBallMissedSecondary(Core.Result result)
    {
        // #if false    //@@@ always missed
        //         return true;
        // #else
        int difficultyIndex = GetDifficultyIndex();
        switch (result)
        {
            case Core.Result.Ball:
                {
                    float rate = missBallRate[difficultyIndex];
                    return (Random.Range(0.0f, 1.0f) < rate);
                }
            default:
                {
                    float rate = missStrikeRate[difficultyIndex];
                    return (Random.Range(0.0f, 1.0f) < rate);
                }
        }
        // #endif
    }

    /// <summary>
    /// ピッチパワーをインデックスに変換する
    /// </summary>
    /// <param name="pitchPower">ピッチパワー</param>
    /// <returns>インデックス</returns>
    private int GetPitchPowerIndex(float pitchPower)
    {
        if (pitchPower < 0.1f) { return 0; }
        else if (0.1f <= pitchPower && pitchPower < 0.2f) { return 1; }
        else if (0.2f <= pitchPower && pitchPower < 0.3f) { return 2; }
        else if (0.3f <= pitchPower && pitchPower < 0.4f) { return 3; }
        else if (0.4f <= pitchPower && pitchPower < 0.5f) { return 4; }
        else if (0.5f <= pitchPower && pitchPower < 0.6f) { return 5; }
        else if (0.6f <= pitchPower && pitchPower < 0.7f) { return 6; }
        else if (0.7f <= pitchPower && pitchPower < 0.8f) { return 7; }
        else if (0.8f <= pitchPower && pitchPower < 0.9f) { return 8; }
        else { return 9; }
    }

    /// <summary>
    /// 各投球速度、CPU難易度におけるスイング開始の最小タイミング
    /// </summary>
    private static readonly float[,] swingStartMin = new float[,] {
        {   0.0f,  0.7f,    1.05f,   },
        {   0.0f,  0.475f,  0.7125f, },
        {   0.0f,  0.325f,  0.4875f, },
        {   0.0f,  0.225f,  0.3375f, },
        {   0.0f,  0.175f,  0.2625f, },
        {   0.0f,  0.125f,  0.1875f, },
        {   0.0f,  0.1f,    0.15f,   },
        {   0.0f,  0.075f,  0.1125f, },
        {   0.0f,  0.075f,  0.1125f, },
        {   0.0f,  0.05f,   0.075f,  },
        {   0.0f,  0.025f,  0.0375f, },
    };

    /// <summary>
    /// 各投球速度、CPU難易度におけるスイング開始の最小タイミングを取得する
    /// </summary>
    /// <value>スイング開始の最小タイミング</value>
    public float GetSwingStartMin(float pitchPower)
    {
        int difficultyIndex = GetDifficultyIndex();
        int pitchPowerIndex = GetPitchPowerIndex(pitchPower);
        return swingStartMin[pitchPowerIndex, difficultyIndex];
    }

    /// <summary>
    /// 各投球速度、CPU難易度におけるスイング開始の最大タイミング
    /// </summary>
    private static readonly float[,] swingStartMax = new float[,] {
        {   5.0f,  3.2f,    2.3f,    },
        {   2.0f,  1.475f,  1.2125f, },
        {   1.6f,    1.125f,  0.8875f, },
        {   1.2f,    0.825f,  0.6375f, },
        {   0.8f,    0.575f,  0.4625f, },
        {   0.8f,    0.525f,  0.3875f, },
        {   0.8f,    0.5f,    0.35f,   },
        {   0.6f,    0.375f,  0.2625f, },
        {   0.6f,    0.375f,  0.2625f, },
        {   0.5f,    0.3f,    0.2f,    },
        {   0.5f,    0.275f,  0.1625f, },
    };

    /// <summary>
    /// 各投球速度、CPU難易度におけるスイング開始の最大タイミングを取得する
    /// </summary>
    /// <value>スイング開始の最大タイミング</value>
    public float GetSwingStartMax(float pitchPower)
    {
        int difficultyIndex = GetDifficultyIndex();
        int pitchPowerIndex = GetPitchPowerIndex(pitchPower);
        return swingStartMax[pitchPowerIndex, difficultyIndex];
    }

}
