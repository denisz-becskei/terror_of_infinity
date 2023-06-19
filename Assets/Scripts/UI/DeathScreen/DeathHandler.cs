using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Camera fpsCamera;
    [SerializeField] Camera eventCamera;

    [SerializeField] VideoShrink vs;
    [SerializeField] SpawnSkulls ss;
    [SerializeField] TypewriterUI twUI;
    [SerializeField] DiedZoopController dzc;
    [SerializeField] FirstPersonMovement fpm;
    [SerializeField] FirstPersonLook fpl;
    [SerializeField] GeneratePrompt gp;

    [SerializeField] GameObject skullContainer;

    [SerializeField] VideoPlayer vp;

    [SerializeField] Animator diedZoopAnimator;
    [SerializeField] Animator cbAnimator;
    [SerializeField] Animator causeAnimator;

    private GameObject uiContainer;

    private void Start()
    {
        uiContainer = GameObject.FindGameObjectWithTag("UIContainer");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartDeathSequence("KILLED BY CONSOLE");
        }
    }

    public void StartDeathSequence(string deathCause)
    {
        fpsCamera.enabled = false;
        eventCamera.enabled = true;

        causeAnimator.GetComponent<TMP_Text>().text = deathCause;

        fpm.ToggleMovement();
        vp.Play();
        vs.StartVideoShrink();
        gp.Generate();
        twUI.StartTypewrite();
        dzc.StartDelayedAnimation();
        ss.StartSkullSpawns();
    }

    public void FinishDeathSequence()
    {
        eventCamera.enabled = false;
        fpsCamera.enabled = true;

        fpm.ToggleMovement();
        fpl.ToggleCursorLocked(CursorLockMode.Locked);

        foreach(Transform child in skullContainer.transform)
        {
            Destroy(child.gameObject);
        }

        vs.GetComponent<Animator>().Play("VideoCanvasIdle");
        uiContainer.GetComponent<Animator>().Play("UIContainerIdle");
        diedZoopAnimator.Play("GeneralAnimationZoopIdle");
        causeAnimator.Play("GeneralAnimationZoopIdle");
        cbAnimator.Play("ButtonFadeIdle");
        ss.SetSkullsToGenerate(ss.GetSkullsToGenerate() - 1);

        // TODO: Respawn Player Elsewhere
    }
}
