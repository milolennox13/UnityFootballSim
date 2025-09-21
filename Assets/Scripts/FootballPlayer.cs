using UnityEngine;
using System.Linq;

public class FootballPlayer : MonoBehaviour
{
    public float maxSpeed;
    public float maxAcc;
    public float maxShotPower;
    public float ballAggression;
    public float kickAccuracy;
    public float playerPlusTime;
    public Vector2 target = new Vector2(0f, 0f);
    public Vector2 designatedPosition;
    public Team team;
    private float currentSpeed;
    private Vector2 currentDirection;
    private Ball ball;
    private Pitch pitch;
    private Vector2 shotTarget;
    private bool kicking;
    private float kickTimer;
    private Vector2 kickTarget;
    private float kickForce;

    [System.Serializable]
    public class PlayerTargetCoefficients
    {
        public float playerCoverage;
        public float distanceFromBall;
        public float distanceFromGoal;
        public float teamCoverage;
        public float playerPosition;
    }

    public PlayerTargetCoefficients targetCoefficients;

    [System.Serializable]
    public class PlayerPassCoefficients
    {
        public float distanceFromBall;
        public float distanceFromGoal;
    }
    public PlayerPassCoefficients passCoefficients;

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    void Start()
    {
        if (GetComponent<Rigidbody2D>() == null)
        {
            Debug.LogError("Rigidbody2D is missing from this GameObject!");
        }
        ball = FindObjectOfType<Ball>();
        if (ball == null)
        {
            Debug.LogError("No Ball object found in the scene!");
        }
        pitch = FindObjectOfType<Pitch>();
        if (pitch == null)
        {
            Debug.LogError("No Pitch object found in the scene!");
        }
        shotTarget = new Vector2(team.dir * 0.5f * pitch.dimensions[0] * pitch.yardScale, 0);
        currentSpeed = maxSpeed;
    }
    void FixedUpdate()
    {
        TargetPosition();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        currentDirection = (target - (Vector2)transform.position).normalized;
        Vector2 v = rb.velocity;
        Vector2 desiredVelocity = currentDirection * currentSpeed;
        float frameAcc = maxAcc * Time.deltaTime;

        if ((desiredVelocity - v).magnitude <= frameAcc)
        {
            rb.velocity = desiredVelocity;
        }
        else
        {
            rb.velocity = v + (desiredVelocity - v).normalized * frameAcc;
        }
        if (kicking)
        {
            kickTimer -= Time.deltaTime;
            if (kickTimer < 0)
            {
                kicking = false;
                float kickRange = ball.GetComponent<CircleCollider2D>().radius + 0.2f;
                if (Vector2.Distance(transform.position, ball.transform.position) < kickRange)
                {
                    ExecuteKick();
                }
            }
        }
        else if (Vector2.Distance(transform.position, ball.transform.position) < ball.GetComponent<CircleCollider2D>().radius + 0.5f)
        {
            if (Vector2.Distance(transform.position, shotTarget) < 1.8f)
            {
                Shoot();
            }
            else
            {
                Pass(playerPlusTime);
            }
        }

    }

    public void ResetPlayer()
    {
        transform.position = designatedPosition;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.velocity = Vector2.zero;
        target = new Vector2(0f, 0f);
    }

    void TargetPosition()
    {
        Vector2 bestTarget = target;
        float bestScore = PositionObjectiveFunction(target, playerPlusTime);
        for (int i = 0; i < 10; i++)
        {
            Vector2 tryPosition = (Vector2)transform.position + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            float objective = PositionObjectiveFunction(tryPosition, playerPlusTime);
            if (objective < bestScore)
            {
                bestTarget = tryPosition;
                bestScore = objective;
            }
        }
        target = bestTarget;
    }

    float PositionObjectiveFunction(Vector2 location, float plusTime = 0)
    {
        float logDistanceFromBall = Mathf.Log((location - ball.FuturePosition(plusTime)).magnitude);
        float logDistanceFromGoal = Mathf.Log((location - shotTarget).magnitude);
        float logPlayerCoverage = Mathf.Log(PlayerCoverageContribution(location, plusTime));
        float logTeamCoverage = Mathf.Log(team.TeamPitchControl(location, plusTime));
        float logPlayerPosition = Mathf.Log((location - designatedPosition).magnitude);

        return -targetCoefficients.playerCoverage * logPlayerCoverage + targetCoefficients.distanceFromBall * logDistanceFromBall + targetCoefficients.distanceFromGoal * logDistanceFromGoal + targetCoefficients.teamCoverage * logTeamCoverage + targetCoefficients.playerPosition * logPlayerPosition;
    }

