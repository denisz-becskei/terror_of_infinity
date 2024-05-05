using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{
    private static Image enemyIndicator;
    [SerializeField] private AgentTypeContainerScriptableObject agentTypeInit;
    public static AgentTypeContainerScriptableObject agentTypes;

    private void Start()
    {
        agentTypes = agentTypeInit;
        enemyIndicator = GetComponent<Image>();
    }

    public static void UpdateEnemy(AgentScriptableObject agentType)
    {
        if (agentType == null)
        {
            enemyIndicator.sprite = null;
        }
        else
        {
            enemyIndicator.sprite = agentType.agentSprite;
        }
        
    }
}
