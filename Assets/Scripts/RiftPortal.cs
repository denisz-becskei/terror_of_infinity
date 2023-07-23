using UnityEngine;

public class RiftPortal : MonoBehaviour
{
    private DataPersistanceManager dpm;
    private GenerationManager gm;

    private bool wasTriggered = false;

    void Awake()
    {
        dpm = GameObject.FindGameObjectWithTag("DataPersistance").GetComponent<DataPersistanceManager>();
        gm = GameObject.FindGameObjectWithTag("GenerationController").GetComponent<GenerationManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !wasTriggered)
        {
            wasTriggered = true;
            Teleport();
        } else if (wasTriggered)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    void Teleport()
    {
        dpm.ReformatGame();
        dpm.LoadGame();
        gm.GenerateWorld();
    }
}
