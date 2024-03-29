using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チーム名をランダムで生成する
/// </summary>
public class RandomName
{
    /// <summary>
    /// エリア一覧
    /// </summary>
    private static readonly string[] areas = {
        "江別",
        "千歳",
        "札幌",
        "岩見沢",
        "滝川",
        "深川",
        "小樽",
        "倶知安",
        "函館",
        "長万部",
        "江差",
        "苫小牧",
        "室蘭",
        "浦河",
        "旭川",
        "士別",
        "名寄",
        "留萌",
        "稚内",
        "網走",
        "北見",
        "紋別",
        "帯広",
        "釧路",
        "根室",
        "日高",
        "富良野",
        "枝幸",
        "弟子屈",
        "青森",
        "八戸",
        "弘前",
        "十和田",
        "むつ",
        "五所川原",
        "盛岡",
        "一関",
        "釜石",
        "北上",
        "宮古",
        "久慈",
        "二戸",
        "大船渡",
        "花巻",
        "奥州",
        "仙台",
        "石巻",
        "大崎",
        "気仙沼",
        "白石",
        "秋田",
        "大館",
        "能代",
        "由利本荘",
        "大仙",
        "横手",
        "湯沢",
        "山形",
        "米沢",
        "新庄",
        "酒田",
        "鶴岡",
        "福島",
        "郡山",
        "白河",
        "会津若松",
        "いわき",
        "南相馬",
        "相馬",
        "南会津",
        "鹿嶋",
        "古河",
        "筑西",
        "土浦",
        "日立",
        "水戸",
        "足利",
        "宇都宮",
        "小山",
        "日光",
        "那須塩原",
        "桐生",
        "渋川",
        "高崎",
        "沼田",
        "前橋",
        "草津",
        "さいたま",
        "春日部",
        "川越",
        "熊谷",
        "秩父",
        "草加",
        "所沢",
        "東松山",
        "柏",
        "木更津",
        "千葉",
        "成田",
        "浅草橋",
        "池袋",
        "上野",
        "五反田",
        "新宿",
        "渋谷",
        "品川",
        "巣鴨",
        "東京",
        "日本橋",
        "八王子",
        "厚木",
        "小田原",
        "相模原",
        "横須賀",
        "横浜",
        "川崎",
        "村上",
        "新潟",
        "長岡",
        "上越",
        "糸魚川",
        "南魚沼",
        "三条",
        "十日町",
        "魚津",
        "富山",
        "高岡",
        "砺波",
        "小松",
        "金沢",
        "七尾",
        "輪島",
        "福井",
        "敦賀",
        "大月",
        "甲府",
        "韮崎",
        "富士吉田",
        "身延",
        "飯田",
        "上田",
        "小諸",
        "塩尻",
        "諏訪",
        "長野",
        "松本",
        "岐阜",
        "高山",
        "大垣",
        "美濃加茂",
        "多治見",
        "静岡",
        "浜松",
        "沼津",
        "御前崎",
        "名古屋",
        "豊橋",
        "豊田",
        "津",
        "四日市",
        "伊勢",
        "尾鷲",
        "伊賀",
        "松阪",
        "大津",
        "福知山",
        "舞鶴",
        "京都",
        "京都",
        "堀川五条",
        "大阪",
        "難波",
        "天王寺",
        "大阪港",
        "南港",
        "深江橋",
        "堺",
        "神戸",
        "姫路",
        "豊岡",
        "洲本",
        "三宮",
        "神戸",
        "奈良",
        "大和郡山",
        "天理",
        "橿原",
        "大和高田",
        "五條",
        "和歌山",
        "田辺",
        "新宮",
        "米子",
        "倉吉",
        "鳥取",
        "出雲",
        "益田",
        "大田",
        "松江",
        "浜田",
        "津山",
        "新見",
        "岡山",
        "広島",
        "福山",
        "三次",
        "山口",
        "宇部",
        "周南",
        "岩国",
        "萩",
        "下関",
        "徳島",
        "三好",
        "つるぎ",
        "高松",
        "松山",
        "今治",
        "宇和島",
        "西条",
        "大洲",
        "高知",
        "室戸",
        "四万十",
        "須崎",
        "足摺岬",
        "福岡",
        "久留米",
        "大牟田",
        "飯塚",
        "北九州",
        "唐津",
        "鳥栖",
        "武雄",
        "佐賀",
        "伊万里",
        "佐世保",
        "諫早",
        "島原",
        "大村",
        "長崎",
        "八代",
        "人吉",
        "水俣",
        "天草",
        "宇土",
        "熊本",
        "荒尾",
        "中津",
        "日田",
        "佐伯",
        "大分",
        "宇佐",
        "別府",
        "都城",
        "延岡",
        "日南",
        "小林",
        "宮崎",
        "薩摩川内",
        "鹿屋",
        "枕崎",
        "霧島",
        "鹿児島",
        "辺戸岬",
        "沖縄",
        "那覇",
        "名護",
    };

