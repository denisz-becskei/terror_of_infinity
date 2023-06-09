using System.Collections;
using UnityEngine;
using static GenerationManager;
using static IntVector2Init;

public class ChunkData : MonoBehaviour
{
    public IntVector2 ChunkPositionInWorld { get; set; }
    public ChunkType chunkType;

    public string GetChunkPositionInWorld()
    {
        return ChunkPositionInWorld.x.ToString() + ":" + ChunkPositionInWorld.y.ToString();
    }

}
