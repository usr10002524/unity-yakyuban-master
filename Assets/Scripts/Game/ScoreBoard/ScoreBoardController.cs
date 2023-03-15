using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoardController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI firstNameText;
    [SerializeField] private TextMeshProUGUI secondNameText;

    [SerializeField] private GameObject inningContainerPrerab;
    [SerializeField] private GameObject totalgContainerPrerab;

    [SerializeField] private Vector2 inningContainerPos;
    [SerializeField] private Vector2 inningContainerOffs;
    [SerializeField] private Vector2 totalContainerPos;

    private List<ScoreBoardInningController> inningControllers;
    private ScoreBoardInningController totalController;


    public void Show(bool flag)
    {
        gameObject.SetActive(flag);
    }

    public void OnInit()
    {
        RedrawTeamName();

        inningControllers = new List<ScoreBoardInningController>();

        // 各イニングごとのスコアボードを初期化
        int innings = GameManager.Instance.GetInnings();
        for (int i = 0; i < innings; i++)
        {
            GameObject gameObject = Instantiate(inningContainerPrerab, transform);

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Vector2 pos = inningContainerPos + inningContainerOffs * i;
                rectTransform.anchoredPosition = pos;
            }

            ScoreBoardInningController controller = gameObject.GetComponent<ScoreBoardInningController>();
            if (controller != null)
            {
                string inningText = string.Format("{0}", i + 1);
                controller.Redraw(inningText, "", "");

                inningControllers.Add(controller);
            }
        }

        // トータル部分を初期化
        {
            GameObject gameObject = Instantiate(totalgContainerPrerab, transform);
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = totalContainerPos;
            }

            totalController = gameObject.GetComponent<ScoreBoardInningController>();
            if (totalController != null)
            {
                string inningText = string.Format("計");
                totalController.Redraw(inningText, "0", "0");
            }
        }

        // はじめは非表示
        Show(false);
    }

    public void OnRedraw()
    {
        RedrawTeamName();
        RedrawScoreBoard();
    }

    private void RedrawTeamName()
    {
        if (firstNameText != null)
        {
            firstNameText.text = GameManager.Instance.GetFirstTeamName();
        }
        if (secondNameText != null)
        {
            secondNameText.text = GameManager.Instance.GetSecondTeamName();
        }
    }

    private void RedrawScoreBoard()
    {
        int innings = GameManager.Instance.GetInnings();
        int currentInning = GameManager.Instance.GetCurrentInning();
        bool isGameSet = GameManager.Instance.IsGameSet();
        Core.Order order = GameManager.Instance.GetCurrentOrder();
        Score score = GameManager.Instance.GetScore();

        for (int i = 0; i < innings; i++)
        {
            int firstScore = score.Get(i, Core.Order.First);
            int secondScore = score.Get(i, Core.Order.Second);

            if (i < currentInning || isGameSet)
            {
                // 確定済みの結果
                bool isSayonara = GameManager.Instance.IsSayonara();
                bool isNoLastSecondInning = GameManager.Instance.IsNoLastSecondInning();

                string firstScoreText = string.Format("{0}", firstScore);
                string secondScoreText = string.Format("{0}", secondScore);
                if (GameManager.Instance.IsLastInning(i))
                {
                    if (isNoLastSecondInning)
                    {
                        secondScoreText = "X";
                    }
                    else if (isSayonara)
                    {
                        secondScoreText = string.Format("{0}{1}", secondScore, "x");
                    }
                }

                RedrawScore(i, firstScoreText, secondScoreText);
            }
            else if (i == currentInning)
            {
                // 現在進捗中
                if (order == Core.Order.First)
                {
                    // 先攻は得点が入っていれば表示、なければ非表示
                    string firstScoreText = string.Format("{0}", (firstScore > 0) ? firstScore : "");
                    // 後攻は非表示
                    string secondScoreText = string.Format("");
                    RedrawScore(i, firstScoreText, secondScoreText);
                }
                else
                {
                    // 先攻は結果を表示
                    string firstScoreText = string.Format("{0}", firstScore);
                    // 後攻は得点が入っていれば表示、なければ非表示
                    string secondScoreText = string.Format("{0}", (secondScore > 0) ? secondScore : "");
                    RedrawScore(i, firstScoreText, secondScoreText);
                }
            }
            else
            {
                // その他は非表示
                RedrawScore(i, "", "");
            }
        }

        RedrawTotal();
    }

    private void RedrawScore(int inning, string firstScoreText, string secondScoreText)
    {
        if (inning < inningControllers.Count)
        {
            if (inningControllers[inning] != null)
            {
                inningControllers[inning].RedrawScore(firstScoreText, secondScoreText);
            }
        }
    }

    private void RedrawTotal()
    {
        Score score = GameManager.Instance.GetScore();
        int firstScore = score.GetTotal(Core.Order.First);
        int secondScore = score.GetTotal(Core.Order.Second);
        string firstScoreText = string.Format("{0}", firstScore);
        string secondScoreText = string.Format("{0}", secondScore);

        totalController.RedrawScore(firstScoreText, secondScoreText);
    }
}
