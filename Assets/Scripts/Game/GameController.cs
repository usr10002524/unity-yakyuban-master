using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム進行制御クラス
/// </summary>
public partial class GameController : MonoBehaviour
{
    /// <summary>
    /// ステップ
    /// </summary>
    private enum Step
    {
        INIT,

        START_PLAYBALL, // ゲーム開始の開始
        UPDATE_PLAYBALL,    // ゲーム開始の終了まち

        START_INNING,   // イニングの初期化

        START_PLAY,     // １プレーの初期化
        UPDATE_PLAY,    // １プレーの終了まち

        START_RESULT,   // １プレーの結果判定
        UPDATE_RESULT,  // １プレーの結果終了まち

        READY_NEXT,     // 次のプレーの準備

        START_GAMESET,  // ゲーム終了開始
        UPDATE_GAMESET, // ゲーム終了の終了まち

        END,
    }

    /// <summary>
    /// イニング数
    /// </summary>
    [SerializeField] private int innings = 9;
    /// <summary>
    /// 先攻or後攻
    /// </summary>
    [SerializeField] private Core.Order order = Core.Order.First;
    /// <summary>
    /// コールドゲームになるスコア
    /// </summary>
    [SerializeField] private int calledScore = 10;

    /// <summary>
    /// バッターのゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject batterObject;
    /// <summary>
    /// ピッチャーのゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject pitcherObject;
    /// <summary>
    /// ボールのゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject ballObject;
    /// <summary>
    /// カウント情報のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject infoUIObject;
    /// <summary>
    /// カメラのゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject cameraObject;
    /// <summary>
    /// カットイン表示のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject cutinUIObject;
    /// <summary>
    /// 結果表示のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject resultUIObject;
    /// <summary>
    /// スコア表示のゲームオブジェクト
    /// </summary>
    [SerializeField] private GameObject scoreBoardUIObject;

    /// <summary>
    /// 現在のステップ
    /// </summary>
    [SerializeField] private Step step;
    /// <summary>
    /// バッター制御クラス
    /// </summary>
    private BatterController batterController;
    /// <summary>
    /// ピッチャー制御クラス
    /// </summary>
    private PitcherController pitcherController;
    /// <summary>
    /// ボール制御クラス
    /// </summary>
    private BallController ballController;
    /// <summary>
    /// カウント情報制御クラス
    /// </summary>
    private InfomationUIController infoUIController;
    /// <summary>
    /// カメラ制御クラス
    /// </summary>
    private CameraController cameraController;
    /// <summary>
    /// カットイン制御クラス
    /// </summary>
    private CutinController cutinController;
    /// <summary>
    /// 結果表示制御クラス
    /// </summary>
    private ResultController resultController;
    /// <summary>
    /// ゲームオーディオ制御クラス
    /// </summary>
    private GameAudioController gameAudioController;
    /// <summary>
    /// ゲームスコア制御クラス
    /// </summary>
    private GameScoreController gameScoreController;
    /// <summary>
    /// スコアボード制御クラス
    /// </summary>
    private ScoreBoardController scoreBoardController;

