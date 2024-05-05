using System.Collections.Generic;
using UnityEngine;
using static TypeInit;

[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/DialogScriptableObject")]
public class DialogScriptableObject : ScriptableObject
{
    public List<DialogLine> dialogLines = new List<DialogLine>();
}
