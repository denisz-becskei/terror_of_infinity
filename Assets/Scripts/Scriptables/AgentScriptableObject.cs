using UnityEngine;

[CreateAssetMenu(fileName ="Agent", menuName ="ScriptableObjects/AgentScriptableObject")]
public class AgentScriptableObject : ScriptableObject
{
    public string agentType;
    public Sprite agentSprite;

    public float agentSpeed;
    public float boostedAgentSpeedPercentage;
    public float[] huntChance;
    public float[] huntDuration;
    public float[] difficultyRange;
    public bool specialBehavior;
}
