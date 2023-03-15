using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// スコア加算演出
/// </summary>
public class AddScore : MonoBehaviour
{
    /// <summary>
    /// パネルオブジェクト
    /// </summary>
    [SerializeField] private GameObject panelObject;
    /// <summary>
    /// 達成した項目名テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI descr;
    /// <summary>
    /// 加算する値テキスト
    /// </summary>
    [SerializeField] private TextMeshProUGUI score;

    /// <summary>
    /// パネルイメージ
    /// </summary>
    private Image panelImage;
    /// <summary>
    /// イージングリスト
    /// </summary>
    private List<Easing> easeList;

    /// <summary>
    /// 通常レアリティの色
    /// </summary>
    private static readonly Color32 commonColor = new Color32(0x32, 0xCD, 0x32, 0xFF);
    /// <summary>
    /// ハイコモンレアリティの色
    /// </summary>
    private static readonly Color32 highCommonColor = new Color32(0x96, 0x74, 0x44, 0xFF);
    /// <summary>
    /// レアの色
    /// </summary>
    private static readonly Color32 rareColor = new Color32(0xC0, 0xC0, 0xC0, 0xFF);
    /// <summary>
    /// ハイレアの色
    /// </summary>
    private static readonly Color32 highRareColor = new Color32(0xFF, 0xD7, 0x00, 0xFF);

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        panelImage = panelObject.GetComponent<Image>();
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if (IsEndEasing())
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="data">スコアデータ</param>
    public void OnInit(ScoreConst.Data data)
    {
        InitContent(data);
        InitiEasing();
        StartEasing();
    }

    /// <summary>
    /// 内容物の初期化を行う
    /// </summary>
    /// <param name="data">スコアデータ</param>
    private void InitContent(ScoreConst.Data data)
    {
        if (data == null)
        {
            return;
        }

        if (descr != null)
        {
            descr.text = data.descr;
        }
        if (score != null)
        {
            score.text = string.Format("+{0}", data.point);
        }
        if (panelImage != null)
        {
            Color32 color = Color.black;
            switch (data.rarity)
            {
                case ScoreConst.Rarity.Common: color = commonColor; break;
                case ScoreConst.Rarity.HighCommon: color = highCommonColor; break;
                case ScoreConst.Rarity.Rare: color = rareColor; break;
                case ScoreConst.Rarity.HighRare: color = highRareColor; break;
                default: color = commonColor; break;
            }
            panelImage.color = color;
        }
    }

    /// <summary>
    /// イージングの初期化を行う
    /// </summary>
    private void InitiEasing()
    {
        easeList = new List<Easing>();
        Easing[] easings = GetComponents<Easing>();
        foreach (var item in easings)
        {
            item.OnInit();
            easeList.Add(item);
        }
    }

    /// <summary>
    /// イージングを開始する
    /// </summary>
    private void StartEasing()
    {
        foreach (var item in easeList)
        {
            item.StartEasing();
        }
    }

    /// <summary>
    /// イージングが終了したかチェックする
    /// </summary>
    /// <returns>終了した場合はtrue、そうでない場合はfalseを返す。</returns>
    private bool IsEndEasing()
    {
        foreach (var item in easeList)
        {
            if (item.IsInEasing())
            {
                return false;
            }
        }

        return true;
    }
}
