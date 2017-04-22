using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class PlanetGenerator : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private float distortionScale = 1f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float minHeight = 0.2f;
    [SerializeField] private bool update = false;

    private Mesh originalMesh;

    private void Awake()
    {
        originalMesh = new Mesh();
        originalMesh.SetVertices(meshFilter.mesh.vertices.ToList());
        originalMesh.SetIndices(meshFilter.mesh.GetIndices(0), MeshTopology.Triangles, 0);
        originalMesh.SetNormals(meshFilter.mesh.normals.ToList());
        originalMesh.uv = meshFilter.mesh.uv;
        Regenerate();
    }

    private void Update()
    {
        if (update)
        {
            update = false;
            Regenerate();
        }
    }

    private void Regenerate()
    {
        List<Vector3> newVertexPositions = new List<Vector3>(originalMesh.vertexCount);
        List<Color> newVertexColors = new List<Color>();
        for (int i = 0; i < originalMesh.vertexCount; i++)
        {
            Vector2 texPos = originalMesh.uv[i];
            Vector3 vertexPos = originalMesh.vertices[i];

            // hack to hide seam
            float seamScale = 1f;
            if (texPos.x <= 0.1f)
                seamScale *= texPos.x * 2f;
            if (texPos.x >= 0.9f)
                seamScale *= (1 - texPos.x) * 2f;
            if (texPos.y <= 0.1f)
                seamScale *= texPos.y * 2f;
            if (texPos.y >= 0.9f)
                seamScale *= (1 - texPos.y) * 2f;

            Assert.IsTrue(texPos.x >= 0f);
            Assert.IsTrue(texPos.x <= 1f);

            Vector3 vertexNormal = originalMesh.normals[i];
            float noiseValue = Mathf.PerlinNoise(texPos.x * frequency, texPos.y * frequency) * seamScale;
            noiseValue = Mathf.Max(minHeight, noiseValue);

            Assert.IsTrue(noiseValue >= 0f);
            Assert.IsTrue(noiseValue <= 1f);

            newVertexPositions.Add(vertexPos + noiseValue * distortionScale * vertexNormal);
            Color vertexColor = new Color(0f, 0f, 0f, noiseValue);
            newVertexColors.Add(vertexColor);
        }
        meshFilter.mesh.SetVertices(newVertexPositions);
        meshFilter.mesh.SetColors(newVertexColors);
    }
}