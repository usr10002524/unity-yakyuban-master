using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// エイリアス
using Order = Core.Order;

/// <summary>
/// ゲームマネージャクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 初期化パラメータ
    /// </summary>
    public class Initializer
    {
        /// <summary>
        /// イニング数
        /// </summary>
        public int innings;
        /// <summary>
        /// 先攻か後攻か
        /// </summary>
        public Order order;
        /// <summary>
        /// コールドとなる点差
        /// </summary>
        public int calledScore;
        /// <summary>
        /// 先攻チームの名前
        /// </summary>
        public string firstTeamName;
        /// <summary>
        /// 後攻チームの名前
        /// </summary>
        public string secondTeamName;
    }

    /// <summary>
    /// 初期化パラメータ
    /// </summary>
    private Initializer initializer;
    /// <summary>
    /// スコア管理クラス
    /// </summary>
    private Score score;
    /// <summary>
    /// カウント管理クラス
    /// </summary>
    private PitchingCount count;
    /// <summary>
    /// ランナー管理クラス
    /// </summary>
    private Runners runners;

    /// <summary>
    /// 現在のイニング
    /// </summary>
    private int currentInning;
    /// <summary>
    /// 現在の攻守
    /// </summary>
    private Order currentOrder;
    /// <summary>
    /// 三振を取ったか
    /// </summary>
    private bool isStruckOut;
    /// <summary>
    /// フォアボールか
    /// </summary>
    private bool isFourBall;
    /// <summary>
    /// チェンジか
    /// </summary>
    private bool isChange;
    /// <summary>
    /// ゲームセットか
    /// </summary>
    private bool isGameSet;
    /// <summary>
    /// サヨナラか
    /// </summary>
    private bool isSayonara;
    /// <summary>
    /// コールドゲームか
    /// </summary>
    private bool isCalledGame;
    /// <summary>
    /// 最終イニング勝ち越しのためウラの攻撃を行わなかったか
    /// </summary>
    private bool isNoLastSecondInning;

    /// <summary>
    /// シングルトン用のインスタンスを取得する
    /// </summary>
    /// <value>シングルトン用のインスタンス</value>
    public static GameManager Instance { get; private set; }


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
    /// 初期化を行う
    /// </summary>
    /// <param name="init">初期化パラメータ</param>
    public void Initialize(Initializer init)
    {
        initializer = init;
        score = new Score(initializer.innings);
        count = new PitchingCount();
        runners = new Runners();
        // teamName = new TeamName();
        // teamName.SetFirstTeam();
        // teamName.SetSecondTeam();
        currentInning = 0;
        currentOrder = Order.First;
        isGameSet = false;
        isSayonara = false;
        isCalledGame = false;
        isNoLastSecondInning = false;

        // Debug.Log(string.Format("GameManager.Initialize() innings:{0} order:{1}", initializer.innings, initializer.order));
    }

    /// <summary>
    /// 初期化時に設定されたイニング数を取得する
    /// </summary>
    /// <returns>初期化時に設定されたイニング数</returns>
    public int GetInnings()
    {
        return initializer.innings;
    }

    /// <summary>
    /// プレーヤーが先行か後攻かを取得する
    /// </summary>
    /// <returns>プレーヤーが先行か後攻か</returns>
    public Order GetOrder()
    {
        return initializer.order;
    }

    /// <summary>
    /// 先攻チームのチーム名を取得する
    /// </summary>
    /// <returns>先攻チームのチーム名</returns>
    public string GetFirstTeamName()
    {
        return initializer.firstTeamName;
    }

    /// <summary>
    /// 後攻チームのチーム名を取得する
    /// </summary>
    /// <returns>後攻チームのチーム名</returns>
    public string GetSecondTeamName()
    {
        return initializer.secondTeamName;
    }

    /// <summary>
    /// 現在のイニングを取得する
    /// </summary>
    /// <returns>現在のイニング</returns>
    public int GetCurrentInning()
    {
        return currentInning;
    }

    /// <summary>
    /// 現在がオモテか裏かを取得する
    /// </summary>
    /// <returns>現在がオモテか裏か</returns>
    public Order GetCurrentOrder()
    {
        return currentOrder;
    }

    /// <summary>
    /// プレーヤーが攻撃中かどうかをチェックする
    /// </summary>
    /// <returns>プレーヤーが攻撃中の場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsBatting()
    {
        return (initializer.order == currentOrder);
    }

    /// <summary>
    /// プレーヤーが守備中かどうかをチェックする
    /// </summary>
    /// <returns>プレーヤーが守備中の場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsFielding()
    {
        return (initializer.order != currentOrder);
    }

    /// <summary>
    /// 最終回かどうかチェックする
    /// </summary>
    /// <returns>最終回の場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsLastInning()
    {
        return IsLastInning(currentInning);
    }

    /// <summary>
    /// 指定したイニングが最終回かどうかチェックする
    /// </summary>
    /// <param name="inning">イニング数</param>
    /// <returns>最終回の場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsLastInning(int inning)
    {
        return (inning == initializer.innings - 1);
    }

    /// <summary>
    /// ゲームセットかどうかをチェックする
    /// </summary>
    /// <returns>ゲームセットの場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsGameSet()
    {
        return isGameSet;
    }

    /// <summary>
    /// チェンジかどうかをチェックする
    /// </summary>
    /// <returns>チェンジの場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsChange()
    {
        return isChange;
    }

    /// <summary>
    /// サヨナラかどうかをチェックする
    /// </summary>
    /// <returns>サヨナラの場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsSayonara()
    {
        return isSayonara;
    }

    /// <summary>
    /// コールドゲームかどうかをチェックする
    /// </summary>
    /// <returns>コールドゲームの場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsCalledGame()
    {
        return isCalledGame;
    }

    /// <summary>
    /// 三振かどうかをチェックする
    /// </summary>
    /// <returns>三振の場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsStruckOut()
    {
        return isStruckOut;
    }

    /// <summary>
    /// フォアボールかどうかをチェックする
    /// </summary>
    /// <returns>フォアボールの場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsFourBall()
    {
        return isFourBall;
    }

    /// <summary>
    /// プレーヤーが勝ったかどうかをチェックする
    /// </summary>
    /// <returns>プレーヤーが勝った場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsWin()
    {
        if (initializer.order == Core.Order.First)
        {
            return (score.GetTotal(Core.Order.First) > score.GetTotal(Core.Order.Second));
        }
        else
        {
            return (score.GetTotal(Core.Order.Second) > score.GetTotal(Core.Order.First));
        }
    }

    /// <summary>
    /// プレーヤーが負けたかどうかをチェックする
    /// </summary>
    /// <returns>プレーヤーが負けた場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsLose()
    {
        if (initializer.order == Core.Order.First)
        {
            return (score.GetTotal(Core.Order.Second) > score.GetTotal(Core.Order.First));
        }
        else
        {
            return (score.GetTotal(Core.Order.First) > score.GetTotal(Core.Order.Second));
        }
    }

    /// <summary>
    /// 引き分けかどうかをチェックする
    /// </summary>
    /// <returns>引き分けの場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsDraw()
    {
        return (score.GetTotal(Core.Order.First) == score.GetTotal(Core.Order.Second));
    }

    /// <summary>
    /// 後攻で勝ち越しが確定したため、最終イニング裏の攻撃を行わなかったかどうかをチェックする
    /// </summary>
    /// <returns>最終イニング裏の攻撃を行わなかった場合はtrue、そうでない場合はfalseを返す。</returns>
    public bool IsNoLastSecondInning()
    {
        return isNoLastSecondInning;
    }


    /// <summary>
    /// プレー結果を反映させる
    /// </summary>
    /// <param name="result">プレー結果</param>
    public void UpdateInning(Core.Result result)
    {
        if (IsGameSet())
        {
            return; // ゲームセットしたので何もしない
        }

        isChange = false;
        isStruckOut = false;
        isFourBall = false;

        // piching
        count.ApplyResult(result);
        if (count.IsStruckOut())
        {
            isStruckOut = true;
            count.AddOut();
            count.ReserveCountReset();
        }
        else if (count.IsFourBall())
        {
            isFourBall = true;
            runners.ApplyFourBall();
            count.ReserveCountReset();
        }

        // batting
        if (result == Core.Result.Out)
        {
            count.AddOut();
            count.ReserveCountReset();
        }
        else if (result == Core.Result.Foul)
        {
            count.ApplyFoul();
        }
        else
        {
            runners.ApplyResult(result);
            int homeInCount = runners.GetHomeInCount();
            // サヨナラ勝ちの場合は調整を入れる
            homeInCount = AdjustSayonara(homeInCount, result);
            score.Add(currentInning, currentOrder, homeInCount);

            if (IsHit(result))
            {
                count.ReserveCountReset();
            }
        }

        if (count.IsChange())
        {
            count.UpdateCount();
            runners.Reset();
            ProgressInning();
        }
        if (score.IsSayonara() || IsSayonara())
        {
            count.UpdateCount();
            runners.Reset();
            ProgressInning();
        }
        else
        {
            count.UpdateCount();
        }
    }

    /// <summary>
    /// スコア管理クラスを取得する
    /// </summary>
    /// <returns>スコア管理クラス</returns>
    public Score GetScore()
    {
        return score;
    }

    /// <summary>
    /// ピッチングカウント管理クラスを取得する
    /// </summary>
    /// <returns>ピッチングカウント管理クラス</returns>
    public PitchingCount GetPitchingCount()
    {
        return count;
    }

    /// <summary>
    /// ランナー管理クラスを取得する。
    /// </summary>
    /// <returns>ランナー管理クラス</returns>
    public Runners GetRunners()
    {
        return runners;
    }

    /// <summary>
    /// イニングを進める
    /// </summary>
    private void ProgressInning()
    {
        if (currentOrder == Core.Order.First)
        {
            // 最終イニングかつ、後攻の得点が多い場合はゲームセット
            if (CheckGameSet())
            {
                isNoLastSecondInning = true;
                isGameSet = true;
            }
            else
            {
                // オモテの攻撃だった場合は裏の攻撃へ
                currentOrder = Core.Order.Second;
                isChange = true;
            }
        }
        else
        {
            if (CheckGameSet())
            {
                // 最終イニングの場合はゲームセット
                isGameSet = true;
            }
            else
            {
                // 裏の攻撃だった場合は次のイニングへ
                currentInning++;
                currentOrder = Core.Order.First;
                isChange = true;
            }
        }
    }

    /// <summary>
    /// ゲーム終了かどうかを判定する
    /// </summary>
    /// <returns>ゲーム終了の場合はtrue、そうでない場合はfalseを返す</returns>
    private bool CheckGameSet()
    {
        if (currentOrder == Core.Order.First)
        {
            // オモテの攻撃
            if (IsLastInning())
            {
                // 最終イニングかつ、後攻の得点が多い場合はゲームセット
                if (score.GetTotal(Core.Order.Second) > score.GetTotal(Core.Order.First))
                {
                    return true;
                }
            }
            else
            {
                // 後攻が先攻より一定数得点が多い場合はゲームセット（コールド）
                int diff = score.GetTotal(Core.Order.Second) - score.GetTotal(Core.Order.First);
                if (diff >= initializer.calledScore)
                {
                    isCalledGame = true;
                    return true;
                }
            }
        }
        else
        {
            // ウラの攻撃
            if (IsLastInning())
            {
                return true;
            }
            else
            {
                // 得点差が一定数より多い場合はゲームセット（コールド）
                int diff = Mathf.Abs(score.GetTotal(Core.Order.Second) - score.GetTotal(Core.Order.First));
                if (diff >= initializer.calledScore)
                {
                    isCalledGame = true;
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// サヨナラが成立した時加算スコアを調整する
    /// </summary>
    /// <param name="homeInCount">ホーム因数</param>
    /// <param name="battingResult">プレー結果</param>
    /// <returns></returns>
    private int AdjustSayonara(int homeInCount, Core.Result battingResult)
    {
        int firstScore = score.GetTotal(Order.First);
        int secondScore = score.GetTotal(Order.Second);

        // 最終イニングの後攻で、
        // 後攻が勝ち越した場合、サヨナラ勝ち
        if (IsLastInning()
            && (currentOrder == Core.Order.Second)
            && (secondScore + homeInCount > firstScore))
        {
            // ホームランの場合はそのまま加算する。
            // ホームラン以外の場合は、先攻の点数を上回った時点でゲームセットとなるので、
            // 先攻スコア+1になるように調整する。
            if (battingResult != Core.Result.HomeRun)
            {
                homeInCount = firstScore - secondScore + 1;
            }
            isSayonara = true;
        }
        // 最終イニング以外で、
        // 後攻がコールドスコア以上の点差をつけた場合、サヨナラ勝ち
        else if ((currentOrder == Core.Order.Second)
            && ((secondScore + homeInCount) - firstScore >= initializer.calledScore))
        {
            // ホームランの場合はそのまま加算する。
            // ホームラン以外の場合は、先攻の点数を上回った時点でゲームセットとなるので、
            // 先攻スコア+1になるように調整する。
            if (battingResult != Core.Result.HomeRun)
            {
                homeInCount = initializer.calledScore - secondScore + firstScore;
            }
            isSayonara = true;
            isCalledGame = true;
        }


        return homeInCount;
    }

    /// <summary>
    /// プレー結果がヒットかどうかをチェックする。
    /// </summary>
    /// <param name="result">プレー結果</param>
    /// <returns>ヒットの場合はtrue、そうでない場合はfalseを返す。</returns>
    private bool IsHit(Core.Result result)
    {
        switch (result)
        {
            case Core.Result.Hit1Base:
            case Core.Result.Hit2Base:
            case Core.Result.Hit3Base:
            case Core.Result.HomeRun:
                return true;
            default:
                return false;
        }
    }
}
