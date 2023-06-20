using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldWideScripts : MonoBehaviour
{
    public static Dictionary<char, KeyCode> keyCodes = new Dictionary<char, KeyCode>()
    {
        { 'b', KeyCode.B },
        { 'c', KeyCode.C },
        { 'e', KeyCode.E },
        { 'f', KeyCode.F },
        { 'g', KeyCode.G },
        { 'h', KeyCode.H },
        { 'i', KeyCode.I },
        { 'j', KeyCode.J },
        { 'k', KeyCode.K },
        { 'l', KeyCode.L },
        { 'm', KeyCode.M },
        { 'n', KeyCode.N },
        { 'o', KeyCode.O },
        { 'p', KeyCode.P },
        { 'q', KeyCode.Q },
        { 'r', KeyCode.R },
        { 's', KeyCode.S },
        { 't', KeyCode.T },
        { 'u', KeyCode.U },
        { 'v', KeyCode.V },
        { 'x', KeyCode.X },
        { 'y', KeyCode.Y },
        { 'z', KeyCode.Z },
        { '-', KeyCode.None }
    };

    public static GameObject GetFurthestRoomFromGameObject(Transform compare)
    {
        GameObject furthest = null;
        float furthestDistance = -Mathf.Infinity;

        foreach(GameObject room in GenerationManager.rooms)
        {
            if(Vector3.Distance(compare.position, room.transform.position) > furthestDistance && room.GetComponent<Room>().centerAvailable)
            {
                furthestDistance = Vector3.Distance(compare.position, room.transform.position);
                furthest = room;
            }
        }
        return furthest;
    }

    public static GameObject GetFirstCleanRoom()
    {
        bool ready = false;
        GameObject cleanRoom = null;
        while(!ready)
        {
            cleanRoom = GenerationManager.rooms[Random.Range(0, GenerationManager.rooms.Count)];
            if(cleanRoom.GetComponent<Room>().centerAvailable) {
                ready = true;
            }
        }
        return cleanRoom;
    }

    public static GameObject GetFirstCleanRoomInChunkByPos(Vector2 position)
    {
        ChunkData[] cds = FindObjectsOfType<ChunkData>();
        ChunkData chunkDataSelected = cds.Where(x => x.chunkPosition == position).First();
        foreach(Transform child in chunkDataSelected.transform)
        {
            if(child.GetComponent<Room>() != null)
            {
                if (child.GetComponent<Room>().centerAvailable)
                {
                    return child.gameObject;
                }
            }
        }
        return null;
    }

    public static float CalculateDistance(GameObject object1, GameObject object2)
    {
        return Vector3.Distance(object1.transform.position, object2.transform.position);
    }

    public static bool Chance(int percentage)
    {
        return Random.Range(0, 100) <= percentage;
    }

    public static Chunk GetChunkByCoordinate(Vector2 coordinates)
    {
        Chunk[] chunks = FindObjectsByType<Chunk>(FindObjectsSortMode.None);
        foreach (Chunk chunk in chunks)
        {
            if(chunk.chunkCoordinates.x == coordinates.x && chunk.chunkCoordinates.y == coordinates.y)
            {
                return chunk;
            }
        }
        return null;
    }
    public static float GetPerlinNoiseValue(float x, float y)
    {
        return Mathf.PerlinNoise(x, y);
    }

    public static int GetTotallyRandomNumberBetween(int a, int b)
    {
        System.Random rng = new System.Random();
        return rng.Next(a, b);
    }
}
