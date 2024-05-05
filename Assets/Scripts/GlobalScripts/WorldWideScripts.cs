using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GenerationManager;
using static TypeInit;

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

    public static Dictionary<int, string> chunkTypesByInt = new Dictionary<int, string>()
    {
        {1, "Entrance" },
        {2, "EntangledDarkness" },
        {3, "MadnessInRed" },
        {4, "TheWarehouse" },
        {5, "HazeOfDeath" },
        {6, "ColdForest" },
        {7, "PossessedTeddies" },
        {8, "LabyrinthOfBlindness" },
        {9, "TheSimulation" },
        {10, "InfinityMaze" },
        {11, "ShiftingWorld" },
        {12, "InfiniteNightSky" },
        {13, "TheRottingCrypts" },
        {14, "GateToReality" },
        {15, "ChromaticConondrum" },
        {16, "BrightOfAngels" },
        {17, "DimensionalRift" },
        {18, "GuardingFlame" }
    };

    public static List<string> objectiveTypes = new List<string>()
    {
        "BLACK", "BROWN", "ORANGE", "PURPLE", "MAGENTA", "BLUE", "GREEN", "YELLOW", "RED", "WHITE"
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
            if (cleanRoom.GetComponent<Room>().centerAvailable)
            {
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

    public static bool Chance(float percentage)
    {
        return Random.Range(0f, 100f) <= percentage;
    }

    public static Chunk GetChunkByCoordinate(IntVector2 coordinates)
    {
        Chunk[] chunks = FindObjectsByType<Chunk>(FindObjectsSortMode.None);
        foreach (Chunk chunk in chunks)
        {
            if (chunk.chunkCoordinates.x == coordinates.x && chunk.chunkCoordinates.y == coordinates.y)
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

    public static int GetPureRandomNumberBetween(int a, int b)
    {
        System.Random rng = new System.Random();
        return rng.Next(a, b);
    }

    public static string ChunkTypeToAbbrString(ChunkType chunkType)
    {
        switch (chunkType)
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
            case ChunkType.GuardingFlame:
                return "GF";
            default:
                return "0";
        }
    }

    public static IntVector2[] GetPositionByString(string positionString)
    {
        string[] positionsSplitToChunksAndRooms = positionString.Split("::");
        string[] chunkPositionString = positionsSplitToChunksAndRooms[0].Split(':');
        string[] roomPositionString = positionsSplitToChunksAndRooms[1].Split(':');

        return new IntVector2[]
        {
            new IntVector2(int.Parse(chunkPositionString[0]), int.Parse(chunkPositionString[1])),
            new IntVector2(int.Parse(roomPositionString[0]), int.Parse(roomPositionString[1])),
        };
    }

    public static AgentScriptableObject GetAgentByChunkType(ChunkType chunkType, AgentTypeContainerScriptableObject agentTypes)
    {
        switch (chunkType)
        {
            case ChunkType.Entrance:
            case ChunkType.EntangledDarkness:
            case ChunkType.MadnessInRed:
                return agentTypes.agentTypes[0];
            case ChunkType.PossessedTeddies:
                return agentTypes.agentTypes[1];
            case ChunkType.ShiftingWorld:
            case ChunkType.TheSimulation:
            case ChunkType.GateToReality:
                return agentTypes.agentTypes[2];
            case ChunkType.TheRottingCrypts:
            case ChunkType.TheWarehouse:
                return agentTypes.agentTypes[3];
            case ChunkType.HazeOfDeath:
            case ChunkType.ColdForest:
                return agentTypes.agentTypes[4];
            case ChunkType.InfiniteNightSky:
            case ChunkType.LabyrinthOfBlindness:
                return agentTypes.agentTypes[5];
            case ChunkType.ChromaticConondrum:
            case ChunkType.BrightOfAngels:
                return agentTypes.agentTypes[6];
            case ChunkType.GuardingFlame:
                return agentTypes.agentTypes[7];
            case ChunkType.DimensionalRift:
            case ChunkType.InfinityMaze:
                return agentTypes.agentTypes[8];
                //TODO: Add Glitch
        }
        return null;
    }

    public static Vector3 ModifyV3(Vector3 input, float modx, float mody, float modz)
    {
        return new Vector3(input.x + modx, input.y + mody, input.z + modz);
    }

    public static string GetObjectiveTypeByValue(float value)
    {
        for (int i = 0; i < objectiveTypes.Count; i++)
        {
            if (i * 0.1f >= value && value <= i * 0.1f + 0.1f)
            {
                return objectiveTypes[i];
            }
        }

        return null;
    }

    public static Color ToColor(string color)
    {
        switch (color)
        {
            case "black":
                return new Color(0, 0, 0, 1);
            case "brown":
                return new Color(0.4f, 0, 0, 1);
            case "orange":
                return new Color(1, 0.5f, 0, 1);
            case "purple":
                return new Color(0.4f, 0, 0.4f, 1);
            case "magenta":
                return new Color(1, 0, 1, 1);
            case "blue":
                return new Color(0, 0, 1, 1);
            case "green":
                return new Color(0, 1, 0, 1);
            case "yellow":
                return new Color(1, 1, 0, 1);
            case "red":
                return new Color(1, 0, 0, 1);
            case "white":
                return new Color(1, 1, 1, 1);
            default:
                return new Color(1, 1, 1, 1);
        }
        return new Color(1, 1, 1, 1);
    }

    public static int GetValueOfColor(string color)
    {
        switch (color)
        {
            case "black":
                return 100;
            case "brown":
                return 200;
            case "orange":
                return 300;
            case "purple":
                return 400;
            case "magenta":
                return 500;
            case "blue":
                return 600;
            case "green":
                return 700;
            case "yellow":
                return 800;
            case "red":
                return 900;
            case "white":
                return 1000;
            default:
                return 0;
        }
        return 0;
    }
}
