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
                b.Reset();
                b.inGoal = false;
            }
            else
            {
                FindObjectOfType<ScoreManager>().AddPointToTeam1();
                b.Reset();
                b.inGoal = false;
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