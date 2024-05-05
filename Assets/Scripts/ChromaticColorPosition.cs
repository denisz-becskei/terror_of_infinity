using UnityEngine;

public class ChromaticColorPosition : MonoBehaviour
{
    public bool isEnabled = false;
    public bool isCalled = false;
    public Color? currentColor;
    public string debugColor;

    private PlayerInformation pi;
    private RaycastHit hit;

    private void Start()
    {
        pi = GetComponent<PlayerInformation>();
    }

    private void FixedUpdate()
    {
        if (pi.currentChunkType != GenerationManager.ChunkType.ChromaticConondrum) return;

        if(!isEnabled) return;
        if(isCalled) RayCastColorDetection();
    }

    void RayCastColorDetection()
    {
        Vector3 dwn = transform.TransformDirection(Vector3.down);
        
        if(Physics.Raycast(transform.position, dwn, out hit, 4))
        {
            Debug.Log("Hitting: " + hit.transform.gameObject.name);
            Light? light = hit.transform.parent.gameObject.GetComponentInChildren<Light>();
            if (light == null)
            {
                currentColor = null;
            } else
            {
                currentColor = light.color;
            }
            debugColor = currentColor.ToString();
        }
    }
}
