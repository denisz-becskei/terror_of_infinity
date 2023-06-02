using System.Collections.Generic;
using UnityEngine;

public class AnimatedTexture : MonoBehaviour
{

    [SerializeField] private Texture2D[] framesBatch1;
    [SerializeField] private Texture2D[] framesBatch2;
    [SerializeField] private float fps = 24f;

    public uint state = 0;

    private Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        int index = (int)(Time.time * fps);
        switch(state)
        {
            case 0:
                index = index % framesBatch1.Length;
                mat.mainTexture = framesBatch1[index];
                break;
            case 1:
                index = index % framesBatch2.Length;
                mat.mainTexture = framesBatch2[index];
                break;
        }
    }
}