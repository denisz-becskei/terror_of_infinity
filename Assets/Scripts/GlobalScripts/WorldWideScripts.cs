using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static GenerationManager;
using static IntVector2Init;

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

        foreach (GameObject room in GenerationManager.rooms)
        {
            if (Vector3.Distance(compare.position, room.transform.position) > furthestDistance && room.GetComponent<Room>().centerAvailable)
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
        while (!ready)
        {
            cleanRoom = GenerationManager.rooms[Random.Range(0, GenerationManager.rooms.Count)];
            if (cleanRoom.GetComponent<Room>().centerAvailable) {
                ready = true;
            }
        }
        return cleanRoom;
    }

    public static GameObject GetFirstCleanRoomInChunkByPos(IntVector2 position)
    {
        ChunkData[] cds = FindObjectsOfType<ChunkData>();
        ChunkData chunkDataSelected = cds.Where(x => x.ChunkPositionInWorld == position).First();
        foreach (Transform child in chunkDataSelected.transform)
        {
            if (child.GetComponent<Room>() != null)
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
        return Vector3Distance(object1.transform.position, object2.transform.position);
    }

    private static float Vector3Distance(Vector3 a, Vector3 b)
    {
        float num1 = a.x - b.x;
        float num2 = a.y - b.y;
        float num3 = a.z - b.z;
        return Mathf.Sqrt(num1 * num1 + num2 * num2 + num3 * num3);
    }

    public static bool Chance(int percentage)
    {
        return Random.Range(0, 100) <= percentage;
    }

    public static Chunk GetChunkByCoordinate(IntVector2 coordinates)
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

    public static string ChunkTypeToAbbrString(ChunkType chunkType)
    {
        switch(chunkType)
        {
            case ChunkType.Entrance:
                return "E";
            case ChunkType.EntangledDarkness:
                return "ED";
            case ChunkType.MadnessInRed:
                return "MiR";
            case ChunkType.TheWarehouse:
                return "TW";
            case ChunkType.HazeOfDeath:
                return "HoD";
            case ChunkType.ColdForest:
                return "CF";
            case ChunkType.PossessedTeddies:
                return "PT";
            case ChunkType.LabyrinthOfBlindness:
                return "LoB";
            case ChunkType.TheSimulation:
                return "TS";
            case ChunkType.InfinityMaze:
                return "IM";
            case ChunkType.ShiftingWorld:
                return "SW";
            case ChunkType.InfiniteNightSky:
                return "INS";
            case ChunkType.TheRottingCrypts:
                return "TRC";
            case ChunkType.GateToReality:
                return "GTR";
            case ChunkType.ChromaticConondrum:
                return "CC";
            case ChunkType.BrightOfAngels:
                return "BoA";
            case ChunkType.DimensionalRift:
                return "DR";
            default:
                return "0";
        }
    }

    public static IntVector2[] GetPositionByString(string positionString)
    {
        string[] positionsSplitToChunksAndRooms = positionString.Split('-');
        string[] chunkPositionString = positionsSplitToChunksAndRooms[0].Split(':');
        string[] roomPositionString = positionsSplitToChunksAndRooms[1].Split(':');

        return new IntVector2[]
        {
            new IntVector2(int.Parse(chunkPositionString[0]), int.Parse(chunkPositionString[1])),
            new IntVector2(int.Parse(roomPositionString[0]), int.Parse(roomPositionString[1])),
        };
    }
}
