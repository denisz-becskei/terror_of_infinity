using UnityEngine;

public class BitGeneration : MonoBehaviour
{
    public void GenerateBit(GameObject bitPrefab, string objectiveType)
    {
        GameObject newBit = Instantiate(bitPrefab, WorldWideScripts.ModifyV3(this.transform.position, 0, 1.5f, 0), Quaternion.identity);
        newBit.transform.parent = this.transform;
        newBit.GetComponent<Bit>().SetColor(objectiveType);
    }
}
