using UnityEngine;

public class Team : MonoBehaviour
{
    public int dir;
    public bool inPossession;

    void Start()
    {

    }

    void Update()
    {

    }

    float TeamCoverage(Vector2 location)
    {
        float thisTeamCoverage = 0;
        foreach (var player in FindObjectsOfType<FootballPlayer>())
        {
            if (player.team == this)
            {
                thisTeamCoverage += 1 / ((Vector2)player.transform.position - location).magnitude;
            }
        }
        return thisTeamCoverage;
    }

    float TeamPitchControl(Vector2 location, float plusTime = 0)
    {
        float thisTeamCoverage = 0;
        float otherTeamCoverage = 0;
        foreach (var player in FindObjectsOfType<FootballPlayer>())
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>(player);
            Vector2 futurePosition = player.transform.position + plusTime * rb.velocity;

            if (player.team == this)
            {
                thisTeamCoverage += 1 / (futurePosition - location).magnitude;
            }
            else
            {
                otherTeamCoverage += 1 / (futurePosition - location).magnitude;
            }
        }
        return thisTeamCoverage / (thisTeamCoverage + otherTeamCoverage);
    }

}