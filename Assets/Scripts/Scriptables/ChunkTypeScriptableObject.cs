using System.Collections.Generic;
using UnityEngine;
using static GenerationManager;

[CreateAssetMenu(fileName = "ChunkType", menuName = "ScriptableObjects/ChunkTypeScriptableObject")]
public class ChunkTypeScriptableObject : ScriptableObject
{
    public ChunkType chunkType = ChunkType.Entrance;
    public Vector4 ranges;
    public List<GameObject> roomPrefabs;
    public GameObject emptyRoomPrefab;
    public GameObject lightPrefab;

    public int mapBrightness = 7;
    public int mapEmptiness = 10;
}
