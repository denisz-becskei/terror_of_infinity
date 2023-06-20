using System.Collections;
using UnityEngine;

public class ChunkChecker : MonoBehaviour
{
    public GenerationManager gm;
    private bool isOnCooldown = true;

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isOnCooldown)
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
