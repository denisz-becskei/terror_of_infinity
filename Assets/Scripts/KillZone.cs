using UnityEngine;

public class KillZone : MonoBehaviour
{
    public bool isActive = false;
    private GameObject player;
    private DeathHandler dh;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dh = GameObject.FindGameObjectWithTag("GameController").GetComponent<DeathHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isActive && other.gameObject == player)
        {
            dh.StartDeathSequence("RAN INTO OBLIVION.");
        }
    }
}
