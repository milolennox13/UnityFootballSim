using UnityEngine;

public class Pitch : MonoBehaviour
{
    public Sprite grassSprite;
    public Sprite whiteSprite;
    public Material pitchWhiteMaterial;

    public Vector2 dimensions = new Vector2(115f, 74f);
    public float outOfBoundsWidth = 20f;
    public Vector2 goalDimensions = new Vector2(4f, 8f);
    public float whitePaintThickness = 0.1f;
    public float netThickness = 0.05f;
    public float postThickness = 0.1f;
    public float yardScale = 0.1f;

    void Start()
    {
        ResetPitch();
    }

    void ResetPitch()
    {
        Vector2 scaledDimensions = yardScale * dimensions;
        float scaledPaintThickness = yardScale * whitePaintThickness;

        PitchBuilder.CreatePitchRectangle(pitchWhiteMaterial, "PitchOutline", Vector2.zero, scaledDimensions[0], scaledDimensions[1], scaledPaintThickness, transform);

        Ball ball = FindObjectOfType<Ball>();
        CircleCollider2D ballCol = ball.GetComponent<CircleCollider2D>();
        float ballDiameter = 2 * ballCol.radius;
        PitchBuilder.CreatePitchElement("OoBSideLine", "TopOutofBounds", null, new Vector2(0, ballDiameter + scaledDimensions[1] / 2 + outOfBoundsWidth / 2 * yardScale), new Vector2(scaledDimensions[0], outOfBoundsWidth * yardScale), transform);
        PitchBuilder.CreatePitchElement("OoBSideLine", "BottomOutofBounds", null, new Vector2(0, -(ballDiameter + scaledDimensions[1] / 2 + outOfBoundsWidth / 2 * yardScale)), new Vector2(scaledDimensions[0], outOfBoundsWidth * yardScale), transform);
        PitchBuilder.CreatePitchElement("OoBGoalLine", "LeftOutofBounds", null, new Vector2(-(ballDiameter + scaledDimensions[0] / 2 + outOfBoundsWidth / 2 * yardScale), 0), new Vector2(outOfBoundsWidth * yardScale, scaledDimensions[1]), transform);
        PitchBuilder.CreatePitchElement("OoBGoalLine", "RightOutofBounds", null, new Vector2(ballDiameter + scaledDimensions[0] / 2 + outOfBoundsWidth / 2 * yardScale, 0), new Vector2(outOfBoundsWidth * yardScale, scaledDimensions[1]), transform);

        Vector2 scaledGoalDimensions = yardScale * goalDimensions;
        GameObject goal1 = PitchBuilder.CreatePitchElement("Goal", "LeftGoal", null, new Vector2(-(ballDiameter + scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2), 0), scaledGoalDimensions, transform);
        GameObject goal2 = PitchBuilder.CreatePitchElement("Goal", "RightGoal", null, new Vector2(ballDiameter + scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2, 0), scaledGoalDimensions, transform);
        Goal goalComponent1 = goal1.GetComponent<Goal>();
        Goal goalComponent2 = goal2.GetComponent<Goal>();
        goalComponent1.team = 1;
        goalComponent2.team = 2;

        float scaledNetThickness = yardScale * netThickness;
        PitchBuilder.CreatePitchElement("Net", "LeftGoalBackNet", whiteSprite, new Vector2(-(scaledDimensions[0] / 2 + scaledGoalDimensions[0]), 0), new Vector2(scaledNetThickness, scaledGoalDimensions[1]), goal1.transform);
        PitchBuilder.CreatePitchElement("Net", "LeftGoalLeftNet", whiteSprite, new Vector2(-(scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2), -(scaledGoalDimensions[1] / 2)), new Vector2(scaledGoalDimensions[0], scaledNetThickness), goal1.transform);
        PitchBuilder.CreatePitchElement("Net", "LeftGoalRightNet", whiteSprite, new Vector2(-(scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2), scaledGoalDimensions[1] / 2), new Vector2(scaledGoalDimensions[0], scaledNetThickness), goal1.transform);
        PitchBuilder.CreatePitchElement("Net", "RightGoalBackNet", whiteSprite, new Vector2(scaledDimensions[0] / 2 + scaledGoalDimensions[0], 0), new Vector2(scaledNetThickness, scaledGoalDimensions[1]), goal2.transform);
        PitchBuilder.CreatePitchElement("Net", "RightGoalLeftNet", whiteSprite, new Vector2(scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2, scaledGoalDimensions[1] / 2), new Vector2(scaledGoalDimensions[0], scaledNetThickness), goal2.transform);
        PitchBuilder.CreatePitchElement("Net", "RightGoalRightNet", whiteSprite, new Vector2(scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2, -(scaledGoalDimensions[1] / 2)), new Vector2(scaledGoalDimensions[0], scaledNetThickness), goal2.transform);

        float scaledPostThickness = yardScale * postThickness;
        PitchBuilder.CreatePitchElement("Crossbar", "LeftCrossbar", whiteSprite, new Vector2(-(scaledDimensions[0] / 2), 0), new Vector2(scaledPostThickness, scaledGoalDimensions[1]), goal1.transform);
        PitchBuilder.CreatePitchElement("Crossbar", "RightCrossbar", whiteSprite, new Vector2(scaledDimensions[0] / 2, 0), new Vector2(scaledPostThickness, scaledGoalDimensions[1]), goal2.transform);

        PitchBuilder.CreatePitchRectangle(pitchWhiteMaterial, "HalfwayLine", Vector2.zero, scaledPaintThickness, scaledDimensions[1], scaledPaintThickness, transform);
        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "CentreDot", Vector2.zero, 0.25f * yardScale, 0.25f * yardScale, 0f, 360f, transform);

