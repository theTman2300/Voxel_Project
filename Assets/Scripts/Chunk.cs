using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    enum TestGrid { rows };

    [SerializeField] public Vector3 ChunkPosition = Vector3.zero;
    [SerializeField] int chunkSize = 3;
    [SerializeField] TestGrid testGrid;

    int[,,] grid;
    //int[,,] grid =
    //{
    //    {{0, 1, 0}, {1, 1, 1}, {0, 1, 0} },
    //    {{1, 1, 1}, {1, 1, 1}, {1, 1, 1} },
    //    {{0, 1, 0}, {1, 1, 1}, {0, 1, 0} }
    //};
    MeshFilter meshFilter;
    Mesh mesh;

    private void Awake()
    {
        transform.position = ChunkPosition * chunkSize;
        meshFilter = GetComponent<MeshFilter>();

        mesh = new();
        mesh.name = "Mesh: " + ChunkPosition;
        meshFilter.mesh = mesh;

        SetTestGrid();
    }

    void SetTestGrid()
    {
        grid = new int[chunkSize, chunkSize, chunkSize];
        switch (testGrid)
        {
            case TestGrid.rows:
                for (int x = 0; x < chunkSize; x++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        for (int z = 0; z < chunkSize; z++)
                        {
                            if (x%2 == 0 || y%8 == 0) grid[x, y, z] = 1;
                        }
                    }
                }
                break;
        }
    }

    [Button]
    void UpdateMesh()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();
        int verticeCounter = 0;
        //int[] defaultFaceVertices = new int[] { verticeCounter, verticeCounter + 1, verticeCounter + 3, verticeCounter + 1, verticeCounter + 2, verticeCounter + 3 };

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    if (grid[x, y, z] == 0) continue;

                    // --- Z axis ---
                    //front
                    if (z == 0 || grid[x, y, z - 1] == 0) //check if face is not obscured
                    {
                        vertices.AddRange(new Vector3[] { new(x, y, z), new(x, y + 1, z), new(x + 1, y + 1, z), new(x + 1, y, z) });
                        triangles.AddRange(new int[] { verticeCounter, verticeCounter + 1, verticeCounter + 3, verticeCounter + 1, verticeCounter + 2, verticeCounter + 3 });
                        verticeCounter += 4;
                    }

                    //back
                    if (z == chunkSize - 1 || grid[x, y, z + 1] == 0) //check if face is not obscured
                    {
                        vertices.AddRange(new Vector3[] { new(x + 1, y, z + 1), new(x + 1, y + 1, z + 1), new(x, y + 1, z + 1), new(x, y, z + 1) });
                        triangles.AddRange(new int[] { verticeCounter, verticeCounter + 1, verticeCounter + 3, verticeCounter + 1, verticeCounter + 2, verticeCounter + 3 });
                        verticeCounter += 4;
                    }

                    // --- X axis ---
                    //left (Looking at the left side of the chunk)
                    if (x == 0 || grid[x - 1, y, z] == 0) //check if face is not obscured
                    {
                        vertices.AddRange(new Vector3[] { new(x, y, z + 1), new(x, y + 1, z + 1), new(x, y + 1, z), new(x, y, z) });
                        triangles.AddRange(new int[] { verticeCounter, verticeCounter + 1, verticeCounter + 3, verticeCounter + 1, verticeCounter + 2, verticeCounter + 3 });
                        verticeCounter += 4;
                    }

                    //right (Looking at the right side of the chunk)
                    if (x == chunkSize - 1 || grid[x + 1, y, z] == 0) //check if face is not obscured
                    {
                        vertices.AddRange(new Vector3[] { new(x + 1, y, z), new(x + 1, y + 1, z), new(x + 1, y + 1, z + 1), new(x + 1, y, z + 1) });
                        triangles.AddRange(new int[] { verticeCounter, verticeCounter + 1, verticeCounter + 3, verticeCounter + 1, verticeCounter + 2, verticeCounter + 3 });
                        verticeCounter += 4;
                    }

                    // --- Y axis ---
                    //bottom
                    if (y == 0 || grid[x, y - 1, z] == 0) //check if face is not obscured
                    {
                        vertices.AddRange(new Vector3[] { new(x, y, z + 1), new(x, y, z), new(x + 1, y, z), new(x + 1, y, z + 1) });
                        triangles.AddRange(new int[] { verticeCounter, verticeCounter + 1, verticeCounter + 3, verticeCounter + 1, verticeCounter + 2, verticeCounter + 3 });
                        verticeCounter += 4;
                    }

                    //top
                    if (y == chunkSize - 1 || grid[x, y + 1, z] == 0) //check if face is not obscured
                    {
                        vertices.AddRange(new Vector3[] { new(x, y + 1, z), new(x, y + 1, z + 1), new(x + 1, y + 1, z + 1), new(x + 1, y + 1, z) });
                        triangles.AddRange(new int[] { verticeCounter, verticeCounter + 1, verticeCounter + 3, verticeCounter + 1, verticeCounter + 2, verticeCounter + 3 });
                        verticeCounter += 4;
                    }
                }
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }


}
