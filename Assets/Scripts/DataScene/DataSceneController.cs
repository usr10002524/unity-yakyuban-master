using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataSceneController : MonoBehaviour
{
    /// <summary>
    /// 順位ごとのデータコンテナオブジェクト
    /// </summary>
    [SerializeField] List<GameObject> dataContaienrObjects;

    /// <summary>
    /// データコンテナマップ
    /// </summary>
    private Dictionary<DataContainer.Rank, DataContainer> dataContainerMap;

    /// <summary>
    /// 表示データを変更する。
    /// </summary>
    /// <param name="inning">表示するランキングのタイプ</param>
    public void ChangeData(int inning)
    {
        SetData(inning);
    }

    /// <summary>
    /// タイトルに戻る。
    /// </summary>
    public void ReturnTitle()
    {
        Initiate.Fade("TitleScene", FadeConst.sceneTransitColor, FadeConst.fadeDump, FadeConst.sceneTransitSortOrder);
    }

    /// <summary>
    /// テストシーンに移動する。
    /// </summary>
    public void TransitTestScene()
    {
        SceneManager.LoadScene("TestScene");
    }


    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        dataContainerMap = new Dictionary<DataContainer.Rank, DataContainer>();
        Initialize();
        SetData(1);
    }

    /// <summary>
    /// 初期化を行う
    /// </summary>
    private void Initialize()
    {
        dataContainerMap = new Dictionary<DataContainer.Rank, DataContainer>();

        foreach (var item in dataContaienrObjects)
        {
            DataContainer dataContainer = item.GetComponent<DataContainer>();
            if (dataContainer != null)
            {
                dataContainerMap.Add(dataContainer.GetRank(), dataContainer);
            }
        }
    }

    /// <summary>
    /// データコンテナオブジェクトにデータをセットする
    /// </summary>
    /// <param name="inning">セットするランキングのタイプ</param>
    private void SetData(int inning)
    {
        List<RecordData> records = RecordManager.Instance.Get(inning);
        if (records == null)
        {
            //データがければ非表示
            foreach (var item in dataContainerMap)
            {
                item.Value.OnInit(false, 0, 0, "", "");
            }
        }
        else
        {
            int index = 0;
            int lastRank = 1;
            int lastScore = 0;

            foreach (var item in dataContainerMap)
            {
                if (index < records.Count)
                {
                    int score = records[index].score;
                    int rank = (lastScore == score) ? lastRank : (index + 1);   //同率なら同じ順位にする
                    string ownTeam = records[index].ownTeamName;
                    string otherTeam = records[index].otherTeamName;
                    item.Value.OnInit(true, rank, score, ownTeam, otherTeam);

                    index++;
                    lastRank = rank;
                    lastScore = score;
                }
                else
                {
                    item.Value.OnInit(false, 0, 0, "", "");
                }
            }
        }
    }
}
