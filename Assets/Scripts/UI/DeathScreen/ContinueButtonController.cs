using UnityEngine;

public class ContinueButtonController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    private Animator uiContainerAnimator;
    private GameObject skullContainer;
    

    private void Start()
    {
        uiContainerAnimator = GameObject.FindGameObjectWithTag("UIContainer").GetComponent<Animator>();
        skullContainer = GameObject.FindGameObjectWithTag("SkullContainer");
        audioSource = GameObject.FindGameObjectWithTag("ContinueButton").GetComponent<AudioSource>();
    }

    public void Continue()
    {
        uiContainerAnimator.Play("UIContainerShrink");
        audioSource.Play();
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
