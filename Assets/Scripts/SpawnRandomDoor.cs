using UnityEngine;

public class SpawnRandomDoor : MonoBehaviour
{
    [SerializeField] DoorTypeScriptableObject[] possibleDoorTypes;

    private void Start()
    {
        GameObject door = Instantiate(possibleDoorTypes[Random.Range(0, possibleDoorTypes.Length)].doorObjectPrefab, transform);
        door.transform.localScale = new Vector3(0.29f, 0.31f, 4.89f);
        door.transform.localPosition = new Vector3(0, -1.15f, 0);
    }
}
