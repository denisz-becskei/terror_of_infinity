using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using System.Linq;
using System;

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
        ChromaticConondrum
    }

    [SerializeField] GameObject centerMarkerPrefab;

    [SerializeField] GameObject worldGrid;
    [SerializeField] GameObject player;
    [SerializeField] GameObject chunkHandler;

    [SerializeField] NavMeshSurface nms;
    [SerializeField] GameObject enemyController;
    [SerializeField] int blockSize;

    public static List<GameObject> rooms;
    [SerializeField] ChunkType chunkOverride;

    private int checkerRange = 5;
    private int chunkLimit = 36;
    private int baseChunkAmt = 5;

    public GenerationState currentState = GenerationState.Idle;

    private List<Chunk> chunks = new List<Chunk>();
    private GetChunkType gct;
    private PlayerInformation pi;


    private void Start()
    {
        rooms = new List<GameObject>();
        gct = GetComponent<GetChunkType>();
        GenerateStartChunks();
        SpawnPlayer();
        ChunkChecker();

        // TODO:
        // Trigger Enemy
        //Rebake();
        //enemyController.GetComponent<EnemyController>().enemyState = EnemyController.EnemyState.Harass;
    }


    private void SpawnPlayer()
    {
        player.SetActive(false);

        GameObject cleanRoom = WorldWideScripts.GetFirstCleanRoomInChunkByPos(new Vector2(2, 2));
        player.transform.position = new Vector3(cleanRoom.transform.position.x, 3f, cleanRoom.transform.position.z);

        player.SetActive(true);
        pi = player.GetComponent<PlayerInformation>();
        Camera.main.gameObject.SetActive(false);
    }

    private void GenerateChunkAtPosition(float x, float y)
    {
        Chunk chunk = chunkHandler.AddComponent<Chunk>();

        if(chunkOverride == ChunkType.None)
        {
            chunk.Setup(blockSize,
                    gct.GetChunkAtPosition(Mathf.FloorToInt(x), Mathf.FloorToInt(y)),
                    worldGrid,
                    centerMarkerPrefab,
                    enemyController.GetComponent<EnemyController>(),
                    new Vector2(x, y));
        } else
        {
            chunk.Setup(blockSize,
                    gct.OverrideChunkType(chunkOverride),
                    worldGrid,
                    centerMarkerPrefab,
                    enemyController.GetComponent<EnemyController>(),
                    new Vector2(x, y));
        }
        chunk.Generate();
        chunks.Add(chunk);
    }

    private void GenerateStartChunks()
    {
        for (int i = 0; i < baseChunkAmt; i++)
        {
            for (int j = 0; j < baseChunkAmt; j++)
            {
                GenerateChunkAtPosition(i, j);
            }
        }
    }


    public void ChunkChecker()
    {
        // Get the Chunk Coordinates where the player stands
        GameObject[] markers = GameObject.FindGameObjectsWithTag("Marker");
        float[] distances = new float[markers.Length];
        for (int i = 0; i < markers.Length; i++)
        {
            distances[i] = WorldWideScripts.CalculateDistance(player, markers[i]);
        }
        int minIndex = Enumerable.Range(0, distances.Length).Aggregate((a, b) => (distances[a] < distances[b]) ? a : b);

        ChunkData currentChunkData = markers[minIndex].transform.parent.GetComponent<ChunkData>();
        pi.currentChunkType = currentChunkData.chunkType;
        Vector2 currentChunk = currentChunkData.chunkPosition;
        pi.ChunkUpdateAction();


        // If this iteration contains more Chunks than ChunkLimit, then remove 10% of the Chunks that are the farthest from the player
        int originalChunkCount = chunks.Count;
        while (chunks.Count > chunkLimit)
        {
            int countToRemove = Mathf.CeilToInt(originalChunkCount * 0.1f);
            for (int i = 0; i < countToRemove; i++)
            {
                int farthestChunkIndex = Array.IndexOf(distances, distances.Max());
                Vector2 chunkPosition = markers[farthestChunkIndex].transform.parent.GetComponent<ChunkData>().chunkPosition;
                Chunk chunkToDestroy = WorldWideScripts.GetChunkByCoordinate(chunkPosition);
                chunks.Remove(chunkToDestroy);
                chunkToDestroy.DestroySelf();
                distances[farthestChunkIndex] = Mathf.NegativeInfinity;
            }
        }


        // Check if every Chunk is spawned around the player in a ChunkRange x ChunkRange area
        int halfRange = Mathf.FloorToInt(checkerRange / 2f);
        for (int i = -halfRange; i <= halfRange; i++)
        {
            for (int j = -halfRange; j <= halfRange; j++)
            {
                if (i == 0 && j == 0) continue; // Unncecessary to check the chunk the player is standing in
                Vector2 chunkPosition = currentChunk + new Vector2(i, j);
                if (WorldWideScripts.GetChunkByCoordinate(chunkPosition) == null) // If the Chunk doesn't exist
                {
                    GenerateChunkAtPosition(chunkPosition.x, chunkPosition.y); // Generate the Chunk @ that position
                }
            }
        }
    }

    private void Rebake()
    {
        nms.BuildNavMesh();
    }
}
