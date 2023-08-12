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

    private void Awake()
    {
        sinewaveContainer.SetActive(true);
        sinewave = sinewaveContainer.GetComponentInChildren<Sinewave>();
        sinewaveContainer.SetActive(false);
    }

    public void StartAction(ChunkType chunkTypeLeft, ChunkType chunkTypeEntered)
    {
        LeftChunkAction(chunkTypeLeft);
        DefaultChunkAction(chunkTypeEntered);
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
                sinewaveTexture.SetActive(true);
                sinewave.isActive = true;
                ccp.isEnabled = true;
                statusEffectController.AddStatusEffect("FeelingVibrations", false);
                break;
            case ChunkType.PossessedTeddies:
                statusEffectController.AddStatusEffect("PowerSurge", true, 88.4f);
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
                statusEffectController.RemoveStatusEffect("FeelingVibrations", false);
                break;
            case ChunkType.PossessedTeddies:
                statusEffectController.RemoveStatusEffect("PowerSurge", true, 46.7f);
                break;
        }
    }

    private void DefaultChunkAction(ChunkType chunkType)
    {
        AgentScriptableObject currentActiveAgent = WorldWideScripts.GetAgentByChunkType(chunkType, EnemyIndicator.agentTypes);
        EnemyIndicator.UpdateEnemy(currentActiveAgent);
    }
}
