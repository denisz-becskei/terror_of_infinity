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

    private ChunkTypeScriptableObject SelectChunkType(float objectiveScore, float difficultyScore)
    {
        foreach(ChunkTypeScriptableObject chunkType in chunkTypes)
        {
            if (chunkType.ranges[0] < objectiveScore && objectiveScore <= chunkType.ranges[1])
            {
                if (chunkType.ranges[2] < difficultyScore && difficultyScore <= chunkType.ranges[3])
                {
                    return chunkType;
                }
            }
        }
        return chunkTypes[0];
    }

    public ChunkTypeScriptableObject GetChunkAtPosition(int positionX, int positionY)
    {
        float xObjective = objectiveShift + (float)positionX / 32 * 2;
        float yObjective = objectiveShift + (float)positionY / 32 * 2;

        float xDifficulty = difficultyShift + (float)positionX / 32 * 2;
        float yDifficulty = difficultyShift + (float)positionY / 32 * 2;

        float objectiveValueAtPosition = WorldWideScripts.GetPerlinNoiseValue(xObjective, yObjective);
        float difficultyValueAtPosition = WorldWideScripts.GetPerlinNoiseValue(xDifficulty, yDifficulty);
        Debug.Log("Values: " + objectiveValueAtPosition.ToString() + " ; " + difficultyValueAtPosition.ToString());
        return SelectChunkType(objectiveValueAtPosition, difficultyValueAtPosition); ;
        
    }

    public ChunkTypeScriptableObject OverrideChunkType(ChunkType chunkType)
    {
        foreach(ChunkTypeScriptableObject chunkType_ in chunkTypes)
        {
            if (chunkType_.chunkType == chunkType)
            {
                return chunkType_;
            }
        }
        return chunkTypes[0];
    }
}
