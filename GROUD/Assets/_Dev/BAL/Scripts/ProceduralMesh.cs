using System;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
    [SerializeField] private MeshCollider collider; // Radius of the mesh
    [SerializeField] MeshFilter filter; // Radius of the mesh
    [SerializeField, ReadOnly] private int numSegments = 10; // Number of segments in the mesh
    [SerializeField, ReadOnly] private float depth = 1.0f; // Radius of the mesh

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector3 origin = Vector3.zero;
    private Vector2[] uvs;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        transform.rotation = Quaternion.Euler(-90f, -90f, -90f);
        CreateMesh();
    }

    private void calculateVertice()
    {
        vertices = new Vector3[numSegments * 2];
        uvs = new Vector2[numSegments * 2];

        float angle = 0f;
        float x = 0f;
        float z = 0f;
        for (int i = 0; i < numSegments; i++)
        {
            angle = i * Mathf.PI * 2.0f / numSegments;
            x = origin.x + Mathf.Cos(angle) * depth;
            z = origin.z + Mathf.Sin(angle) * depth;

            vertices[i * 2] = new Vector3(x, origin.y, z);
            vertices[i * 2 + 1] = new Vector3(x, origin.y + 1, z);

            float y = Math.Abs(z);

            uvs[i * 2] = new Vector2(i, 1);
            uvs[i * 2 + 1] = new Vector2(i, 1);
        }

        // uvs[vertices.Length - 1] = new Vector2(1, 1);
        //
        // angle = vertices.Length - 1 * Mathf.PI * 2.0f / numSegments;
        // x = origin.x + Mathf.Cos(angle) * depth;
        // z = origin.z + Mathf.Sin(angle) * depth;
        // vertices[vertices.Length - 1] = new Vector3(x, origin.y, z);
    }

    private void calculateTriangle()
    {
        triangles = new int[numSegments * 9];
        for (int i = 0; i < numSegments; i++)
        {
            int triangleIndex = i * 9;

            // Triangle 1
            triangles[triangleIndex] = i * 2;
            triangles[triangleIndex + 1] = i * 2 + 1;
            triangles[triangleIndex + 2] = (i + 1) % numSegments * 2;

            // Triangle 2
            triangles[triangleIndex + 3] = (i + 1) % numSegments * 2;
            triangles[triangleIndex + 4] = i * 2 + 1;
            triangles[triangleIndex + 5] = (i + 1) % numSegments * 2 + 1;

            //back face
            // triangles[triangleIndex + 6] = i * 2;
            // triangles[triangleIndex + 7] = (i + 1) % numSegments * 2;
            // triangles[triangleIndex + 8] = vertices.Length - 1;
        }
    }

    // Create the mesh
    void CreateMesh()
    {
        vertices = new Vector3[numSegments * 2 + 1];
        triangles = new int[numSegments * 9];
        uvs = new Vector2[vertices.Length];

        // Create vertices
        calculateVertice();

        // Create triangles
        calculateTriangle();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();


        filter.mesh = mesh;
        collider.sharedMesh = mesh;
        SpinManager.instance.SetMesh(gameObject, numSegments);
    }
}