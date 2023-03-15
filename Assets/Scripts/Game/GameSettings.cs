using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム設定クラス
/// TitleSceneからMainSceneに持ち越すための仕組み
/// </summary>
public class GameSettings : MonoBehaviour
{
    /// <summary>
    /// プレーヤーが先行かどうか
    /// </summary>
    public Core.Order order { get; private set; }
    /// <summary>
    /// イニング数
    /// </summary>
    public int innings { get; private set; }
    /// <summary>
    /// コールドゲームとなる得点差
    /// </summary>
    public int calledScore { get; private set; }
    /// <summary>
    /// 先攻チームの名前
    /// </summary>
    public string firstTeamName { get; private set; }
    /// <summary>
    /// 後攻チームの名前
    /// </summary>
    public string secondTeamName { get; private set; }

    /// <summary>
    /// シングルトン用インスタンス
    /// </summary>
    public static GameSettings Instance { get; private set; }

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
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    /// <summary>
    /// プレーヤーが先行か後攻かを設定する。
    /// </summary>
    /// <param name="order">プレーヤーが先行か後攻か</param>
    public void SetOrder(Core.Order order)
    {
        this.order = order;
    }

    /// <summary>
    /// イニング数を設定する
    /// </summary>
    /// <param name="innings">イニング数</param>
    public void SetInnings(int innings)
    {
        this.innings = innings;
    }

    /// <summary>
    /// コールドゲームとなる得点差を設定する
    /// </summary>
    /// <param name="calledScore">コールドゲームとなる得点差</param>
    public void SetCalledScore(int calledScore)
    {
        this.calledScore = calledScore;
    }

    /// <summary>
    /// 先攻チームの名前を設定する
    /// </summary>
    /// <param name="teamName">先攻チームの名前</param>
    public void SetFirstTeamName(string teamName)
    {
        this.firstTeamName = teamName;
    }

    /// <summary>
    /// 後攻チームの名前を設定する
    /// </summary>
    /// <param name="teamName">後攻チームの名前</param>
    public void SetSecondTeamName(string teamName)
    {
        this.secondTeamName = teamName;
    }

    /// <summary>
    /// ゲーム設定が有効かどうかをチェックする
    /// </summary>
    /// <returns>有効の場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsValid()
    {
        if (innings == 0)
        {
            return false;
        }
        if (calledScore == 0)
        {
            return false;
        }
        if (firstTeamName.Length == 0)
        {
            return false;
        }
        if (secondTeamName.Length == 0)
        {
            return false;
        }
        return true;
    }
}
