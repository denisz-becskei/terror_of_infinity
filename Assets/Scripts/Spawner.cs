using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] int spawnChance;
    [SerializeField] int spawnProxy;
    [SerializeField] float rotationY;

    private GameObject player;
    private GameObject prefabToSpawn;
    private bool[] spawnerActive = new bool[] { false, false };

    private void Start()
    {
        uint indexToSpawn = (uint)Random.Range(0, prefabs.Length);
        prefabToSpawn = prefabs[indexToSpawn];
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
        spawned.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1f, transform.localPosition.z);
        spawned.transform.rotation = Quaternion.Euler(spawned.transform.rotation.x, spawned.transform.rotation.y + rotationY, spawned.transform.rotation.z);

        Destroy(gameObject, 2f);
    }
}
