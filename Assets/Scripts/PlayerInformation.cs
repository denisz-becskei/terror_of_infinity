using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GenerationManager;

public class PlayerInformation : MonoBehaviour
{
    public ChunkType currentChunkType;
    public bool walkVision = false;

    [SerializeField] private GameObject nightSkyDome;
    [SerializeField] private HavenColorPosition hcp;
    [SerializeField] private GameObject sinewave;

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

        // Turn on Haven scripts when entering Haven
        if(currentChunkType == ChunkType.Haven)
        {
            sinewave.SetActive(true);
            hcp.isEnabled = true;
        }
        else if(currentChunkType != ChunkType.Haven && (sinewave.activeSelf || hcp.enabled))
        {
            sinewave.GetComponent<Sinewave>().flashlight.Interrupt();
            sinewave.SetActive(false);
            
            hcp.isEnabled = false;
        }
    }
}
