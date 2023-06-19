using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkulls : MonoBehaviour
{
    [SerializeField] GameObject skullPrefab;
    [SerializeField] int skullsToGenerate = 10;

    public void StartSkullSpawns()
    {
        StartCoroutine(DelayedSkullSpawns());
    }

    public int GetSkullsToGenerate()
    {
        return skullsToGenerate;
    }

    public void SetSkullsToGenerate(int skullsToGenerate)
    {
        this.skullsToGenerate = skullsToGenerate;
    }

    IEnumerator DelayedSkullSpawns()
    {
        yield return new WaitForSeconds(10f);
        for (int i = -5; i < skullsToGenerate - 5; i++)
        {
            GameObject skull = Instantiate(skullPrefab, transform);
            skull.transform.localPosition = new Vector3(0.04f + i * 0.08f, 0f, 0f);
            skull.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            skull.transform.localRotation = Quaternion.Euler(90f, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
