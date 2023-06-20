using UnityEngine;

public class RoomPosition : MonoBehaviour
{
    public ChunkPosition ContainerChunkPosition { get; set; }
    public Vector2 RoomPositionInContainer { get; set; }
    public string coordinates;

    private void Start()
    {
        coordinates = GetRoomCoordinatesInWorld();
    }

    public string GetRoomCoordinatesInWorld()
    {
        return ContainerChunkPosition.ChunkPositionInWorld.x.ToString() + ":" + ContainerChunkPosition.ChunkPositionInWorld.y.ToString() + 
            "-" + RoomPositionInContainer.x.ToString() + ":" + RoomPositionInContainer.y.ToString();
    }
}
