using UnityEngine;

public static class ProceduralRectangleMesh
{
    /// <summary>
    /// Generates a rectangular outline mesh.
    /// </summary>
    /// <param name="width">Outer width of the rectangle</param>
    /// <param name="height">Outer height of the rectangle</param>
    /// <param name="thickness">Thickness of the border</param>
    /// <returns>Mesh representing a rectangular outline</returns>
    public static Mesh Generate(float width, float height, float thickness)
    {
        Mesh mesh = new Mesh();

        float halfW = width / 2f;
        float halfH = height / 2f;
        float t = thickness;

        // 8 vertices: two for each side (outer and inner)
        Vector3[] vertices = new Vector3[8]
        {
            // Bottom edge
            new Vector3(-halfW, -halfH, 0),
            new Vector3(halfW, -halfH, 0),
            new Vector3(-halfW + t, -halfH + t, 0),
            new Vector3(halfW - t, -halfH + t, 0),

            // Top edge
            new Vector3(-halfW, halfH, 0),
            new Vector3(halfW, halfH, 0),
            new Vector3(-halfW + t, halfH - t, 0),
            new Vector3(halfW - t, halfH - t, 0)
        };

        // Define triangles for edges
        int[] triangles = new int[]
        {
            // Bottom edge
            0, 2, 1,
            1, 2, 3,

            // Top edge
            4, 5, 6,
            5, 7, 6,

            // Left edge
            0,4,2,
            4,6,2,

            // Right edge
            1,3,5,
            5,3,7
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
