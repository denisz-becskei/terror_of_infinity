using UnityEngine;

public class Room : MonoBehaviour
{
    public bool centerAvailable;
    [SerializeField]
    private bool baseRoomGenOverride = false;
    [SerializeField]
    private Material groundMaterial, roofMaterial;


    private void Start()
    {
        if(!baseRoomGenOverride)
        {
            NecessaryObjects no = GameObject.FindGameObjectWithTag("GenerationController").GetComponent<NecessaryObjects>();

            GameObject ground = Instantiate(no.groundPrefab, transform);
            GameObject roof = Instantiate(no.roofPrefab, transform);

            ground.GetComponent<MeshRenderer>().material = groundMaterial;
            roof.GetComponent<MeshRenderer>().material = roofMaterial;
        }
    }
}
