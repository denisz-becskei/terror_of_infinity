using UnityEngine;

public class AssignChromaticLightColors : MonoBehaviour
{
    public int color;

    private void Start()
    {
        Light lightType = GetComponentInChildren<Light>();
        color = Random.Range(0, 4);
        switch(color)
        {
            case 0:
                lightType.color = Color.red;
                break;
            case 1:
                lightType.color = Color.blue;
                break;
            case 2:
                lightType.color = Color.green;
                break;
            case 3:
                lightType.color = Color.yellow;
                break;
            default:
                Debug.LogError("Something is wrong with da colors");
                break;
        }
    }
}
