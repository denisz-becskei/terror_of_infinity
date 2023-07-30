using UnityEngine;
using static GenerationManager;

public class OnChangeChunkType : MonoBehaviour
{
    [Header("Basic Objects")]
    [SerializeField] private StatusEffectController statusEffectController;
    [SerializeField] private FlashlightHandler flashlightHandler;
    
    [Header("Infinite Night Sky Objects")]
    [SerializeField] private GameObject nightSkyDome;

    [Header("Chromatic Conondrum Objects")]
    [SerializeField] private ChromaticColorPosition ccp;
    [SerializeField] private GameObject sinewaveContainer;
    [SerializeField] private GameObject sinewaveTexture;

    private Sinewave sinewave;

    private void Start()
    {
        sinewaveContainer.SetActive(true);
        sinewave = sinewaveContainer.GetComponentInChildren<Sinewave>();
        sinewaveContainer.SetActive(false);
    }

    public void StartAction(ChunkType chunkTypeLeft, ChunkType chunkTypeEntered)
    {
        LeftChunkAction(chunkTypeLeft);
        EnteredChunkAction(chunkTypeEntered);
    }

    private void EnteredChunkAction(ChunkType chunk)
    {
        switch (chunk)
        {
            case ChunkType.HazeOfDeath:
                float fogDensity = 0.3f;
                Color fogColorHaze = new Color32(173, 202, 198, 255);
                RenderSettings.fogDensity = fogDensity;
                RenderSettings.fogColor = fogColorHaze;
                break;
            case ChunkType.LabyrinthOfBlindness:
                flashlightHandler.Activate(false);
                statusEffectController.AddStatusEffect("BrokenFlashlight", false);
                statusEffectController.AddStatusEffect("WalkVision", false);
                break;
            case ChunkType.InfiniteNightSky:
                nightSkyDome.SetActive(true);
                break;
            case ChunkType.MadnessInRed:
                statusEffectController.AddStatusEffect("Toxicated", true);
                break;
            case ChunkType.ChromaticConondrum:
                sinewaveContainer.SetActive(true);
                sinewave.isActive = true;
                sinewaveTexture.SetActive(true);
                ccp.isEnabled = true;
                break;
        }
    }

    private void LeftChunkAction(ChunkType chunk)
    {
        switch(chunk)
        {
            case ChunkType.HazeOfDeath:
                float fogDensity = 0.027f;
                Color fogColorHaze = new Color(0, 0, 0);
                RenderSettings.fogDensity = fogDensity;
                RenderSettings.fogColor = fogColorHaze;
                break;
            case ChunkType.LabyrinthOfBlindness:
                flashlightHandler.Activate(true);
                statusEffectController.RemoveStatusEffect("BrokenFlashlight", false);
                statusEffectController.RemoveStatusEffect("WalkVision", false);
                break;
            case ChunkType.InfiniteNightSky:
                nightSkyDome.SetActive(false);
                break;
            case ChunkType.MadnessInRed:
                statusEffectController.RemoveStatusEffect("Toxicated", true);
                break;
            case ChunkType.ChromaticConondrum:
                sinewave.flashlight.Interrupt();
                sinewave.isActive = false;
                sinewaveContainer.SetActive(false);
                sinewaveTexture.SetActive(false);

                ccp.isEnabled = false;
                break;
        }
    }
}
