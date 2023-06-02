using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DoorType", menuName = "ScriptableObjects/DoorTypeScriptableObject")]
public class DoorTypeScriptableObject : ScriptableObject
{
    public GameObject doorObjectPrefab;
    public float openDoorTime;
}
