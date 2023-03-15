using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チーム名管理クラス
/// </summary>
public class TeamName
{
    // 先攻チームの地域名
    private string firstAreaName;
    // 先攻チームのチーム名
    private string firstTeamName;
    // 後攻チームの地域名
    private string secondAreaName;
    // 後攻チームのチーム名
    private string secondTeamName;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TeamName()
    {
        firstAreaName = "";
        firstTeamName = "";
        secondAreaName = "";
        secondTeamName = "";
    }

    /// <summary>
    /// 先攻チーム名を設定する。
    /// </summary>
    public void SetFirstTeam()
    {
        firstAreaName = GetAreaName(secondAreaName);
        firstTeamName = GetTeamName(secondTeamName);
    }

    /// <summary>
    /// 後攻チーム名を設定する。
    /// </summary>
    public void SetSecondTeam()
    {
        secondAreaName = GetAreaName(firstAreaName);
        secondTeamName = GetTeamName(firstTeamName);
    }

    /// <summary>
    /// 先攻チーム名を取得する。
    /// </summary>
    /// <returns>先攻チーム名</returns>
    public string GetFirstTeam()
    {
        string ret = firstAreaName + "\n" + firstTeamName;
        return ret;
    }

    /// <summary>
    /// 後攻チーム名を取得する。
    /// </summary>
    /// <returns>後攻チーム名</returns>
    public string GetSecondTeam()
    {
        string ret = secondAreaName + "\n" + secondTeamName;
        return ret;
    }

    /// <summary>
    /// 地域名を取得する。
    /// othre が指定してあれば、かぶらないように再抽選を行う。
    /// ただし、最大5回抽選してもかぶるようならそれを採用する。
    /// </summary>
    /// <param name="other">他チームの地域名</param>
    /// <returns>地域名</returns>
    private string GetAreaName(string other)
    {
        string areaName = RandomName.GatArea();
        if (other.Length > 0)
        {
            int count = 5;
            while (count > 0)
            {
                if (areaName == other)
                {
                    areaName = RandomName.GatArea();
                    count--;
                }
                else
                {
                    break;
                }
            }
        }
        return areaName;
    }

    /// <summary>
    /// チーム名を取得する。
    /// othre が指定してあれば、かぶらないように再抽選を行う。
    /// ただし、最大5回抽選してもかぶるようならそれを採用する。
    /// </summary>
    /// <param name="other">他チームのチーム名</param>
    /// <returns>チーム名</returns>
    private string GetTeamName(string other)
    {
        string teamName = RandomName.GetName();
        if (other.Length > 0)
        {
            int count = 5;
            while (count > 0)
            {
                if (teamName == other)
                {
                    teamName = RandomName.GetName();
                    count--;
                }
                else
                {
                    break;
                }
            }
        }
        return teamName;
    }
}
