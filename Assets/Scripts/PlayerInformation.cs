using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GenerationManager;

public class PlayerInformation : MonoBehaviour, IDataPersistance
{
    public ChunkType currentChunkType;
    public bool walkVision = false;

    private RaycastHit hit;
    public string playerCurrentWorldPosition;

    [SerializeField] private GameObject nightSkyDome;
    [SerializeField] private ChromaticColorPosition ccp;
    [SerializeField] private GameObject sinewave;
    [SerializeField] private GameObject sinewaveTexture;

    [SerializeField] private DialogUIHandler dialog;

    private Coroutine updatePositionRoutine;
    private Dictionary<ChunkType, bool> wereVisited = new Dictionary<ChunkType, bool>();
    private bool isDialogSystemRunning = false;

    private void Start()
    {
        StartCoroutine(LateStart());
        updatePositionRoutine = StartCoroutine(UpdatePlayerPosition());
    }

    public void ChunkUpdateAction()
    {
        // If the player stands in a "Haze of Death" ChunkType, generate thick fog around the player, else remove fog if exists
        float fogDensity = currentChunkType == ChunkType.HazeOfDeath ? 0.3f : 0.027f;
        Color fogColorHaze = currentChunkType == ChunkType.HazeOfDeath ? new Color32(173, 202, 198, 255) : new Color(0, 0, 0);
        if (RenderSettings.fogDensity != fogDensity)
        {
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fogColor = fogColorHaze;
        }
        
        // Turn off Flashlight if standing in "Labyrinth of Blindness" ChunkType
        walkVision = currentChunkType == ChunkType.LabyrinthOfBlindness;
        GetComponentInChildren<Light>().enabled = !walkVision;

        // Enable NightSky dome if standing in "Infinite Night Sky" ChunkType
        if(currentChunkType == ChunkType.InfiniteNightSky && !nightSkyDome.activeSelf)
        {
            nightSkyDome.SetActive(true);
        } else if (currentChunkType != ChunkType.InfiniteNightSky && nightSkyDome.activeSelf)
        {
            nightSkyDome.SetActive(false);
        }

        // Turn on Chromatic scripts when entering Chromatic Conondrum
        if(currentChunkType == ChunkType.ChromaticConondrum)
        {
            sinewave.SetActive(true);
            sinewave.GetComponentInChildren<Sinewave>().isActive = true;
            sinewaveTexture.SetActive(true);
            ccp.isEnabled = true;

        }
        else if(currentChunkType != ChunkType.ChromaticConondrum && (sinewave.activeSelf || ccp.enabled))
        {
            sinewave.GetComponentInChildren<Sinewave>().flashlight.Interrupt();
            sinewave.GetComponentInChildren<Sinewave>().isActive = false;
            sinewave.SetActive(false);
            sinewaveTexture.SetActive(false);
            
            ccp.isEnabled = false;
        }

        // Turn off Player Position Updating when in Purgatory
        if(currentChunkType == ChunkType.Purgatory)
        {
            StopCoroutine(updatePositionRoutine);
            updatePositionRoutine = null;
        } else { 
            updatePositionRoutine = StartCoroutine(UpdatePlayerPosition());
        }

        if(isDialogSystemRunning)
        {
            DialogUpdateAction();
        }
    }

    private IEnumerator UpdatePlayerPosition()
    {
        yield return new WaitForSeconds(2f);
        PositionUpdate();
    }

    private void PositionUpdate()
    {
        Vector3 dwn = transform.TransformDirection(Vector3.down);
        Vector3 newRaycastPosition = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
        if (Physics.Raycast(newRaycastPosition, dwn, out hit, 20f))
        {
            if (hit.transform.gameObject.CompareTag("Ground"))
            {
                var rp = hit.transform.parent.GetComponent<RoomPosition>();
                if (rp != null)
                {
                    playerCurrentWorldPosition = rp.GetRoomCoordinatesInWorld();
                }
            }
        }
        updatePositionRoutine = StartCoroutine(UpdatePlayerPosition());
    }

    public void LoadData(GameStates data)
    {
        playerCurrentWorldPosition = data.PLAYER_WORLD_COORDINATES;
        Debug.Log("Loading World Position");
        // TODO: Move player there
    }

    public void SaveData(ref GameStates data)
    {
        data.PLAYER_WORLD_COORDINATES = playerCurrentWorldPosition;
        Debug.Log("Saving World Position");
    }

    private void DialogUpdateAction()
    {
        if(wereVisited.Count == 0)
        {
            PopulateVisitedDictionary();
        }

        if (wereVisited[currentChunkType] == false)
        {
            bool success = dialog.Play(WorldWideScripts.chunkTypesByInt[((int)currentChunkType)], false);
            if (success)
            {
                wereVisited[currentChunkType] = true;
            }
        }
    }

    private void PopulateVisitedDictionary()
    {
        for(int i = 1; i < 18; i++)
        {
            wereVisited.Add((ChunkType)i, false);
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(5f);
        isDialogSystemRunning = true;
    }
}
