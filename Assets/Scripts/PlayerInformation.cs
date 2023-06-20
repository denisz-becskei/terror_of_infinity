using System;
using System.Collections;
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

    private void Start()
    {
        StartCoroutine(UpdatePlayerPosition());
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
            sinewaveTexture.SetActive(true);
            ccp.isEnabled = true;
        }
        else if(currentChunkType != ChunkType.ChromaticConondrum && (sinewave.activeSelf || ccp.enabled))
        {
            sinewave.GetComponentInChildren<Sinewave>().flashlight.Interrupt();
            sinewave.SetActive(false);
            sinewaveTexture.SetActive(false);
            
            ccp.isEnabled = false;
        }
    }

    private IEnumerator UpdatePlayerPosition()
    {
        yield return new WaitForSeconds(2f);

        Vector3 dwn = transform.TransformDirection(Vector3.down);

        if (Physics.Raycast(transform.position, dwn, out hit, 4))
        {
            try
            {
                if (hit.transform.parent.TryGetComponent<RoomPosition>(out var rp))
                {
                    playerCurrentWorldPosition = rp.coordinates;
                }
            } catch (NullReferenceException)
            {
                
            }
        }
        StartCoroutine(UpdatePlayerPosition());
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
}
