using System.Collections;
using UnityEngine;

public class DiedZoopController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] Animator causeAnimator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartDelayedAnimation(bool isGameOver = false)
    {
        StartCoroutine(DelayedAnimationStart(isGameOver));
        if(!isGameOver) StartCoroutine(DelayedCauseAnimationStart());
    }

    IEnumerator DelayedAnimationStart(bool isGameOver)
    {
        Debug.Log("isCalled");
        if(!isGameOver) yield return new WaitForSeconds(DeathLength.DEATH_LENGTH);
        else yield return new WaitForSeconds(DeathLength.GAME_OVER_LENGTH);
        animator.Play("GeneralAnimationZoop");
    }

    IEnumerator DelayedCauseAnimationStart()
    {
        yield return new WaitForSeconds(DeathLength.DEATH_LENGTH + 0.75f);
        causeAnimator.Play("GeneralAnimationZoop");
    }
}