        if (dimensions[0] > 64 & dimensions[1] > 64)
        {
            AddPitchMarkings();
        }
    }
    void AddPitchMarkings()
    {
        Vector2 scaledDimensions = yardScale * dimensions;
        float scaledPaintThickness = yardScale * whitePaintThickness;

        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "CentreCircle", Vector2.zero, 10 * yardScale, scaledPaintThickness, 0f, 360f);

        Vector2 scaledPenaltyArea = yardScale * new Vector2(18f, 44f);
        PitchBuilder.CreatePitchRectangle(pitchWhiteMaterial, "PenaltyAreaLeft", new Vector2(0.5f * (scaledPenaltyArea[0] - scaledDimensions[0]), 0), scaledPenaltyArea[0], scaledPenaltyArea[1], scaledPaintThickness, transform);
        PitchBuilder.CreatePitchRectangle(pitchWhiteMaterial, "PenaltyAreaRight", new Vector2(0.5f * (scaledDimensions[0] - scaledPenaltyArea[0]), 0), scaledPenaltyArea[0], scaledPenaltyArea[1], scaledPaintThickness, transform);

        Vector2 scaledGoalArea = yardScale * new Vector2(6f, 20f);
        PitchBuilder.CreatePitchRectangle(pitchWhiteMaterial, "GoalAreaLeft", new Vector2(0.5f * (scaledGoalArea[0] - scaledDimensions[0]), 0), scaledGoalArea[0], scaledGoalArea[1], scaledPaintThickness, transform);
        PitchBuilder.CreatePitchRectangle(pitchWhiteMaterial, "GoalAreaRight", new Vector2(0.5f * (scaledDimensions[0] - scaledGoalArea[0]), 0), scaledGoalArea[0], scaledGoalArea[1], scaledPaintThickness, transform);

        float penaltyArcEndAngle = Mathf.Acos(0.6f) * Mathf.Rad2Deg;       // ≈ 1.047 rad
        float penaltyArcStartAngle = -penaltyArcEndAngle;
        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "PenaltyArcLeft", new Vector2(-0.5f * scaledDimensions[0] + 12f * yardScale, 0), 10f * yardScale, scaledPaintThickness, penaltyArcStartAngle, penaltyArcEndAngle, transform);
        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "PenaltyArcLeft", new Vector2(0.5f * scaledDimensions[0] - 12f * yardScale, 0), 10f * yardScale, scaledPaintThickness, 180f + penaltyArcStartAngle, 180f + penaltyArcEndAngle, transform);

        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "CornerArcTopLeft", new Vector2(-0.5f * scaledDimensions[0], 0.5f * scaledDimensions[1]), 1f * yardScale, scaledPaintThickness, 270f, 0f, transform);
        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "CornerArcBottomLeft", new Vector2(-0.5f * scaledDimensions[0], -0.5f * scaledDimensions[1]), 1f * yardScale, scaledPaintThickness, 0f, 90f, transform);
        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "CornerArcTopRight", new Vector2(0.5f * scaledDimensions[0], 0.5f * scaledDimensions[1]), 1f * yardScale, scaledPaintThickness, 180f, 270f, transform);
        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "CornerArcBottomRight", new Vector2(0.5f * scaledDimensions[0], -0.5f * scaledDimensions[1]), 1f * yardScale, scaledPaintThickness, 90f, 180f, transform);

        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "PenaltyDotLeft", new Vector2(-0.5f * scaledDimensions[0] + 12f * yardScale, 0), 0.25f * yardScale, 0.25f * yardScale, 0f, 360f, transform);
        PitchBuilder.CreatePitchCircle(pitchWhiteMaterial, "PenaltyDotRight", new Vector2(0.5f * scaledDimensions[0] - 12f * yardScale, 0), 0.25f * yardScale, 0.25f * yardScale, 0f, 360f, transform);
    }
}
