using UnityEngine;

public static class PitchBuilder
{
    public static GameObject CreatePitchElement(string type, string name, Sprite sprite, Vector2 position, Vector2 size, Transform parent = null)
    {
        GameObject obj = new GameObject(name);
        if (parent != null) obj.transform.SetParent(parent);
        obj.transform.position = position;

        SpriteRenderer sr = null;
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
                sr.sortingLayerName = "Pitch"; sr.sortingOrder = 1; break;
            case "WhitePaint":
                sr.sortingLayerName = "Pitch"; sr.sortingOrder = 2; break;
            case "OoBGoalLine":
                col = obj.AddComponent<BoxCollider2D>(); col.isTrigger = true; col.size = size;
                obj.AddComponent<OoBGoalLine>(); break;
            case "OoBSideLine":
                col = obj.AddComponent<BoxCollider2D>(); col.isTrigger = true; col.size = size;
                obj.AddComponent<OoBSideLine>(); break;
            case "Goal":
                col = obj.AddComponent<BoxCollider2D>(); col.isTrigger = true; col.size = size;
                obj.AddComponent<Goal>(); break;
            case "Net":
                sr.sortingLayerName = "On Pitch"; sr.sortingOrder = 2; break;
            case "Crossbar":
                sr.sortingLayerName = "Above Pitch"; sr.sortingOrder = 1; break;
        }
        return obj;
    }

    public static GameObject CreatePitchCircle(Material pitchWhiteMaterial, string name, Vector2 position, float radius, float thickness, float startAngle, float endAngle, Transform parent = null)
    {
        GameObject circleObj = new GameObject(name);
        MeshFilter mf = circleObj.AddComponent<MeshFilter>();
        MeshRenderer mr = circleObj.AddComponent<MeshRenderer>();

        mf.mesh = ProceduralCircleMesh.Generate(radius, thickness, 128, startAngle, endAngle);
        mr.material = pitchWhiteMaterial;

        circleObj.transform.position = position;
        circleObj.transform.parent = parent;

        mr.sortingLayerName = "Pitch";
        mr.sortingOrder = 2;

        return circleObj;
    }

    public static GameObject CreatePitchRectangle(Material pitchWhiteMaterial, string name, Vector2 position, float width, float height, float thickness, Transform parent = null)
    {
        GameObject rectangleObj = new GameObject(name);
        MeshFilter mf = rectangleObj.AddComponent<MeshFilter>();
        MeshRenderer mr = rectangleObj.AddComponent<MeshRenderer>();

        mf.mesh = ProceduralRectangleMesh.Generate(width, height, thickness);
        mr.material = pitchWhiteMaterial;

        rectangleObj.transform.position = position;
        rectangleObj.transform.parent = parent;

        mr.sortingLayerName = "Pitch";
        mr.sortingOrder = 2;

        return rectangleObj;
    }
}
