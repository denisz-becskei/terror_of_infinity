using UnityEngine;
using static TypeInit;

public class Room : MonoBehaviour
{
    public bool centerAvailable;
    [SerializeField]
    private bool baseRoomGenOverride = false;
    [SerializeField]
    private Material groundMaterial, roofMaterial;
    [SerializeField]
    private SurfaceSoundScriptableObject surfaceSound = null;

    private GameObject player;

    public WallsFilled walls;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if(!baseRoomGenOverride)
        {
            NecessaryObjects no = GameObject.FindGameObjectWithTag("GenerationController").GetComponent<NecessaryObjects>();

            GameObject ground = Instantiate(no.groundPrefab, transform);
            GameObject roof = Instantiate(no.roofPrefab, transform);

            if(surfaceSound == null)
            {
                ground.GetComponent<MeshRenderer>().material = groundMaterial;
            } else
            {
                ground.GetComponent<MeshRenderer>().material = surfaceSound.material;
                FirstPersonAudio fpa = player.GetComponentInChildren<FirstPersonAudio>();
                fpa.landingSFX = new AudioClip[] { surfaceSound.landingSound1, surfaceSound.landingSound2, surfaceSound.landingSound3 };
                fpa.stepAudio.clip = surfaceSound.stepSound;
                fpa.runningAudio.clip = surfaceSound.stepSound;
            }
            roof.GetComponent<MeshRenderer>().material = roofMaterial;
        }
    }
}
