using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] Chunk[] existingChunks;
    [SerializeField] int chunkSize = 16;

    Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();

    private void Start()
    {
        SetExistingChunks();
    }

    void SetExistingChunks()
    {
        foreach (Chunk chunk in existingChunks)
        {
            chunk.transform.position = chunk.ChunkPosition * chunkSize;
            chunk.ChunkSize = chunkSize;

            chunks.Add(chunk.ChunkPosition, chunk);
        }
    }
}