    float DribbleObjectiveFunction(Vector2 location, float plusTime = 0)
    {
        float logDistanceFromBall = Mathf.Log((location - ball.FuturePosition(plusTime)).magnitude);
        float logDistanceFromGoal = Mathf.Log((location - shotTarget).magnitude);
        float logTeamCoverage = Mathf.Log(team.TeamPitchControl(location, plusTime));

        return targetCoefficients.distanceFromBall * logDistanceFromBall + targetCoefficients.distanceFromGoal * logDistanceFromGoal - targetCoefficients.teamCoverage * logTeamCoverage;
    }

    float PlayerCoverageContribution(Vector2 location, float plusTime = 0)
    {
        Vector2 futurePosition = FuturePosition(plusTime);
        float playerCoverage = ((Vector2)futurePosition - location).magnitude;
        float teamCoverage = team.TeamCoverage(location, plusTime);
        return playerCoverage / teamCoverage;
    }

    public Vector2 FuturePosition(float plusTime = 0)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        return (Vector2)transform.position + plusTime * (Vector2)rb.velocity;
    }

    bool IsClosestToBall()
    {
        Vector2 myPosition = transform.position;
        Vector2 ballPosition = ball.transform.position;
        float myDistSqr = (myPosition - ballPosition).sqrMagnitude;

        foreach (var other in FindObjectsOfType<FootballPlayer>())
        {
            if (other.team != team) continue;

            float otherDistSqr = ((Vector2)other.transform.position - ballPosition).sqrMagnitude;
            if (otherDistSqr < myDistSqr)
            {
                return false;
            }
        }
        return true;
    }

    float EvaluatePassOption(Vector2 passerPosition, float plusTime = 0)
    {
        Vector2 futurePos = FuturePosition(plusTime);
        float logDistanceFromBall = Mathf.Log((futurePos - (Vector2)ball.GetPosition()).magnitude);
        float logDistanceFromGoal = Mathf.Log((futurePos - shotTarget).magnitude);
        return passCoefficients.distanceFromGoal * logDistanceFromGoal + passCoefficients.distanceFromBall * logDistanceFromBall;
    }

    void PrepareKick()
    {
        kicking = true;
        kickTimer = 0.5f * kickForce / maxShotPower;
    }

    void ExecuteKick()
    {
        if (ball != null)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            Vector2 dir = (kickTarget - (Vector2)transform.position).normalized;
            rb.AddForce(dir * kickForce, ForceMode2D.Impulse);
        }
    }

    void Shoot()
    {
        kickTarget = shotTarget;
        kickForce = maxShotPower;
        PrepareKick();
    }

    void Pass(float plusTime = 0)
    {
        FootballPlayer[] teamPlayers = team.players;

        FootballPlayer passTarget = teamPlayers
            .OrderBy(p => p.EvaluatePassOption(transform.position, plusTime))
            .First();

        if (passTarget == this)
        {
            Dribble(playerPlusTime);
        }
        else
        {
            kickTarget = passTarget.FuturePosition(plusTime);
            kickForce = maxShotPower;
            PrepareKick();
        }
    }

    void Dribble(float plusTime = 0)
    {
        Vector2 bestTarget = target;
        float bestScore = PositionObjectiveFunction(target, plusTime);
        for (int i = 0; i < 10; i++)
        {
            Vector2 tryPosition = (Vector2)transform.position + new Vector2(0.5f * team.dir, 0) + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            float objective = DribbleObjectiveFunction(tryPosition, plusTime);
            if (objective < bestScore)
            {
                bestTarget = tryPosition;
                bestScore = objective;
            }
        }
        kickTarget = bestTarget;
        kickForce = Mathf.Min(maxShotPower, 0.5f);
        PrepareKick();
    }
}