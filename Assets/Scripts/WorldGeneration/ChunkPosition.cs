using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPosition : MonoBehaviour
{
    public Vector2 ChunkPositionInWorld { get; set; }

    public string GetChunkPositionInWorld()
    {
        return ChunkPositionInWorld.x.ToString() + ":" + ChunkPositionInWorld.y.ToString();
    }
}
