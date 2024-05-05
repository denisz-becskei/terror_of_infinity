using System.Collections;
using UnityEngine;

public class ChunkChecker : MonoBehaviour
{
    public GenerationManager gm;
    private bool isOnCooldown = true;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(DelayedStart());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isOnCooldown && other.gameObject == player)
        {
            gm.ChunkChecker();
        }
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3);
        isOnCooldown = false;
    }
}
