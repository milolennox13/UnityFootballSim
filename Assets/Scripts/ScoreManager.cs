using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int team1Score = 0;
    public int team2Score = 0;

    public TextMeshProUGUI scoreText;

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddPointToTeam1()
    {
        team1Score++;
        UpdateScoreUI();
    }

    public void AddPointToTeam2()
    {
        team2Score++;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = team1Score + " - " + team2Score;
    }
}
