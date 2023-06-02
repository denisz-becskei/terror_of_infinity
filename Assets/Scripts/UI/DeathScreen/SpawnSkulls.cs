using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkulls : MonoBehaviour
{
    [SerializeField] GameObject skullPrefab;

    void Start()
    {

        for(int i = -5; i < 5; i++)
        {
            GameObject skull = Instantiate(skullPrefab, transform);
            skull.transform.localPosition = new Vector3(0.64f + i * 0.15f, 0.011f, -0.33f);
            skull.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            skull.transform.localRotation = Quaternion.Euler(90f, 0, 0);
        }
    }
}
