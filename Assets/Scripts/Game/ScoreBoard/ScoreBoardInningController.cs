using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoardInningController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inningText;
    [SerializeField] private TextMeshProUGUI firstScoreText;
    [SerializeField] private TextMeshProUGUI secondScoreText;

    public void Redraw(string inning, string firstScore, string secondScore)
    {
        if (inningText != null)
        {
            inningText.text = inning;
        }
        if (firstScoreText != null)
        {
            firstScoreText.text = firstScore;
        }
        if (secondScoreText != null)
        {
            secondScoreText.text = secondScore;
        }
    }

    public void RedrawScore(string firstScore, string secondScore)
    {
        if (firstScoreText != null)
        {
            firstScoreText.text = firstScore;
        }
        if (secondScoreText != null)
        {
            secondScoreText.text = secondScore;
        }
    }
}
