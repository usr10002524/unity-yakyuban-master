using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// スコア管理クラス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// スコア加算オブジェクト
    /// </summary>
    [SerializeField] GameObject addScoreParent;
    /// <summary>
    /// スコア加算パネルのプレファブ
    /// </summary>
    [SerializeField] private GameObject addScorePrefab;

    /// <summary>
    /// スコアデータリスト
    /// </summary>
    private List<ScoreConst.Data> dataList;
    /// <summary>
    /// 加算するスコアデータのゲームオブジェクト
    /// </summary>
    GameObject addScoreObject;

    /// <summary>
    /// 表示中スコア
    /// </summary>
    private int score;
    /// <summary>
    /// 実際のスコア
    /// </summary>
    private int actualScore;

    /// <summary>
    /// シングルトン用インスタンス
    /// </summary>
    public static ScoreManager Instance { get; private set; }

    /// <summary>
    /// ビッグイニング獲得の条件
    /// </summary>
    private static readonly int bigInningThreshold = 5;

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

        dataList = new List<ScoreConst.Data>();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if (addScoreObject == null)
        {
            StartAddScore();
        }
    }

    /// <summary>
    /// スコア加算演出を開始する
    /// </summary>
    private void StartAddScore()
    {
        ScoreConst.Data data = Pop();
        if (data == null)
        {
            return; // キューにつまれていなければ何もしない
        }

        // プレファブからインスタンス化
        addScoreObject = Instantiate(addScorePrefab, addScoreParent.transform);
        // 加算演出を初期化
        AddScore addScore = addScoreObject.GetComponent<AddScore>();
        addScore.OnInit(data);

        // 表示スコアを更新
        int beforeScore = 0;
        int afterScore = 0;
        ApplyInfo(data, ref beforeScore, ref afterScore);
    }

    /// <summary>
    /// スコアデータリストの先頭から1件データを取り出す
    /// </summary>
    /// <returns>スコアデータ</returns>
    private ScoreConst.Data Pop()
    {
        if (dataList.Count > 0)
        {
            ScoreConst.Data data = dataList[0];
            dataList.RemoveAt(0);
            return data;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// スコアデータリストにデータを追加するする
    /// </summary>
    /// <param name="data">スコアデータ</param>
    public void Push(ScoreConst.Data data)
    {
        if (data != null)
        {
            actualScore += data.point;
            dataList.Add(data);
        }
    }

    /// <summary>
    /// 表示スコアを更新する
    /// </summary>
    /// <param name="data">反映させるスコアデータ</param>
    /// <param name="beforeScore">加算前のスコア</param>
    /// <param name="afterScore">加算後のスコア</param>
    private void ApplyInfo(ScoreConst.Data data, ref int beforeScore, ref int afterScore)
    {
        if (data == null)
        {
            return;
        }

        beforeScore = score;
        score += data.point;
        afterScore = score;
    }

    /// <summary>
    /// 現在の表示スコアを取得
    /// </summary>
    /// <returns>表示スコア</returns>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// 現在の実際のスコアを取得
    /// </summary>
    /// <returns>実際のスコア</returns>
    public int GetActualScore()
    {
        return actualScore;
    }

    /// <summary>
    /// バッティング結果のスコアデータを取得する
    /// </summary>
    /// <param name="result">スコアデータ</param>
    /// <returns></returns>
    public ScoreConst.Data MakeBattingResult(Core.Result result)
    {
        switch (result)
        {
            case Core.Result.Hit1Base: return ScoreConst.Get(ScoreConst.Type.Hit1Base);
            case Core.Result.Hit2Base: return ScoreConst.Get(ScoreConst.Type.Hit2Base);
            case Core.Result.Hit3Base: return ScoreConst.Get(ScoreConst.Type.Hit3Base);
            case Core.Result.HomeRun: return ScoreConst.Get(ScoreConst.Type.HomeRun);
            default: return null;
        }
    }

    /// <summary>
    /// ピッチング結果のスコアデータを取得する
    /// </summary>
    /// <param name="result">スコアデータ</param>
    /// <returns></returns>
    public ScoreConst.Data MakePitchingResult(Core.Result result)
    {
        switch (result)
        {
            case Core.Result.Out: return ScoreConst.Get(ScoreConst.Type.Out);
            default: return null;
        }
    }

    /// <summary>
    /// 奪三振のスコアデータを取得する
    /// </summary>
    /// <returns>スコアデータ</returns>
    public ScoreConst.Data MakeStruckOut()
    {
        return ScoreConst.Get(ScoreConst.Type.StruckOut);
    }

    /// <summary>
    /// サヨナラ勝ちのスコアデータを取得する
    /// </summary>
    /// <returns>スコアデータ</returns>
    public ScoreConst.Data MakeSayonara()
    {
        return ScoreConst.Get(ScoreConst.Type.Sayonara);
    }

    /// <summary>
    /// 完封勝ちのスコアデータを取得する
    /// </summary>
    /// <returns>スコアデータ</returns>
    public ScoreConst.Data MakeShutOut()
    {
        return ScoreConst.Get(ScoreConst.Type.Shutout);
    }

    /// <summary>
    /// ノーヒットノーランのスコアデータを取得する
    /// </summary>
    /// <returns>スコアデータ</returns>
    public ScoreConst.Data MakeNoNo()
    {
        return ScoreConst.Get(ScoreConst.Type.NoNo);
    }

    /// <summary>
    /// 完全試合のスコアデータを取得する
    /// </summary>
    /// <returns>スコアデータ</returns>
    public ScoreConst.Data MakePerfectGame()
    {
        return ScoreConst.Get(ScoreConst.Type.PerfectGame);
    }

    /// <summary>
    /// 得失点差のスコアデータを取得する
    /// </summary>
    /// <returns>スコアデータ</returns>
    public ScoreConst.Data MakeWinScoreDiff(int diff)
    {
        if (diff > 0)
        {
            ScoreConst.Data data = ScoreConst.Get(ScoreConst.Type.DiffRate);
            ScoreConst.Data ret = new ScoreConst.Data(data.type, data.descr, data.rarity, data.point * diff);
            return ret;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// イニング中のシチュエーションに関するスコアデータを取得する
    /// </summary>
    /// <returns>スコアデータ</returns>
    public ScoreConst.Data MakeInningSituation(int current, int inningStart, int playBefore, int other)
    {
        if (playBefore == 0 && other == 0 && current > 0)
        {
            // 先取点
            return ScoreConst.Get(ScoreConst.Type.FastScore);
        }
        else if (playBefore <= other && other > 0 && current > other)
        {
            // 勝ち越し
            return ScoreConst.Get(ScoreConst.Type.OverScore);
        }
        else if (current - inningStart >= bigInningThreshold)
        {
            // ビッグイニング
            return ScoreConst.Get(ScoreConst.Type.BigInning);
        }
        else
        {
            return null;
        }
    }
}
