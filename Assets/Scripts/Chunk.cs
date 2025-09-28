using NaughtyAttributes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public class AdjecentChunks
    {
        public Chunk Front;
        public Chunk Back;
        public Chunk Left;
        public Chunk Right;
        public Chunk Bottom;
        public Chunk Top;
    }

    enum TestGrid { rows };

    [SerializeField] public Vector3Int ChunkPosition = Vector3Int.zero;
    [SerializeField] TestGrid testGrid;

    [HideInInspector] public int ChunkSize = 3;
    int[,,] grid;
    AdjecentChunks adjecentChunks;
    MeshFilter meshFilter;
    Mesh mesh;

    private void Awake()
    {
        transform.position = ChunkPosition * ChunkSize;
        meshFilter = GetComponent<MeshFilter>();

        mesh = new();
        mesh.name = "Mesh: " + ChunkPosition;
        meshFilter.mesh = mesh;

        SetTestGrid();
    }

    void SetTestGrid()
    {
        grid = new int[ChunkSize, ChunkSize, ChunkSize];
        switch (testGrid)
        {
            case TestGrid.rows:
                for (int x = 0; x < ChunkSize; x++)
                {
                    for (int y = 0; y < ChunkSize; y++)
                    {
                        for (int z = 0; z < ChunkSize; z++)
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

        for (int x = 0; x < ChunkSize; x++)
        {
            for (int y = 0; y < ChunkSize; y++)
            {
                for (int z = 0; z < ChunkSize; z++)
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
                    if (z == ChunkSize - 1 || grid[x, y, z + 1] == 0) //check if face is not obscured
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
                    if (x == ChunkSize - 1 || grid[x + 1, y, z] == 0) //check if face is not obscured
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
                    if (y == ChunkSize - 1 || grid[x, y + 1, z] == 0) //check if face is not obscured
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
