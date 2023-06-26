using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] Vector3[] prefabScaleValues;
    [SerializeField] int spawnChance;
    [SerializeField] int spawnProxy;

    [SerializeField] Vector3 localRotation;

    private GameObject player;
    private GameObject prefabToSpawn;
    private Vector3 spawnedScale;
    private bool[] spawnerActive = new bool[] { false, false };

    private void Start()
    {
        uint indexToSpawn = (uint)Random.Range(0, prefabs.Length);
        prefabToSpawn = prefabs[indexToSpawn];
        spawnedScale = prefabScaleValues[indexToSpawn];
        player = GameObject.FindGameObjectWithTag("Player");
        if(spawnChance > 0 && WorldWideScripts.Chance(spawnChance)) spawnerActive[0] = true;

        if (!spawnerActive[0]) Destroy(gameObject, 2f);
    }

    private void FixedUpdate()
    {
        if (spawnerActive[0])
        {
            if(WorldWideScripts.CalculateDistance(gameObject, player) < spawnProxy)
            {
                spawnerActive[1] = true;
            }
        }

        if (spawnerActive[0] && spawnerActive[1])
        {
            Spawn();
        }
    }

    void Spawn()
    {
        spawnerActive[0] = false;
        GameObject spawned = Instantiate(prefabToSpawn, transform.parent);
        spawned.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 3f, transform.localPosition.z);
        //spawned.transform.localScale = new Vector3(0.0005f, 0.0025f, 0.0005f);
        spawned.transform.localScale = spawnedScale;
        spawned.transform.localRotation = Quaternion.Euler(localRotation.x, localRotation.y, localRotation.z);
        //spawned.GetComponentInChildren<MeshFilter>().gameObject.transform.localPosition = new Vector3(0.004279251f, 0.02407454f, 0.004279251f);

        Destroy(gameObject, 2f);
    }
}
