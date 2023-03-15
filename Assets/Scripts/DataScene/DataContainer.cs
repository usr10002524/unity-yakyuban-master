using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ランキング1データあたりのコンテナ
/// </summary>
public class DataContainer : MonoBehaviour
{
    /// <summary>
    /// 順位定義
    /// </summary>
    public enum Rank
    {
        Rank1,
        Rank2,
        Rank3,
        Rank4,
        Rank5,
    }

    // 順位
    [SerializeField] private Rank rank;
    // コンテナゲームオブジェクト
    [SerializeField] private GameObject dataContainerObject;
    // 順位テキスト
    [SerializeField] private TextMeshProUGUI textRank;
    // スコアテキスト
    [SerializeField] private TextMeshProUGUI textScore;
    // 自チーム名テキスト
    [SerializeField] private TextMeshProUGUI textOwnTeam;
    // 相手チーム名テキスト
    [SerializeField] private TextMeshProUGUI textOtherTeam;
    // パネルオブジェクト
    [SerializeField] private GameObject panelObject;


    /// <summary>
    /// 順位を取得する。
    /// </summary>
    /// <returns>順位</returns>
    public Rank GetRank()
    {
        return rank;
    }

    /// <summary>
    /// データコンテナを初期化する
    /// </summary>
    /// <param name="visible">表示するかどうか。非表示の場合、以下のパラメータはセットされません。</param>
    /// <param name="rank">順位</param>
    /// <param name="score">スコア</param>
    /// <param name="ownTeam">自チーム名</param>
    /// <param name="otherTeam">相手チーム名</param>
    public void OnInit(bool visible, int rank, int score, string ownTeam, string otherTeam)
    {
        if (visible)
        {
            dataContainerObject.SetActive(true);

            textRank.text = string.Format("{0}", rank);
            textScore.text = string.Format("{0}", score);
            textOwnTeam.text = string.Format("{0}", ownTeam);
            textOtherTeam.text = string.Format("{0}", otherTeam);

            Image image = panelObject.GetComponent<Image>();
            if (image != null)
            {
                image.color = GetRankColor(rank);
            }

            // Debug.Log(string.Format("DataContainer.OnInit() rank:{0} score:{1} ownTeam:{2} otherTeam:{3}", rank, score, ownTeam, otherTeam));
        }
        else
        {
            if (dataContainerObject != null)
            {
                dataContainerObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 順位に対応したパネルの色を取得する。
    /// </summary>
    /// <param name="rank">順位</param>
    /// <returns>色</returns>
    private Color32 GetRankColor(int rank)
    {
        switch (rank)
        {
            case 1: return new Color32(0xff, 0xd7, 0x00, 0xff);
            case 2: return new Color32(0xc0, 0xc0, 0xc0, 0xff);
            case 3: return new Color32(0xac, 0x6b, 0x25, 0xff);
            default: return new Color32(0xff, 0xff, 0xff, 0xff);
        }
    }
}
