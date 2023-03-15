using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレーヤーデータ管理クラス
/// </summary>
public class PlayerDataManager : MonoBehaviour
{
    /// <summary>
    /// シングルトン用インスタンス
    /// </summary>
    public static PlayerDataManager Instance { get; private set; }

    /// <summary>
    /// レコードクラス
    /// </summary>
    private MyRecords myRecords;

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
    /// 指定されたjsonからデータを復元する
    /// </summary>
    /// <param name="json">jsonデータ</param>
    public void ParseJson(string json)
    {
        AtsumaruAPI.ServerDataItems serverDataItems = JsonUtility.FromJson<AtsumaruAPI.ServerDataItems>(json);

        foreach (var item in serverDataItems.data)
        {
            string key = item.key;
            string value = item.value;

            if (item.key == MyRecords.key)
            {
                ParseMyRecored(value);
            }
        }
    }

    /// <summary>
    /// レコードを取得する
    /// </summary>
    /// <returns>レコード</returns>
    public MyRecords GetMyRecords()
    {
        return myRecords;
    }

    /// <summary>
    /// 指定されたjsonからレコードを復元する
    /// </summary>
    /// <param name="json">jsonデータ</param>
    private void ParseMyRecored(string json)
    {
        myRecords = JsonUtility.FromJson<MyRecords>(json);
    }

}
