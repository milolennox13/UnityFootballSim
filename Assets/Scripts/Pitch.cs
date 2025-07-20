using UnityEngine;

public class Pitch : MonoBehaviour
{
    public Sprite grassSprite;
    public Sprite whiteSprite;
    public Vector2 dimensions = new Vector2(115f, 74f);
    public float outOfBoundsWidth = 20f;
    public Vector2 goalDimensions = new Vector2(2f, 8f);
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
        CreatePitchElement("Grass", "Grass", grassSprite, new Vector2(0, 0), scaledDimensions, transform);

        float scaledPaintThickness = yardScale * whitePaintThickness;
        CreatePitchElement("WhitePaint", "TopSideLine", whiteSprite, new Vector2(0, (scaledDimensions[1] - scaledPaintThickness) / 2), new Vector2(scaledDimensions[0], scaledPaintThickness), transform);
        CreatePitchElement("WhitePaint", "BottomSideLine", whiteSprite, new Vector2(0, -(scaledDimensions[1] - scaledPaintThickness) / 2), new Vector2(scaledDimensions[0], scaledPaintThickness), transform);
        CreatePitchElement("WhitePaint", "LeftGoalLine", whiteSprite, new Vector2(-(scaledDimensions[0] - scaledPaintThickness) / 2, 0), new Vector2(scaledPaintThickness, scaledDimensions[1]), transform);
        CreatePitchElement("WhitePaint", "RightGoalLine", whiteSprite, new Vector2((scaledDimensions[0] - scaledPaintThickness) / 2, 0), new Vector2(scaledPaintThickness, scaledDimensions[1]), transform);

        Ball ball = FindObjectOfType<Ball>();
        CircleCollider2D ballCol = ball.GetComponent<CircleCollider2D>();
        float ballDiameter = 2 * ballCol.radius;
        CreatePitchElement("OoBSideLine", "TopOutofBounds", null, new Vector2(0, ballDiameter + scaledDimensions[1] / 2 + outOfBoundsWidth / 2 * yardScale), new Vector2(scaledDimensions[0], outOfBoundsWidth * yardScale), transform);
        CreatePitchElement("OoBSideLine", "BottomOutofBounds", null, new Vector2(0, -(ballDiameter + scaledDimensions[1] / 2 + outOfBoundsWidth / 2 * yardScale)), new Vector2(scaledDimensions[0], outOfBoundsWidth * yardScale), transform);
        CreatePitchElement("OoBGoalLine", "LeftOutofBounds", null, new Vector2(-(ballDiameter + scaledDimensions[0] / 2 + outOfBoundsWidth / 2 * yardScale), 0), new Vector2(outOfBoundsWidth * yardScale, scaledDimensions[1]), transform);
        CreatePitchElement("OoBGoalLine", "RightOutofBounds", null, new Vector2(ballDiameter + scaledDimensions[0] / 2 + outOfBoundsWidth / 2 * yardScale, 0), new Vector2(outOfBoundsWidth * yardScale, scaledDimensions[1]), transform);

        Vector2 scaledGoalDimensions = yardScale * goalDimensions;
        GameObject goal1 = CreatePitchElement("Goal", "LeftGoal", null, new Vector2(-(ballDiameter + scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2), 0), scaledGoalDimensions, transform);
        GameObject goal2 = CreatePitchElement("Goal", "RightGoal", null, new Vector2(ballDiameter + scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2, 0), scaledGoalDimensions, transform);
        Goal goalComponent1 = goal1.GetComponent<Goal>();
        Goal goalComponent2 = goal2.GetComponent<Goal>();
        goalComponent1.team = 1;
        goalComponent2.team = 2;

        float scaledNetThickness = yardScale * netThickness;
        CreatePitchElement("Net", "LeftGoalBackNet", whiteSprite, new Vector2(-(scaledDimensions[0] / 2 + scaledGoalDimensions[0]), 0), new Vector2(scaledNetThickness, scaledGoalDimensions[1]), goal1.transform);
        CreatePitchElement("Net", "LeftGoalLeftNet", whiteSprite, new Vector2(-(scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2), -(scaledGoalDimensions[1] / 2)), new Vector2(scaledGoalDimensions[0], scaledNetThickness), goal1.transform);
        CreatePitchElement("Net", "LeftGoalRightNet", whiteSprite, new Vector2(-(scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2), scaledGoalDimensions[1] / 2), new Vector2(scaledGoalDimensions[0], scaledNetThickness), goal1.transform);
        CreatePitchElement("Net", "RightGoalBackNet", whiteSprite, new Vector2(scaledDimensions[0] / 2 + scaledGoalDimensions[0], 0), new Vector2(scaledNetThickness, scaledGoalDimensions[1]), goal2.transform);
        CreatePitchElement("Net", "RightGoalLeftNet", whiteSprite, new Vector2(scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2, scaledGoalDimensions[1] / 2), new Vector2(scaledGoalDimensions[0], scaledNetThickness), goal2.transform);
        CreatePitchElement("Net", "RightGoalRightNet", whiteSprite, new Vector2(scaledDimensions[0] / 2 + scaledGoalDimensions[0] / 2, -(scaledGoalDimensions[1] / 2)), new Vector2(scaledGoalDimensions[0], scaledNetThickness), goal2.transform);

        float scaledPostThickness = yardScale * postThickness;
        CreatePitchElement("Crossbar", "LeftCrossbar", whiteSprite, new Vector2(-(scaledDimensions[0] / 2), 0), new Vector2(scaledPostThickness, scaledGoalDimensions[1]), goal1.transform);
        CreatePitchElement("Crossbar", "RightCrossbar", whiteSprite, new Vector2(scaledDimensions[0] / 2, 0), new Vector2(scaledPostThickness, scaledGoalDimensions[1]), goal2.transform);
    }

    public GameObject CreatePitchElement(string type, string name, Sprite sprite, Vector2 position, Vector2 size, Transform parent = null)
    {
        // Create a new GameObject
        GameObject obj = new GameObject(name);

        // Set parent if provided
        if (parent != null)
        {
            obj.transform.SetParent(parent);
        }

        // Set position
        obj.transform.position = position;

        SpriteRenderer sr = null;
        // Add a SpriteRenderer
        if (sprite != null)
        {
            sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            Vector2 spriteSize = sprite.bounds.size;
            Vector3 scale = new Vector3(size.x / spriteSize.x, size.y / spriteSize.y, 1);
            obj.transform.localScale = scale;
        }

        BoxCollider2D col;
        switch (type)
        {
            case "Grass":
                sr.sortingLayerName = "Pitch";
                sr.sortingOrder = 1;
                break;
            case "WhitePaint":
                sr.sortingLayerName = "Pitch";
                sr.sortingOrder = 2;
                break;
            case "OoBGoalLine":
                col = obj.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = size;
                obj.AddComponent<OoBGoalLine>();
                break;
            case "OoBSideLine":
                col = obj.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = size;
                obj.AddComponent<OoBSideLine>();
                break;
            case "Goal":
                col = obj.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
                col.size = size;
                obj.AddComponent<Goal>();
                break;
            case "Net":
                sr.sortingLayerName = "On Pitch";
                sr.sortingOrder = 2;
                break;
            case "Crossbar":
                sr.sortingLayerName = "Above Pitch";
                sr.sortingOrder = 1;
                break;
            default:
                break;
        }

        return obj;
    }
}