using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralCircleMesh : MonoBehaviour
{
    public float radius = 1f;
    public float thickness = 0.1f;
    public int segments = 64; // More segments = smoother circle
    public float angleSpan = 360f; // Degrees (360 = full circle)

    void Start()
    {
        GenerateCircleMesh();
    }

    void GenerateCircleMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments * 2 + 2];
        int[] triangles = new int[segments * 6];

        float angleStep = angleSpan / segments;
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            // Outer and inner points
            Vector3 outer = new Vector3(cos * radius, sin * radius, 0);
            Vector3 inner = new Vector3(cos * (radius - thickness), sin * (radius - thickness), 0);

            vertices[vertIndex++] = outer;
            vertices[vertIndex++] = inner;

            // Add triangles
            if (i < segments)
            {
                int start = i * 2;
                triangles[triIndex++] = start;
                triangles[triIndex++] = start + 1;
                triangles[triIndex++] = start + 2;

                triangles[triIndex++] = start + 2;
                triangles[triIndex++] = start + 1;
                triangles[triIndex++] = start + 3;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
