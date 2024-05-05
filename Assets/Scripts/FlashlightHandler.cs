using UnityEngine;

public class FlashlightHandler : MonoBehaviour
{
    private Light lightSource;
    private void Start()
    {
        lightSource = GetComponentInChildren<Light>();
    }

    public void Activate(bool toTurnOn)
    {
        lightSource.enabled = toTurnOn;
    }
}
