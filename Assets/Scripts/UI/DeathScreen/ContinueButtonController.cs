using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] FirstPersonLook fpl;

    private DeathHandler dh;
    private AudioSource audioSource;
    private Animator uiContainerAnimator;
    private GameObject skullContainer;
    
    
    private void Start()
    {
        uiContainerAnimator = GameObject.FindGameObjectWithTag("UIContinueContainer").GetComponent<Animator>();
        skullContainer = GameObject.FindGameObjectWithTag("SkullContainer");
        audioSource = transform.parent.GetComponent<AudioSource>();
        dh = GameObject.FindGameObjectWithTag("GameController").GetComponent<DeathHandler>();
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
        StartCoroutine(DelayFinishSequence());
    }

    public void ReturnToMainMenu()
    {
        // TODO: RETURN TO MAIN MENU
        Debug.Log("Returning to Main Menu");
    }

    public void Animate()
    {
        fpl.ToggleCursorLocked(CursorLockMode.None);
        animator.Play("ButtonFade");
    }

    IEnumerator DelayFinishSequence()
    {
        yield return new WaitForSeconds(1.25f);
        dh.FinishDeathSequence();
    }
}
