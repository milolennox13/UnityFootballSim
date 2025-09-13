using UnityEngine;

public class Goal : MonoBehaviour
{
    public int team;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) // Ensure the Ball has the "Ball" tag
        {
            Ball b = other.GetComponent<Ball>();
            b.inGoal = true;
            if (team == 1)
            {
                FindObjectOfType<ScoreManager>().AddPointToTeam2();
            }
            else
            {
                FindObjectOfType<ScoreManager>().AddPointToTeam1();
            }
            b.Reset();
            b.inGoal = false;

            FootballPlayer[] players = FindObjectsOfType<FootballPlayer>();

            // Reset each one
            foreach (FootballPlayer player in players)
            {
                player.ResetPlayer();
            }
        }
    }

    void Start()
    {
    }
    
    void Update()
    {
    }
}