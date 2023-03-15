using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レコード管理クラス
/// </summary>
public class RecordManager : MonoBehaviour
{
    /// <summary>
    /// 1イニング上位レコード
    /// </summary>
    private List<RecordData> inning1;
    /// <summary>
    /// 3イニング上位レコード
    /// </summary>
    private List<RecordData> inning3;
    /// <summary>
    /// 5イニング上位レコード
    /// </summary>
    private List<RecordData> inning5;
    /// <summary>
    /// 7イニング上位レコード
    /// </summary>
    private List<RecordData> inning7;
    /// <summary>
    /// 9イニング上位レコード
    /// </summary>
    private List<RecordData> inning9;

    /// <summary>
    /// シングルトン用インスタンス
    /// </summary>
    public static RecordManager Instance { get; private set; }

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
        DontDestroyOnLoad(gameObject);

        OnInit();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void OnInit()
    {
        inning1 = new List<RecordData>();
        inning3 = new List<RecordData>();
        inning5 = new List<RecordData>();
        inning7 = new List<RecordData>();
        inning9 = new List<RecordData>();
    }

    /// <summary>
    /// 指定したイニングのレコードリストを取得する
    /// </summary>
    /// <param name="inning">イニング</param>
    /// <returns>レコードリスト</returns>
    public List<RecordData> Get(int inning)
    {
        switch (inning)
        {
            case 1: return inning1;
            case 3: return inning3;
            case 5: return inning5;
            case 7: return inning7;
            case 9: return inning9;
            default: return null;
        }
    }

    /// <summary>
    /// 指定したイニングのレコードを追加する
    /// </summary>
    /// <param name="inning">イニング</param>
    /// <param name="recordData">レコード</param>
    public void AddData(int inning, RecordData recordData)
    {
        switch (inning)
        {
            case 1: AddData(ref inning1, recordData); break;
            case 3: AddData(ref inning3, recordData); break;
            case 5: AddData(ref inning5, recordData); break;
            case 7: AddData(ref inning7, recordData); break;
            case 9: AddData(ref inning9, recordData); break;
        }
    }

    /// <summary>
    /// レコードリストにレコードを追加する
    /// </summary>
    /// <param name="dst">対象のレコードリスト</param>
    /// <param name="recordData">追加するデータ</param>
    private void AddData(ref List<RecordData> dst, RecordData recordData)
    {
        // レコードを追加しソートする
        dst.Add(recordData);
        dst.Sort();

        // 最大件数を超えるようであれば下位のデータを削除する
        if (dst.Count > BestRecords.maxRecordCount)
        {
            int delCount = dst.Count - BestRecords.maxRecordCount;
            dst.RemoveRange(BestRecords.maxRecordCount, delCount);
        }
    }

    /// <summary>
    /// 自己ベストレコードクラスから復元する
    /// </summary>
    /// <param name="myRecords">自己ベストレコードクラス</param>
    public void Restore(MyRecords myRecords)
    {
        if (myRecords != null)
        {
            DataFill(ref inning1, myRecords.inning1);
            DataFill(ref inning3, myRecords.inning3);
            DataFill(ref inning5, myRecords.inning5);
            DataFill(ref inning7, myRecords.inning7);
            DataFill(ref inning9, myRecords.inning9);
        }
    }

    /// <summary>
    /// ベストレコードクラスからレコードリストにデータを追加する
    /// </summary>
    /// <param name="dst">レコードリスト</param>
    /// <param name="src">ベストレコードクラス</param>
    private static void DataFill(ref List<RecordData> dst, BestRecords src)
    {
        if (dst == null)
        {
            return;
        }
        if (src == null)
        {
            return;
        }

        dst.Clear();
        foreach (var item in src.bestRecords)
        {
            RecordData recordData = new RecordData(item);
            dst.Add(recordData);
        }
        dst.Sort();
    }

    /// <summary>
    /// MyRecordクラスを作成する
    /// </summary>
    /// <returns>MyRecordクラス</returns>
    public MyRecords Create()
    {
        MyRecords myRecords = new MyRecords();

        DataFill(ref myRecords.inning1, inning1);
        DataFill(ref myRecords.inning3, inning3);
        DataFill(ref myRecords.inning5, inning5);
        DataFill(ref myRecords.inning7, inning7);
        DataFill(ref myRecords.inning9, inning9);

        return myRecords;
    }

    /// <summary>
    /// レコードリストからベストレコードクラスにデータを追加する
    /// </summary>
    /// <param name="dst">ベストレコードクラス</param>
    /// <param name="src">レコードリスト</param>
    private static void DataFill(ref BestRecords dst, List<RecordData> src)
    {
        if (dst == null)
        {
            return;
        }
        if (src == null)
        {
            return;
        }

        dst.bestRecords = new RecordData[src.Count];
        for (int i = 0; i < src.Count; i++)
        {
            RecordData recordData = new RecordData(src[i]);
            dst.bestRecords[i] = recordData;
        }

    }

    /// <summary>
    /// ログに出力する
    /// </summary>
    public void Logging()
    {
        Debug.Log("-- inning1 --");
        foreach (var item in inning1)
        {
            item.Logging();
        }
        Debug.Log("-- inning3 --");
        foreach (var item in inning3)
        {
            item.Logging();
        }
        Debug.Log("-- inning5 --");
        foreach (var item in inning5)
        {
            item.Logging();
        }
        Debug.Log("-- inning7 --");
        foreach (var item in inning7)
        {
            item.Logging();
        }
        Debug.Log("-- inning9 --");
        foreach (var item in inning9)
        {
            item.Logging();
        }
        Debug.Log("------------");

    }
}
