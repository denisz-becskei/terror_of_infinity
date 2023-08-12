using System.Collections;
using UnityEngine;

public class VideoShrink : MonoBehaviour
{
    [SerializeField] RenderTexture rt;
    [SerializeField] RenderTexture rt_gameOver;
    private Animator animator;
    [SerializeField] private bool isGameOver;
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
        if (!isGameOver) yield return new WaitForSeconds(DeathLength.DEATH_LENGTH);
        else yield return new WaitForSeconds(DeathLength.GAME_OVER_LENGTH);
        if (!this.isGameOver) animator.Play("ShrinkVideo");
        else this.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        if(!this.isGameOver) rt.Release();
        else rt_gameOver.Release();
    }
}
