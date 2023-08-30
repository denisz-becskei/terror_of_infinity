using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] float spawnChance;
    [SerializeField] int spawnProxy;
    
    [SerializeField] bool randomRotation;
    [SerializeField] bool randomScale;

    [SerializeField] float[] rotations;
    [SerializeField] float[] scales;

    [SerializeField] private float respawnTimer = 0;

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
            spawnerActive[1] = false;
        }
    }

    void Spawn()
    {
        spawnerActive[0] = false;
        GameObject spawned = Instantiate(prefabToSpawn);
        if (randomRotation) {
            spawned.transform.rotation = Quaternion.Euler(spawned.transform.rotation.x, spawned.transform.rotation.y + rotations[Random.Range(0, rotations.Length)], spawned.transform.rotation.z);
        } else
        {
            spawned.transform.rotation = Quaternion.Euler(spawned.transform.rotation.x, spawned.transform.rotation.y + rotations[0], spawned.transform.rotation.z);
        }

        if (randomScale)
        {
            float randomScale = scales[Random.Range(0, scales.Length)];
            spawned.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
        }
        else
        {
            spawned.transform.localScale = new Vector3(scales[0], scales[0], scales[0]);
        }

        
        spawned.transform.parent = transform.parent;
        spawned.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1f, transform.localPosition.z);

        if (respawnTimer == 0)
        {
            Destroy(gameObject, 2f);
        }
        else
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTimer);
        Spawn();
    }
}