    /// <summary>
    /// 名前一覧
    /// </summary>
    private static readonly string[] names ={
        "ジラフス",
        "ゼブラズ",
        "ホース",
        "ポニーズ",
        "ライナソース",
        "ヒポポタマス",
        "エレファンツ",
        "ドンキーズ",
        "コアラズ",
        "ディアーズ",
        "ゴリラズ",
        "ジャガーズ",
        "ラビッツ",
        "ウルブズ",
        "カンガルーズ",
        "キャッツ",
        "マウスズ",
        "ラッツ",
        "ハムスターズ",
        "パンダズ",
        "シープス",
        "チキンズ",
        "スカンクス",
        "ゴーツ",
        "スカーラルズ",
        "キャメルズ",
        "スネイクス",
        "サーペンツ",
        "レパーズ",
        "チーターズ",
        "バッツ",
        "リザーズ",
        "ベアズ",
        "モールズ",
        "ラクーンズ",
        "フォックス",
        "リンクス",
        "コヨーテズ",
        "ピッグズ",
        "シールズ",
        "バジャーズ",
        "アルマジロズ",
        "イグアナズ",
        "フロッグス",
        "タートルズ",
        "オッターズ",
        "パンサーズ",
        "パピーズ",
        "キトゥンズ",
        "ラムズ",
        "ウォーラス",
        "スロース",
        "バイソンズ",
        "テイパーズ",
        "ビーバーズ",
        "バブーンズ",
        "マンドリルズ",

        "アバロニズ",
        "スクイーズ",
        "ロブスターズ",
        "サーディンズ",
        "イールズ",
        "シュリンプス",
        "オイスターズ",
        "ボニトーズ",
        "クラブズ",
        "サーモンズ",
        "マカレルズ",
        "シャークス",
        "シーバス",
        "オクトパス",
        "コッズ",
        "フランダース",
        "スカロップス",
        "ツナズ",
        "トラウツ",

        "アルバトロス",
        "オークス",
        "ブービーズ",
        "コックス",
        "ロースターズ",
        "コーモランツ",
        "クロウズ",
        "ドードーズ",
        "ダブズ",
        "ダックス",
        "イーグレッツ",
        "ファルコンズ",
        "フラミンゴス",
        "グース",
        "ガルズ",
        "ヘンズ",
        "ヘロンズ",
        "ジェイス",
        "カイツ",
        "リンプキンズ",
        "リネッツ",
        "マカウズ",
        "ナタッチズ",
        "オウルズ",
        "パロッツ",
        "ペリカンズ",
        "ペンギンズ",
        "パトレルズ",
        "ピジョンズ",
        "プロバーズ",
        "レイブンズ",
        "ロビンズ",
        "シーガルズ",
        "シスキンズ",
        "スパローズ",
        "スワンズ",
        "スウィフツ",
        "ターンズ",
        "ターキーズ",
        "トワイツ",
        "ワーブラーズ",
        "レンズ",
    };

    /// <summary>
    /// エリア名をランダムで取得
    /// </summary>
    /// <returns>エリア名</returns>
    public static string GatArea()
    {
        int index = Random.Range(0, areas.Length);
        return areas[index];
    }

    /// <summary>
    /// チーム名をランダムで取得
    /// </summary>
    /// <returns>チーム名</returns>
    public static string GetName()
    {
        int index = Random.Range(0, names.Length);
        return names[index];
    }
}
