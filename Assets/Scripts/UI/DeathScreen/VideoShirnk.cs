using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoShirnk : MonoBehaviour
{
    [SerializeField] RenderTexture rt;
    private Animator animator;
    private void Start()
    {
        StartCoroutine(DelayShrink());
        animator = GetComponent<Animator>();
    }

    IEnumerator DelayShrink()
    {
        yield return new WaitForSeconds(10f);
        animator.Play("ShrinkVideo");
        yield return new WaitForSeconds(2f);
        rt.Release();
    }
}
