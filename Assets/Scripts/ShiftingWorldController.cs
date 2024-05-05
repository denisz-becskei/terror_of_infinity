using System.Collections;
using UnityEngine;

public class ShiftingWorldController : MonoBehaviour
{
    private ShiftingBlock block;
    private GameObject player;

    private void Start()
    {
        block = gameObject.GetComponentInChildren<ShiftingBlock>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Shift());
    }

    IEnumerator Shift()
    {
        float distance = WorldWideScripts.CalculateDistance(player, gameObject);
        yield return new WaitForSeconds(Random.Range(3f, 6f));

        if (distance < 30)
        {
            block.Shift();
        }
        StartCoroutine(Shift());
    }
}
