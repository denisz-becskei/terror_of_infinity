using System.Collections;
using UnityEngine;
using static GenerationManager;

public class ChunkData : MonoBehaviour
{
    public Vector2 chunkPosition;
    public ChunkType chunkType;
    public Coroutine despawnSafeguard;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        despawnSafeguard = StartCoroutine(DespawnSafeguard());
    }

    IEnumerator DespawnSafeguard()
    {
        yield return new WaitForSeconds(WorldWideScripts.GetTotallyRandomNumberBetween(15, 30));
        if(WorldWideScripts.CalculateDistance(this.gameObject, player) > 49 * 6)
        {
            Destroy(gameObject);
        } else
        {
            despawnSafeguard = StartCoroutine(DespawnSafeguard());
        }
    }

}
