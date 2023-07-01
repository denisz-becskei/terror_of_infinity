using System.Collections.Generic;
using UnityEngine;
using static GenerationManager;
using static IntVector2Init;

public class Chunk : MonoBehaviour
{
    // Information from the Controller
    private int chunkSize;
    private int mapBrightness;
    private int mapEmptiness;
    private ChunkType type;

    // Information for generation helping
    private Vector3 currentPos;
    private int currentRoom;
    private float currentPosX, currentPosZ;
    private float roomSize = 7;
    private int posTracker;
    private int chunkSizeSqrt;

    // GameObjects from Controller
    private GameObject emptyRoomPrefab, lightPrefab, worldGrid;
    private List<GameObject> roomPrefabs;
    private EnemyController ec;

    private GenerationState currentState = GenerationState.Idle;

    public IntVector2 chunkCoordinates = new IntVector2();
    private GameObject chunkContainer;
    private GenerationManager gm;

    private List<GameObject> currentChunkRooms;
    private int? GAME_SEED = null;

    private void Awake()
    {
        while(GAME_SEED == null)
        {
            GAME_SEED = GameObject.FindGameObjectWithTag("DataPersistance").GetComponent<DataPersistanceManager>().GetGameState().GAME_SEED;
        }
        Random.InitState((int)GAME_SEED);
    }

    public void Setup(int chunkSize, ChunkTypeScriptableObject chunkType, GameObject worldGrid, EnemyController ec, IntVector2 chunkCoordinates)
    {
        this.chunkSize = chunkSize;
        this.mapBrightness = chunkType.mapBrightness;
        this.mapEmptiness = chunkType.mapEmptiness;
        this.type = chunkType.chunkType;
        this.emptyRoomPrefab = chunkType.emptyRoomPrefab;
        this.worldGrid = worldGrid;
        this.roomPrefabs = new List<GameObject>(chunkType.roomPrefabs);

        this.lightPrefab = chunkType.lightPrefab;
        this.ec = ec;
        this.chunkCoordinates = chunkCoordinates;

        chunkSizeSqrt = (int)Mathf.Sqrt(chunkSize);
        gm = FindFirstObjectByType<GenerationManager>();
        currentChunkRooms = new List<GameObject>();
    }

    public void Generate()
    {
        GenerateChunkContainer();

        for (int i = 0; i < mapEmptiness; i++)
        {
            roomPrefabs.Add(emptyRoomPrefab);
        }

        int roomCoordX = 0, roomCoordY = 0;

        for (int state = 0; state < 3; state++)
        {
            for (int i = 0; i < chunkSize; i++)
            {

                if (posTracker == chunkSizeSqrt)
                {
                    currentPosX = 0;
                    currentPosZ += roomSize;
                    posTracker = 0;
                    if(currentState == GenerationState.GeneratingRooms)
                    {
                        roomCoordX = 0;
                        roomCoordY++;
                    }
                }

                currentPos = new Vector3(currentPosX, 0, currentPosZ);
                switch (currentState)
                {
                    case GenerationState.GeneratingRooms:
                        GameObject newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)], chunkContainer.transform);
                        RoomPosition rp = newRoom.AddComponent<RoomPosition>();
                        rp.ContainerChunkPosition = chunkContainer.GetComponent<ChunkData>();
                        rp.RoomPositionInContainer = new Vector2(roomCoordX, roomCoordY);
                        newRoom.transform.localPosition = currentPos;
                        newRoom.transform.rotation = Quaternion.identity;
                        rooms.Add(newRoom);
                        currentChunkRooms.Add(newRoom);
                        roomCoordX++;

                        break;
                    case GenerationState.GeneratingLights:
                        if(lightPrefab == null)
                        {
                            break;
                        } else
                        {
                            int lightSpawn = Random.Range(-1, mapBrightness);
                            if (lightSpawn == 0 && currentChunkRooms[currentRoom].GetComponent<Room>().centerAvailable)
                            {
                                GameObject newLight = Instantiate(lightPrefab, chunkContainer.transform);
                                newLight.transform.localPosition = currentPos;
                                newLight.transform.rotation = Quaternion.identity;
                            }
                            break;
                        }
                }
                posTracker++;
                currentRoom++;
                currentPosX += roomSize;
            }
            NextState();
        }
    }

    private void NextState()
    {
        currentState++;

        currentRoom = 0;
        currentPosX = 0;
        currentPosZ = 0;
        currentPos = Vector3.zero;
        posTracker = 0;
    }

    private void GenerateChunkContainer()
    {
        chunkContainer = new GameObject("Chunk " + chunkCoordinates.x.ToString() + " ; " + chunkCoordinates.y.ToString());
        chunkContainer.transform.position = new Vector3(chunkCoordinates.x * chunkSizeSqrt * roomSize, 0, chunkCoordinates.y * chunkSizeSqrt * roomSize);
        chunkContainer.transform.parent = worldGrid.transform;

        chunkContainer.AddComponent<BoxCollider>();
        chunkContainer.GetComponent<BoxCollider>().size = new Vector3(56, 10, 56);
        chunkContainer.GetComponent<BoxCollider>().center = new Vector3(24, 5, 24);
        chunkContainer.GetComponent<BoxCollider>().isTrigger = true;
        chunkContainer.AddComponent<ChunkChecker>().gm = gm;

        chunkContainer.AddComponent<ChunkData>();
        chunkContainer.GetComponent<ChunkData>().chunkType = type;
        chunkContainer.GetComponent<ChunkData>().ChunkPositionInWorld = new IntVector2(chunkCoordinates.x, chunkCoordinates.y);
    }

    public GameObject GetParent()
    {
        return chunkContainer;
    }

    public ChunkType GetChunkType()
    {
        return type;
    }

    public void DestroySelf()
    {
        currentChunkRooms.Clear();
        if (chunkContainer == null) return;

        foreach (Transform children in chunkContainer.transform)
        {
            if (children == null) continue;

            if (children.GetComponent<Room>() != null)
            {
                rooms.Remove(children.gameObject);
            }

            Destroy(children.gameObject);
        }

        Destroy(chunkContainer);
        Destroy(this);
    }

    public string GetCoordinates()
    {
        return "X : " + chunkCoordinates.x.ToString() + " ; Y : " + chunkCoordinates.y.ToString();
    }
}
