using UnityEngine;

public class FootballPlayer : MonoBehaviour
{
    public float maxSpeed;
    public float maxAcc;
    public float maxShotPower;
    public float ballAggression;
    public float shotAccuracy;
    public Vector2 target;
    public Vector2 designatedPosition;
    public Team team;
    private float currentSpeed;
    private Vector2 currentDirection;
    private Ball ball;

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
        currentSpeed = maxSpeed;
    }
    void FixedUpdate()
    {
        if (IsClosestToBall())
        {
            TargetBall();
        }
        else
        {
            Vector2 bestTarget = transform.position;
            float bestScore = 0;
            for (int i = 0; i < 5; i++)
            {
                Vector2 tryPosition = (Vector2)transform.position + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                float objective = PositionObjectiveFunction(tryPosition, 0);
                if (objective > bestScore)
                {
                    bestTarget = tryPosition;
                    bestScore = objective;
                }
            }
            target = bestTarget;
        }
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
        if (Vector2.Distance(transform.position, ball.transform.position) < 0.25f)
        {
            Pitch pitch = FindObjectOfType<Pitch>();
            Vector2 shotTarget = new Vector2(team.dir * 0.5f * pitch.dimensions[0] * pitch.yardScale, 0);
            Shoot(ball, shotTarget, maxShotPower);
        }
    }

    void TargetBall()
    {
        if (ball != null)
        {
            target = (Vector2)ball.GetPosition();
        }
    }

    void TargetPosition()
    {
        if (ball != null)
        {
            target = ballAggression * (Vector2)ball.GetPosition() + (1 - ballAggression) * designatedPosition;
        }
    }

    float PositionObjectiveFunction(Vector2 location, float plusTime = 0)
    {
        float distanceFromBall = (location - ball.FuturePosition(plusTime)).magnitude;
        float playerCoverage = PlayerCoverageContribution(location, plusTime);
        float teamCoverage = team.TeamPitchControl(location, plusTime);

        return playerCoverage / distanceFromBall / teamCoverage;
    }

    void KickBall(Ball ball)
    {
        if (ball != null)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            float kickForce = maxShotPower;
            Vector2 dir = Vector2.zero;
            dir = new Vector2(Random.Range(0f, team.dir), Random.Range(-1f, 1f));
            rb.AddForce(Random.Range(0f, 1f) * dir.normalized * kickForce, ForceMode2D.Impulse);
        }
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

    void Shoot(Ball ball, Vector2 shotTarget, float kickForce)
    {
        if (ball != null)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            Vector2 dir = Vector2.zero;
            float angleInDegrees = Vector2.SignedAngle(Vector2.up, shotTarget - (Vector2)(transform.position));
            float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
            angleInRadians += (1 / shotAccuracy) * Random.Range(-1f, 1f);
            dir = new Vector2(-Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));
            rb.AddForce(dir.normalized * kickForce, ForceMode2D.Impulse);
        }
    }
}