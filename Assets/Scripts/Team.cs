using UnityEngine;

public class Team : MonoBehaviour
{
    public int dir;
    public bool inPossession;
    public string teamName;
    public int teamId;
    public GameObject playerPrefab;   // Assign in Inspector
    public Vector2[] startingPositions; // Set per team (array of designated positions)
    public FootballPlayer[] players; // store references

    void Start()
    {
        SpawnPlayers();
    }

    void Update()
    {

    }

    void SpawnPlayers()
    {
        players = new FootballPlayer[startingPositions.Length];

        for (int i = 0; i < startingPositions.Length; i++)
        {
            // Create the player object
            GameObject playerObj = Instantiate(playerPrefab, startingPositions[i], Quaternion.identity, transform);

            // Get the FootballPlayer script
            FootballPlayer fp = playerObj.GetComponent<FootballPlayer>();

            // Assign attributes
            fp.team = this;
            fp.designatedPosition = startingPositions[i];

            // Store reference
            players[i] = fp;
        }
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