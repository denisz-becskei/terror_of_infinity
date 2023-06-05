using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySkull : MonoBehaviour
{
    public void Destroy(uint index)
    {
        Transform[] skulls = GetComponentsInChildren<Transform>();
        skulls[index].GetComponent<Explode2DSprite>().Explode();
    }
}
