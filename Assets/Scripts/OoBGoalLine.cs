using UnityEngine;

public class OoBGoalLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) // Ensure the Ball has the "Ball" tag
        {
            if (!other.GetComponent<Ball>().inGoal)
            {
                other.GetComponent<Ball>().Reset();
                FootballPlayer[] players = FindObjectsOfType<FootballPlayer>();

                // Reset each one
                foreach (FootballPlayer player in players)
                {
                    player.ResetPlayer();
                }
            }
            else
            {

            }
        }
    }
}