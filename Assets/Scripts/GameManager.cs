using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball ball;

    void FixedUpdate()
    {
        GetPossessionTeam();
    }

    public Team GetPossessionTeam()
    {
        Team[] allTeams = FindObjectsOfType<Team>();
        FootballPlayer[] allPlayers = FindObjectsOfType<FootballPlayer>();
        FootballPlayer closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (var player in allPlayers)
        {
            float distance = Vector2.Distance(player.transform.position, ball.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        if (closestPlayer != null)
        {
            foreach (var team in allTeams)
            {
                team.inPossession = (closestPlayer.team == team);
            }
            return closestPlayer.team;
        }

        return null;
    }
}