    /// <summary>
    /// プレー結果
    /// </summary>
    private Core.Result result;
    /// <summary>
    /// 先攻チームの統計情報
    /// </summary>
    private GameDiag firstDiag;
    /// <summary>
    /// 後攻チームの統計情報
    /// </summary>
    private GameDiag secondDiag;

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        firstDiag = new GameDiag();
        secondDiag = new GameDiag();
    }


    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        // GameManagerを初期化
        GameManager.Initializer initializer = new GameManager.Initializer();
        if (GameSettings.Instance.IsValid())
        {
            initializer.innings = GameSettings.Instance.innings;
            initializer.order = GameSettings.Instance.order;
            initializer.calledScore = GameSettings.Instance.calledScore;
            initializer.firstTeamName = GameSettings.Instance.firstTeamName;
            initializer.secondTeamName = GameSettings.Instance.secondTeamName;
        }
        else
        {
            // ゲーム設定が未設定の場合はエディタでセットされているものを使用する
            initializer.innings = innings;
            initializer.order = order;
            initializer.calledScore = calledScore;

            TeamName teamName = new TeamName();
            teamName.SetFirstTeam();
            teamName.SetSecondTeam();
            initializer.firstTeamName = teamName.GetFirstTeam();
            initializer.secondTeamName = teamName.GetSecondTeam();

        }

        GameManager.Instance.Initialize(initializer);

        // 各種制御クラスを取得し初期化を行う
        batterController = batterObject.GetComponent<BatterController>();
        pitcherController = pitcherObject.GetComponent<PitcherController>();
        ballController = ballObject.GetComponent<BallController>();
        infoUIController = infoUIObject.GetComponent<InfomationUIController>();
        infoUIController.OnInit();
        cameraController = cameraObject.GetComponent<CameraController>();
        cameraController.SetCamera(CameraController.CameraType.FieldingCamera);
        cutinController = cutinUIObject.GetComponent<CutinController>();
        cutinController.OnInit();
        resultController = resultUIObject.GetComponent<ResultController>();
        resultController.OnInit();
        gameAudioController = GetComponent<GameAudioController>();
        gameScoreController = GetComponent<GameScoreController>();
        scoreBoardController = scoreBoardUIObject.GetComponent<ScoreBoardController>();
        scoreBoardController.OnInit();

        // 最初のステップを設定
        step = Step.INIT;
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        switch (step)
        {
            case Step.INIT:
                StepInit();
                break;

            case Step.START_PLAYBALL: // ゲーム開始の開始
                StepStartPlayBall();
                break;
            case Step.UPDATE_PLAYBALL:    // ゲーム開始の終了まち
                StepUpdatePlayBall();
                break;

            case Step.START_INNING:   // イニングの初期化
                StepStartInning();
                break;

            case Step.START_PLAY:     // １プレーの初期化
                StepStartPlay();
                break;
            case Step.UPDATE_PLAY:    // １プレーの終了まち
                StepUpdatePlay();
                break;

            case Step.START_RESULT:   // １プレーの結果判定
                StepStartResult();
                break;
            case Step.UPDATE_RESULT:  // １プレーの結果終了まち
                StepUpdateResult();
                break;

            case Step.READY_NEXT:     // 次のプレーの準備
                StepReadyNext();
                break;

            case Step.START_GAMESET:  // ゲーム終了開始
                StepStartGameSet();
                break;
            case Step.UPDATE_GAMESET: // ゲーム終了の終了まち
                StepUpdateGameSet();
                break;

            case Step.END:
                StepEnd();
                break;
        }
    }

    /// <summary>
    /// INITステップの処理
    /// </summary>
    private void StepInit()
    {
        ChangeStep(Step.START_PLAYBALL);
    }

    /// <summary>
    /// START_PLAYBALLステップの処理
    /// </summary>
    private void StepStartPlayBall()
    {
        cutinController.StartCutin(CutinExtra.ExtraType.PlayBall);
        ChangeStep(Step.UPDATE_PLAYBALL);
    }

    /// <summary>
    /// UPDATE_PLAYBALLステップの処理
    /// </summary>
    private void StepUpdatePlayBall()
    {
        if (!cutinController.IsEndCutin())
        {
            return;
        }
        ChangeStep(Step.START_INNING);
    }

    /// <summary>
    /// START_INNINGステップの処理
    /// </summary>
    private void StepStartInning()
    {
        gameAudioController.StartInning();
        gameScoreController.StartInning();
        ChangeStep(Step.START_PLAY);
    }

    /// <summary>
    /// START_PLAYステップの処理
    /// </summary>
    private void StepStartPlay()
    {
        // 各種プレー開始前の準備をおこなう
        batterController.OnReady(GameManager.Instance.IsBatting());
        pitcherController.OnReady(GameManager.Instance.IsFielding());
        cameraController.OnReady(GameManager.Instance.IsBatting());
        gameAudioController.OnReady();

        ChangeStep(Step.UPDATE_PLAY);
    }

    /// <summary>
    /// UPDATE_PLAYステップの処理
    /// </summary>
    private void StepUpdatePlay()
    {
        // ボールがバットに当たったらカメラを切り替える
        if (ballController.IsHitted())
        {
            cameraController.SetCamera(CameraController.CameraType.FieldingCamera);
        }

        // 結果が確定するまで待つ
        result = ballController.GetResult();

        if (result != Core.Result.None)
        {
            if (result == Core.Result.Ball)
            {
                if (batterController.IsSwinged())
                {
                    result = Core.Result.Strike;
                }
            }

            SetPlayResult(result);

            ChangeStep(Step.START_RESULT);
        }
    }

    /// <summary>
    /// START_RESULTステップの処理
    /// </summary>
    private void StepStartResult()
    {
        // 結果に応じた処理を行う
        if (GameManager.Instance.IsGameSet())
        {
            // ゲームセット
            gameAudioController.GameSet();
            // カットインを表示
            cutinController.StartCutin(Core.Result.GameSet);
        }
        else if (GameManager.Instance.IsChange())
        {
            // チェンジ
            gameAudioController.Change();
            // カットインを表示
            cutinController.StartCutin(Core.Result.Change);

            // スコアボードを表示
            scoreBoardController.OnRedraw();
            scoreBoardController.Show(true);

            // カウント情報は非表示
            infoUIController.Show(false);
        }
        else
        {
            // イニング中
            if (GameManager.Instance.IsStruckOut())
            {
                // 三振
                gameAudioController.StartResult(Core.Result.Out);
                // カットインを表示
                cutinController.StartCutin(Core.Result.Out);
            }
            else
            {
                // その他
                gameAudioController.StartResult(result);
                // カットインを表示
                cutinController.StartCutin(result);
            }

        }

        ChangeStep(Step.UPDATE_RESULT);
    }

    /// <summary>
    /// UPDATE_RESULTステップの処理
    /// </summary>
    private void StepUpdateResult()
    {
        // カットインを表示中であれば終了まで待つ
        if (!cutinController.IsEndCutin())
        {
            return;
        }

        ChangeStep(Step.READY_NEXT);
    }

    /// <summary>
    /// READY_NEXTステップの処理
    /// </summary>
    private void StepReadyNext()
    {
        // 現在の状況に応じて遷移先を分岐させる
        if (GameManager.Instance.IsGameSet())
        {
            // ゲームセット
            ChangeStep(Step.START_GAMESET);
        }
        else
        {
            if (GameManager.Instance.IsChange())
            {
                // チェンジ
                // スコアボードを非表示にする
                scoreBoardController.Show(false);
                // カウント情報を表示
                infoUIController.Show(true);

                ChangeStep(Step.START_INNING);
            }
            else
            {
                ChangeStep(Step.START_PLAY);
            }
        }
    }

    /// <summary>
    /// START_GAMESETステップの処理
    /// </summary>
    private void StepStartGameSet()
    {
        // カウント情報を非表示
        infoUIController.Show(false);
        // 結果を表示
        resultController.OnRedraw();
        resultController.Show(true);
        // スコアボードを表示
        scoreBoardController.OnRedraw();
        scoreBoardController.Show(true);

        // プレーヤーの結果に応じてカットインを表示する
        if (GameManager.Instance.IsWin())
        {
            cutinController.StartCutin(CutinExtra.ExtraType.Win);
        }
        else if (GameManager.Instance.IsLose())
        {
            cutinController.StartCutin(CutinExtra.ExtraType.Lose);
        }
        else if (GameManager.Instance.IsDraw())
        {
            cutinController.StartCutin(CutinExtra.ExtraType.Draw);
        }

        // データを保存
        SaveData();

        ChangeStep(Step.UPDATE_GAMESET);
    }

    /// <summary>
    /// UPDATE_GAMESETステップの処理
    /// </summary>
    private void StepUpdateGameSet()
    {
        // カットインを表示中であれば終了まで待つ
        if (!cutinController.IsEndCutin())
        {
            return;
        }
        // タイトルに戻るボタンを表示
        resultController.ShowReturnTitleButton();

        // スコアボードを表示
        ShowScoreBoard();

        ChangeStep(Step.END);
    }

    /// <summary>
    /// ENDステップの処理
    /// </summary>
    private void StepEnd()
    {
        ;
    }

    /// <summary>
    /// プレー結果を反映させる
    /// </summary>
    /// <param name="result">プレー結果</param>
    private void SetPlayResult(Core.Result result)
    {
        // UpdateInning を呼ぶと内部的にイニングが進むので、
        // 先にプレーヤーが攻守どちらかを確認しておく。
        Core.Order order = GameManager.Instance.GetCurrentOrder();
        bool isBatting = GameManager.Instance.IsBatting();

        // イニング開始時、プレー反映前、相手チームのスコアを予め保持しておく
        Score score = GameManager.Instance.GetScore();
        int inning = GameManager.Instance.GetCurrentInning();
        int inningStartScore = (order == Core.Order.First) ?
                                    score.GetInningStart(inning, Core.Order.First) :
                                    score.GetInningStart(inning, Core.Order.Second);
        int playBeforeScore = (order == Core.Order.First) ?
                                    score.GetTotal(Core.Order.First) :
                                    score.GetTotal(Core.Order.Second);
        int otherScore = (order == Core.Order.First) ?
                                    score.GetTotal(Core.Order.Second) :
                                    score.GetTotal(Core.Order.First);


        // コアの更新
        GameManager.Instance.UpdateInning(result);

        // Diag を更新
        bool isFirst = (order == Core.Order.First);
        bool isStruckOut = GameManager.Instance.IsStruckOut();
        bool isFourBall = GameManager.Instance.IsFourBall();

        firstDiag.ApplyResult(isFirst, result, isStruckOut, isFourBall);
        secondDiag.ApplyResult(!isFirst, result, isStruckOut, isFourBall);

        // スコアを更新
        gameScoreController.SetPlayResult(isBatting, result, isStruckOut);
        if (isBatting)
        {
            // 現在のシチュエーションに対するスコアを更新。こちらは攻撃時のみ
            int currentScore = (order == Core.Order.First) ?
                                    score.GetTotal(Core.Order.First) :
                                    score.GetTotal(Core.Order.Second);
            gameScoreController.SetInningSituation(currentScore, inningStartScore, playBeforeScore, otherScore);
        }

        if (GameManager.Instance.IsGameSet())
        {
            GameDiag own = (GameManager.Instance.GetOrder() == Core.Order.First) ? firstDiag : secondDiag;
            GameDiag other = (GameManager.Instance.GetOrder() == Core.Order.First) ? secondDiag : firstDiag;
            gameScoreController.SetGameResult(own, other);
        }

        infoUIController.OnRedraw();
    }

    /// <summary>
    /// ステップの変更を行う
    /// </summary>
    /// <param name="nextStep">変更先のステップ</param>
    private void ChangeStep(Step nextStep)
    {
        if (step != nextStep)
        {
            // Debug.Log(string.Format("ChangeStep() {0} -> {1}", step, nextStep));
            step = nextStep;
        }
    }


    /// <summary>
    /// プレーデータの保存を行う
    /// </summary>
    private void SaveData()
    {
#if false
        if (!AtsumaruAPI.Instance.IsValid())
        {
            return; // AtumaruAPIが無効であれば何もしない
        }

        int inning = GameManager.Instance.GetInnings();
        int score = ScoreManager.Instance.GetActualScore();

        // スコアを保存
        {
            AtsumaruAPI.Instance.SaveScore(inning, score);
        }

        // 自己ベストを記録し、保存
        {
            GameManager gameManager = GameManager.Instance;

            string ownTeamName = (gameManager.GetOrder() == Core.Order.First) ?
                gameManager.GetFirstTeamName() : gameManager.GetSecondTeamName();
            string otherTeamName = (gameManager.GetOrder() == Core.Order.First) ?
                gameManager.GetSecondTeamName() : gameManager.GetFirstTeamName();

            long unixTime = EpochTime.ToUnixTime(DateTime.Now);

            // 自己ベストを更新
            RecordData recordData = new RecordData(ownTeamName, otherTeamName, score, unixTime);
            RecordManager.Instance.AddData(inning, recordData);

            MyRecords myRecords = RecordManager.Instance.Create();

            AtsumaruAPI.ServerDataItems serverDataItems = new AtsumaruAPI.ServerDataItems();
            serverDataItems.data = new AtsumaruAPI.DataItem[1];

            AtsumaruAPI.DataItem item = new AtsumaruAPI.DataItem();
            item.key = MyRecords.key;
            item.value = JsonUtility.ToJson(myRecords);
            // Debug.Log(string.Format("{0}", item.value));
            serverDataItems.data[0] = item;

            string json = JsonUtility.ToJson(serverDataItems);
            // Debug.Log(string.Format("{0}", json));
            AtsumaruAPI.Instance.SaveServerData(json);
        }
#else
        if (AtsumaruAPI.Instance.IsValid())
        {
            SaveWithAtsumaruAPI();
        }
        else
        {
            SaveWithNoAtsumaruAPI();
        }
#endif
    }

    /// <summary>
    /// AtumaruAPIを使用してデータを保存する
    /// </summary>
    private void SaveWithAtsumaruAPI()
    {
        int inning = GameManager.Instance.GetInnings();
        int score = ScoreManager.Instance.GetActualScore();

        // スコアを保存
        {
            AtsumaruAPI.Instance.SaveScore(inning, score);
        }

        // 自己ベストを記録し、保存
        {
            GameManager gameManager = GameManager.Instance;

            string ownTeamName = (gameManager.GetOrder() == Core.Order.First) ?
                gameManager.GetFirstTeamName() : gameManager.GetSecondTeamName();
            string otherTeamName = (gameManager.GetOrder() == Core.Order.First) ?
                gameManager.GetSecondTeamName() : gameManager.GetFirstTeamName();

            long unixTime = EpochTime.ToUnixTime(DateTime.Now);

            // 自己ベストを更新
            RecordData recordData = new RecordData(ownTeamName, otherTeamName, score, unixTime);
            RecordManager.Instance.AddData(inning, recordData);

            MyRecords myRecords = RecordManager.Instance.Create();

            AtsumaruAPI.ServerDataItems serverDataItems = new AtsumaruAPI.ServerDataItems();
            serverDataItems.data = new AtsumaruAPI.DataItem[1];

            AtsumaruAPI.DataItem item = new AtsumaruAPI.DataItem();
            item.key = MyRecords.key;
            item.value = JsonUtility.ToJson(myRecords);
            // Debug.Log(string.Format("{0}", item.value));
            serverDataItems.data[0] = item;

            string json = JsonUtility.ToJson(serverDataItems);
            // Debug.Log(string.Format("{0}", json));
            AtsumaruAPI.Instance.SaveServerData(json);
        }
    }

    /// <summary>
    /// AtsumaruAPIを使用せずデータを保存する
    /// </summary>
    private void SaveWithNoAtsumaruAPI()
    {
        int inning = GameManager.Instance.GetInnings();
        int score = ScoreManager.Instance.GetActualScore();

        // スコアを保存
        {
            // AtsumaruAPI.Instance.SaveScore(inning, score);
        }

        // 自己ベストを記録し、保存
        {
            GameManager gameManager = GameManager.Instance;

            string ownTeamName = (gameManager.GetOrder() == Core.Order.First) ?
                gameManager.GetFirstTeamName() : gameManager.GetSecondTeamName();
            string otherTeamName = (gameManager.GetOrder() == Core.Order.First) ?
                gameManager.GetSecondTeamName() : gameManager.GetFirstTeamName();

            long unixTime = EpochTime.ToUnixTime(DateTime.Now);

            // 自己ベストを更新
            RecordData recordData = new RecordData(ownTeamName, otherTeamName, score, unixTime);
            RecordManager.Instance.AddData(inning, recordData);

            MyRecords myRecords = RecordManager.Instance.Create();

            AtsumaruAPI.ServerDataItems serverDataItems = new AtsumaruAPI.ServerDataItems();
            serverDataItems.data = new AtsumaruAPI.DataItem[1];

            AtsumaruAPI.DataItem item = new AtsumaruAPI.DataItem();
            item.key = MyRecords.key;
            item.value = JsonUtility.ToJson(myRecords);
            // Debug.Log(string.Format("{0}", item.value));
            serverDataItems.data[0] = item;

            string json = JsonUtility.ToJson(serverDataItems);
            // Debug.Log(string.Format("{0}", json));
            // AtsumaruAPI.Instance.SaveServerData(json);
            LocalStorageAPI.Instance.SaveLocalData(json);
        }
    }

    /// <summary>
    /// ランキングボードを表示する
    /// </summary>
    private void ShowScoreBoard()
    {
        if (!AtsumaruAPI.Instance.IsValid())
        {
            return;
        }

        int inning = GameManager.Instance.GetInnings();
        AtsumaruAPI.Instance.DisplayScoreBoard(inning);
    }
}
