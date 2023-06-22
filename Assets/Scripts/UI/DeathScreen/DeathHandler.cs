using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class DeathHandler : MonoBehaviour, IDataPersistance
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
    
    [SerializeField] GameObject worldGrid;
    [SerializeField] GenerationManager gm;

    [SerializeField] GameObject skullContainer;

    [SerializeField] VideoPlayer vp;

    [SerializeField] Animator diedZoopAnimator;
    [SerializeField] Animator cbAnimator;
    [SerializeField] Animator causeAnimator;

    [SerializeField] DataPersistanceManager dpm;

    private GameObject uiContainer;
    private GameObject player;

    private bool isDeathSequenceRunning = false;

    private void Start()
    {
        uiContainer = GameObject.FindGameObjectWithTag("UIContainer");
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

        isDeathSequenceRunning = true;
        dpm.SaveGame();
        NukeWorld();

        player.GetComponent<FirstPersonMovement>().TeleportToPositionalCoordinates(new Vector3(0, -99, 0));
        player.GetComponent<PlayerInformation>().currentChunkType = GenerationManager.ChunkType.Purgatory;
        player.GetComponent<PlayerInformation>().ChunkUpdateAction();

        fpsCamera.enabled = false;
        fpsCamera.GetComponent<AudioListener>().enabled = false;
        eventCamera.enabled = true;
        eventCamera.GetComponent<AudioListener>().enabled = true;

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

        vs.GetComponent<Animator>().Play("VideoCanvasIdle");
        uiContainer.GetComponent<Animator>().Play("UIContainerIdle");
        diedZoopAnimator.Play("GeneralAnimationZoopIdle");
        causeAnimator.Play("GeneralAnimationZoopIdle");
        cbAnimator.Play("ButtonFadeIdle");
        ss.SetSkullsToGenerate(dpm.GetGameState().NUMBER_OF_LIVES - 1);

        isDeathSequenceRunning = false;
        // TODO: Respawn Player Elsewhere
        dpm.ReformatGame();
        gm.GenerateWorld();
    }

    public void LoadData(GameStates data)
    {
        ss.SetSkullsToGenerate(data.NUMBER_OF_LIVES);
        Debug.Log("Loaded Number of Lives");
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
