using System.Collections.Generic;
using UnityEngine;
using static GenerationManager;

public class GetChunkType : MonoBehaviour
{
    [SerializeField] ChunkTypeScriptableObject[] chunkTypes;
    private int objectiveShift = 0;
    private int difficultyShift = 0;
    private void Start()
    {
        objectiveShift = Random.Range(0, 900000);
        difficultyShift = Random.Range(0, 900000);
    }

    private KeyValuePair<ChunkTypeScriptableObject, string> SelectChunkType(float objectiveScore, float difficultyScore)
    {
        foreach(ChunkTypeScriptableObject chunkType in chunkTypes)
        {
            if (chunkType.ranges[0] < objectiveScore && objectiveScore <= chunkType.ranges[1])
            {
                if (chunkType.ranges[2] < difficultyScore && difficultyScore <= chunkType.ranges[3])
                {
                    return new KeyValuePair<ChunkTypeScriptableObject, string>(chunkType, WorldWideScripts.GetObjectiveTypeByValue(objectiveScore));
                }
            }
        }
        return new KeyValuePair<ChunkTypeScriptableObject, string>(chunkTypes[Random.Range(0, chunkTypes.Length)], WorldWideScripts.GetObjectiveTypeByValue(objectiveScore));
    }

    public KeyValuePair<ChunkTypeScriptableObject, string> GetChunkAtPosition(int positionX, int positionY)
    {
        float xObjective = objectiveShift + (float)positionX / 32 * 2;
        float yObjective = objectiveShift + (float)positionY / 32 * 2;

        float xDifficulty = difficultyShift + (float)positionX / 32 * 2;
        float yDifficulty = difficultyShift + (float)positionY / 32 * 2;

        float objectiveValueAtPosition = WorldWideScripts.GetPerlinNoiseValue(xObjective, yObjective);
        float difficultyValueAtPosition = WorldWideScripts.GetPerlinNoiseValue(xDifficulty, yDifficulty);
        return SelectChunkType(objectiveValueAtPosition, difficultyValueAtPosition); ;
        
    }

    public KeyValuePair<ChunkTypeScriptableObject, string> OverrideChunkType(ChunkType chunkType)
    {
        foreach(ChunkTypeScriptableObject chunkType_ in chunkTypes)
        {
            if (chunkType_.chunkType == chunkType)
            {
                return new KeyValuePair<ChunkTypeScriptableObject, string>(chunkType_, WorldWideScripts.GetObjectiveTypeByValue(Random.Range(0, 1)));
            }
        }
        return new KeyValuePair<ChunkTypeScriptableObject, string>(chunkTypes[Random.Range(0, chunkTypes.Length)], 
            WorldWideScripts.GetObjectiveTypeByValue(Random.Range(0, 1)));;
    }
}
