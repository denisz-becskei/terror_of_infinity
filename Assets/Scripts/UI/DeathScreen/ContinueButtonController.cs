using UnityEngine;

public class ContinueButtonController : MonoBehaviour
{
    [SerializeField] Animator animator;
    private Animator uiContainerAnimator;
    private GameObject skullContainer;

    private void Start()
    {
        uiContainerAnimator = GameObject.FindGameObjectWithTag("UIContainer").GetComponent<Animator>();
        skullContainer = GameObject.FindGameObjectWithTag("SkullContainer");
    }

    public void Continue()
    {
        uiContainerAnimator.Play("UIContainerShrink");
        Animator[] animators = skullContainer.GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
        {
            animator.Play("SkullAnimationZoopOut");
        }
    }

    public void Animate()
    {
        animator.Play("ButtonFade");
    }
}
