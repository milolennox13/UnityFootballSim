using UnityEngine;

public static class ProceduralCircleMesh
{
    /// <summary>
    /// Creates a ring mesh (circle outline) or arc between two angles.
    /// </summary>
    /// <param name="radius">Outer radius of the circle</param>
    /// <param name="thickness">Stroke thickness (inner radius = radius - thickness)</param>
    /// <param name="segments">Number of segments for smoothness</param>
    /// <param name="startAngle">Start angle in degrees</param>
    /// <param name="endAngle">End angle in degrees</param>
    public static Mesh Generate(float radius, float thickness, int segments = 64, float startAngle = 0f, float endAngle = 360f)
    {
        Mesh mesh = new Mesh();

        float innerRadius = radius - thickness;
        if (innerRadius < 0f) innerRadius = 0f;

        // Normalize angle range
        if (endAngle < startAngle)
            endAngle += 360f;

        float angleRange = endAngle - startAngle;
        int steps = Mathf.Max(2, Mathf.CeilToInt(segments * (angleRange / 360f)));

        Vector3[] vertices = new Vector3[steps * 2];
        int[] triangles = new int[(steps - 1) * 6];
        Vector2[] uv = new Vector2[vertices.Length];

        for (int i = 0; i < steps; i++)
        {
            float t = (float)i / (steps - 1);
            float angle = Mathf.Deg2Rad * (startAngle + t * angleRange);
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            // Outer vertex
            vertices[i * 2] = new Vector3(cos * radius, sin * radius, 0);
            // Inner vertex
            vertices[i * 2 + 1] = new Vector3(cos * innerRadius, sin * innerRadius, 0);

            uv[i * 2] = new Vector2(t, 1);
            uv[i * 2 + 1] = new Vector2(t, 0);

            // Build triangles
            if (i < steps - 1)
            {
                int baseIndex = i * 6;
                int vi = i * 2;

                triangles[baseIndex] = vi;
                triangles[baseIndex + 1] = vi + 2;
                triangles[baseIndex + 2] = vi + 1;

                triangles[baseIndex + 3] = vi + 1;
                triangles[baseIndex + 4] = vi + 2;
                triangles[baseIndex + 5] = vi + 3;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
