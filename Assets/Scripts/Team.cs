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

    public float TeamCoverage(Vector2 location, float plusTime = 0)
    {
        float thisTeamCoverage = 0;
        foreach (var player in FindObjectsOfType<FootballPlayer>())
        {
            if (player.team == this)
            {
                thisTeamCoverage += 1 / (player.FuturePosition(plusTime) - location).magnitude;
            }
        }
        return thisTeamCoverage;
    }

    public float TeamPitchControl(Vector2 location, float plusTime = 0)
    {
        float thisTeamCoverage = 0;
        float otherTeamCoverage = 0;
        foreach (var player in FindObjectsOfType<FootballPlayer>())
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (player.team == this)
            {
                thisTeamCoverage += 1 / (player.FuturePosition(plusTime) - location).magnitude;
            }
            else
            {
                otherTeamCoverage += 1 / (player.FuturePosition(plusTime) - location).magnitude;
            }
        }
        return thisTeamCoverage / (thisTeamCoverage + otherTeamCoverage);
    }

}