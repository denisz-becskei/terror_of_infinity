using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoShrink : MonoBehaviour
{
    [SerializeField] RenderTexture rt;
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartVideoShrink()
    {
        StartCoroutine(DelayShrink());
    }

    IEnumerator DelayShrink()
    {
        yield return new WaitForSeconds(DeathLength.DEATH_LENGTH);
        animator.Play("ShrinkVideo");
        yield return new WaitForSeconds(2f);
        rt.Release();
    }
}
