using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffAnimationIfFar : MonoBehaviour
{
    private Transform player;
    private bool statusOn = true;
    [SerializeField] float maxDistance = 28f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if(TryGetComponent<Outline>(out Outline ol))
        {
            ol.OutlineColor = new Color(0.259f, 0.627f, 0.961f);
        }

    }

    //void FixedUpdate()
    //{
    //    if (statusOn)
    //    {
    //        if (WorldWideScripts.CalculateDistance(gameObject, player.gameObject) >= maxDistance)
    //        {
    //            statusOn = false;
    //            GetComponent<Animator>().enabled = false;
    //        }
    //    }
    //    else
    //    {
    //        if (WorldWideScripts.CalculateDistance(gameObject, player.gameObject) < maxDistance)
    //        {
    //            statusOn = true;
    //            GetComponent<Animator>().enabled = true;
    //        }
    //    }
    //}
}
