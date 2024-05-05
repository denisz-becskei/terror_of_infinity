using UnityEngine;

public class MoveNightSky : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private Material material;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        this.GetComponent<Renderer>().material = material;
    }

    private void FixedUpdate()
    {
        transform.position = player.transform.position;
    }
}
