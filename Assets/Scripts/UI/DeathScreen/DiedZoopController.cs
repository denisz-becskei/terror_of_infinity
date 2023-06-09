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

    public void StartDelayedAnimation()
    {
        StartCoroutine(DelayedAnimationStart());
        StartCoroutine(DelayedCauseAnimationStart());
    }

    IEnumerator DelayedAnimationStart()
    {
        yield return new WaitForSeconds(DeathLength.DEATH_LENGTH);
        animator.Play("GeneralAnimationZoop");
    }

    IEnumerator DelayedCauseAnimationStart()
    {
        yield return new WaitForSeconds(DeathLength.DEATH_LENGTH + 0.75f);
        causeAnimator.Play("GeneralAnimationZoop");
    }
}
