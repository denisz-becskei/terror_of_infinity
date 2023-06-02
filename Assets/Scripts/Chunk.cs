using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GenerationManager;

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
    private GameObject emptyRoomPrefab, lightPrefab, centerMarkerPrefab, worldGrid;
    private List<GameObject> roomPrefabs;
    private EnemyController ec;

    private GenerationState currentState = GenerationState.Idle;

    public Vector2 chunkCoordinates = new Vector2();
    private GameObject chunkContainer;
    private GenerationManager gm;

    private List<GameObject> currentChunkRooms;


    public void Setup(int chunkSize, ChunkTypeScriptableObject chunkType, GameObject worldGrid,
        GameObject centerMarkerPrefab, EnemyController ec, Vector2 chunkCoordinates)
    {
        this.chunkSize = chunkSize;
        this.mapBrightness = chunkType.mapBrightness;
        this.mapEmptiness = chunkType.mapEmptiness;
        this.type = chunkType.chunkType;
        this.emptyRoomPrefab = chunkType.emptyRoomPrefab;
        this.centerMarkerPrefab = centerMarkerPrefab;
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

        for (int state = 0; state < 3; state++)
        {
            for (int i = 0; i < chunkSize; i++)
            {
                if (posTracker == chunkSizeSqrt)
                {
                    currentPosX = 0;
                    currentPosZ += roomSize;
                    posTracker = 0;
                }

                currentPos = new Vector3(currentPosX, 0, currentPosZ);
                switch (currentState)
                {
                    case GenerationState.GeneratingRooms:
                        GameObject newRoom = Instantiate(roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Count)], chunkContainer.transform);
                        newRoom.transform.localPosition = currentPos;
                        newRoom.transform.rotation = Quaternion.identity;
                        rooms.Add(newRoom);
                        currentChunkRooms.Add(newRoom);

                        break;
                    case GenerationState.GeneratingLights:
                        if(lightPrefab == null)
                        {
                            break;
                        } else
                        {
                            int lightSpawn = UnityEngine.Random.Range(-1, mapBrightness);
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

        GameObject center = Instantiate(centerMarkerPrefab, chunkContainer.transform);
        center.transform.localPosition = new Vector3(chunkSize / 2 - roomSize, 12f, chunkSize / 2 - roomSize);
        center.transform.parent.AddComponent<ChunkData>();
        center.transform.parent.GetComponent<ChunkData>().chunkPosition = chunkCoordinates;
        center.transform.parent.GetComponent<ChunkData>().chunkType = type;
        center.tag = "Marker";
    }

    public GameObject GetParent()
    {
        return chunkContainer;
    }

    public void DestroySelf()
    {
        currentChunkRooms.Clear();

        foreach (Transform children in chunkContainer.transform)
        {
            if (children.GetComponent<Room>() != null)
            {
                GenerationManager.rooms.Remove(children.gameObject);
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
