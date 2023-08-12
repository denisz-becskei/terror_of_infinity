using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class DeathHandler : MonoBehaviour, IDataPersistance
{
    [SerializeField] Camera fpsCamera;
    [SerializeField] Camera eventCamera;

    [SerializeField] VideoShrink vs_death;
    [SerializeField] VideoShrink vs_gameOver;
    [SerializeField] SpawnSkulls ss;
    [SerializeField] TypewriterUI twUI;
    [FormerlySerializedAs("dzc")] [SerializeField] DiedZoopController dzc_death;
    [SerializeField] DiedZoopController dzc_gameOver;
    [SerializeField] FirstPersonMovement fpm;
    [SerializeField] FirstPersonLook fpl;
    [SerializeField] GeneratePrompt gp;
    [SerializeField] PlayerInformation pi;
    
    [SerializeField] GameObject worldGrid;
    [SerializeField] GenerationManager gm;

    [SerializeField] GameObject skullContainer;

    [SerializeField] VideoPlayer vp_death;
    [SerializeField] VideoPlayer vp_gameOver;

    [SerializeField] Animator diedZoopAnimator;
    [SerializeField] Animator cbAnimator;
    [SerializeField] Animator causeAnimator;

    [SerializeField] DataPersistanceManager dpm;
    [SerializeField] DialogUIHandler duih;
    [SerializeField] GameObject returnButton;

    private GameObject uiContainer;
    private GameObject player;

    private bool isDeathSequenceRunning = false;
    private bool isGameOverSequenceRunning = false;

    private void Start()
    {
        uiContainer = GameObject.FindGameObjectWithTag("UIContinueContainer");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            StartDeathSequence("KILLED BY CONSOLE.");
        }
    }

    public void StartDeathSequence(string deathCause)
    {
        if(isDeathSequenceRunning)
        {
            return;
        }

        duih.InterruptAll();

        isDeathSequenceRunning = true;
        dpm.GetGameState().NUMBER_OF_LIVES -= 1;
        dpm.SaveGame();
        NukeWorld();

        player.GetComponent<FirstPersonMovement>().TeleportToPositionalCoordinates(new Vector3(0, -165, 0));
        player.GetComponent<PlayerInformation>().currentChunkType = GenerationManager.ChunkType.Purgatory;
        player.GetComponent<PlayerInformation>().ChunkUpdateAction();

        fpsCamera.enabled = false;
        fpsCamera.GetComponent<AudioListener>().enabled = false;
        eventCamera.enabled = true;
        eventCamera.GetComponent<AudioListener>().enabled = true;

        causeAnimator.GetComponent<TMP_Text>().text = deathCause;

        fpm.ToggleMovement();
        vp_death.Play();
        vs_death.StartVideoShrink();
        gp.Generate();
        twUI.StartTypewrite();
        dzc_death.StartDelayedAnimation();
        ss.StartSkullSpawns();
    }

    public void FinishDeathSequence()
    {
        if(!isDeathSequenceRunning)
        {
            return;
        }

        eventCamera.enabled = false;
        eventCamera.GetComponent<AudioListener>().enabled = false;
        fpsCamera.enabled = true;
        fpsCamera.GetComponent<AudioListener>().enabled = true;

        fpm.ToggleMovement();
        fpl.ToggleCursorLocked(CursorLockMode.Locked);

        foreach(Transform child in skullContainer.transform)
        {
            Destroy(child.gameObject);
        }

        vs_death.GetComponent<Animator>().Play("VideoCanvasIdle");
        uiContainer.GetComponent<Animator>().Play("UIContainerIdle");
        diedZoopAnimator.Play("GeneralAnimationZoopIdle");
        causeAnimator.Play("GeneralAnimationZoopIdle");
        cbAnimator.Play("ButtonFadeIdle");
        ss.SetSkullsToGenerate(dpm.GetGameState().NUMBER_OF_LIVES);

        isDeathSequenceRunning = false;
        dpm.ReformatGame();
        gm.GenerateWorld();
        pi.ChunkUpdateAction();

        StartCoroutine(duih.PlayDelayed("Respawn", true, 1.15f));
    }

    public void StartGameOverSequence()
    {
        if (isGameOverSequenceRunning)
        {
            return;
        } 
        
        duih.InterruptAll();
        isGameOverSequenceRunning = true;
        
        dpm.DeleteGame();
        eventCamera.gameObject.transform.position = WorldWideScripts.ModifyV3(eventCamera.gameObject.transform.position, 186.8f, 0, -20f);
        
        vp_gameOver.Play();
        vs_gameOver.StartVideoShrink();
        
        dzc_gameOver.StartDelayedAnimation(true);
        returnButton.GetComponent<ContinueButtonController>().Animate();
    }

    public void LoadData(GameStates data)
    {
        ss.SetSkullsToGenerate(data.NUMBER_OF_LIVES);
    }

    public void SaveData(ref GameStates data)
    {
        data.NUMBER_OF_LIVES = ss.GetSkullsToGenerate();
    }

    private void NukeWorld()
    {
        foreach(Transform transform in worldGrid.transform)
        {
            Destroy(transform.gameObject);
        }
    }
}
