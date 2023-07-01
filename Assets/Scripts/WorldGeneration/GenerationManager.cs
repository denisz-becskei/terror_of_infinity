using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using System.Linq;
using static IntVector2Init;

public class GenerationManager : MonoBehaviour
{
    public enum GenerationState
    {
        Idle,
        GeneratingRooms,
        GeneratingLights,
        GeneratingMainRoom
    }

    public enum ChunkType
    {
        None,
        Entrance,
        EntangledDarkness,
        MadnessInRed,
        TheWarehouse,
        HazeOfDeath,
        ColdForest,
        PossessedTeddies,
        LabyrinthOfBlindness,
        TheSimulation,
        InfinityMaze,
        ShiftingWorld,
        InfiniteNightSky,
        TheRottingCrypts,
        GateToReality,
        ChromaticConondrum,
        BrightOfAngels,
        DimensionalRift,

        Purgatory
    }

    [SerializeField] GameObject worldGrid;
    [SerializeField] GameObject player;
    [SerializeField] GameObject chunkHandler;

    [SerializeField] NavMeshSurface nms;
    [SerializeField] GameObject enemyController;
    [SerializeField] int blockSize;

    public static List<GameObject> rooms;
    [SerializeField] ChunkType chunkOverride;

    private readonly int checkerRange = 5;
    private readonly int chunkLimit = 36;
    private readonly int baseChunkAmt = 5;

    public int chunksGenerated = 0;

    public GenerationState currentState = GenerationState.Idle;

    private List<Chunk> chunks;
    private GetChunkType gct;
    private PlayerInformation pi;

    private bool navMeshBuildConcluded = false;

    private Coroutine navMeshUpdateRoutine;


    private void Start()
    {
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        chunks = new List<Chunk>();

        rooms = new List<GameObject>();
        gct = GetComponent<GetChunkType>();

        GenerateStartChunks();
        SpawnPlayer();
        ChunkChecker();

        // TODO:
        // Trigger Enemy
        Rebake("Build");
        //if(navMeshUpdateRoutine == null)
        //{
        //    navMeshUpdateRoutine = StartCoroutine(UpdateNavMesh());
        //}

        //enemyController.GetComponent<EnemyController>().enemyState = EnemyController.EnemyState.Harass;
    }


    private void SpawnPlayer()
    {
        player.SetActive(false);

        GameObject cleanRoom = WorldWideScripts.GetFirstCleanRoomInChunkByPos(new Vector2(2, 2));
        player.transform.position = new Vector3(cleanRoom.transform.position.x, 3f, cleanRoom.transform.position.z);

        player.SetActive(true);
        pi = player.GetComponent<PlayerInformation>();
        //Camera.main.gameObject.SetActive(false);
    }

    private void GenerateChunkAtPosition(IntVector2 position)
    {
        Chunk chunk = chunkHandler.AddComponent<Chunk>();

        if (chunkOverride == ChunkType.None)
        {
            chunk.Setup(blockSize,
                    gct.GetChunkAtPosition(position.x, position.y),
                    worldGrid,
                    enemyController.GetComponent<EnemyController>(),
                    position);
        }
        else
        {
            chunk.Setup(blockSize,
                    gct.OverrideChunkType(chunkOverride),
                    worldGrid,
                    enemyController.GetComponent<EnemyController>(),
                    position);
        }
        chunk.Generate();
        ChunkMap.SetValue(position, WorldWideScripts.ChunkTypeToAbbrString(chunk.GetChunkType()));
        chunks.Add(chunk);
        chunksGenerated++;
    }

    private void GenerateStartChunks()
    {
        for (int i = 0; i < baseChunkAmt; i++)
        {
            for (int j = 0; j < baseChunkAmt; j++)
            {
                GenerateChunkAtPosition(new IntVector2(i, j));
            }
        }
    }


    public void ChunkChecker()
    {
        int currentChunkCount = ChunkMap.GetMapLength();
        Dictionary<IntVector2, float> distances = new Dictionary<IntVector2, float>();
        IntVector2 playerPosition = WorldWideScripts.GetPositionByString(pi.playerCurrentWorldPosition)[0];
        foreach(KeyValuePair<IntVector2, string> chunk in ChunkMap.GetMap())
        {
            distances[chunk.Key] = playerPosition.DistanceBetweenPoints(chunk.Key);
        }
        var orderedDistances = distances.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        IntVector2 currentChunk = orderedDistances.First().Key;
        pi.currentChunkType = WorldWideScripts.GetChunkByCoordinate(currentChunk).GetChunkType();
        pi.ChunkUpdateAction();

        if (navMeshBuildConcluded)
        {
            Rebake("Update");
        }

        // If this iteration contains more Chunks than ChunkLimit, then remove 10% of the Chunks that are the farthest from the player
        int originalChunkCount = currentChunkCount;
        int chunkCountBeforeDeletion = currentChunkCount;
        while (chunkCountBeforeDeletion > chunkLimit)
        {
            int countToRemove = Mathf.CeilToInt(originalChunkCount * 0.1f);
            for (int i = 0; i < countToRemove; i++)
            {
                IntVector2 chunkPosition = orderedDistances.Last().Key;
                Chunk chunkToDestroy = WorldWideScripts.GetChunkByCoordinate(chunkPosition);
                chunks.Remove(chunkToDestroy);
                ChunkMap.RemoveIndex(chunkPosition);
                orderedDistances.Remove(chunkPosition);

                chunkToDestroy.DestroySelf();
            }
            chunkCountBeforeDeletion -= countToRemove;
            chunksGenerated = ChunkMap.GetMapLength();
        }


        // Check if every Chunk is spawned around the player in a ChunkRange x ChunkRange area
        int halfRange = Mathf.FloorToInt(checkerRange / 2f);
        for (int i = -halfRange; i <= halfRange; i++)
        {
            for (int j = -halfRange; j <= halfRange; j++)
            {
                if (i == 0 && j == 0) continue; // Unnecessary to check the chunk the player is standing in
                IntVector2 chunkPosition = currentChunk + new IntVector2(i, j);
                if (ChunkMap.GetValue(new IntVector2(chunkPosition.x, chunkPosition.y)) == "0") // If the Chunk doesn't exist
                {
                    GenerateChunkAtPosition(new IntVector2(chunkPosition.x, chunkPosition.y)); // Generate the Chunk @ that position
                }
            }
        }
        //Debug.Log("Running ChunkChecker(TM)");
    }

    public void Rebake(string action)
    {
        switch (action)
        {
            case "Build":
                nms.BuildNavMesh();
                navMeshBuildConcluded = true;
                break;
            case "Update":
                nms.UpdateNavMesh(nms.navMeshData);
                break;
            default:
                Debug.LogError("HOLD UP! WE ARE NOT SUPPOSED TO BE HERE");
                break;
        }
    }
}
