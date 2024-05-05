using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AgentTypeContainer", menuName = "ScriptableObjects/AgentTypeContainerScriptableObject")]
public class AgentTypeContainerScriptableObject : ScriptableObject
{
    public AgentScriptableObject[] agentTypes;
}
