using UnityEngine;

public class TurnOffParticlesIfFar : MonoBehaviour
{
    private Transform player;
    private bool statusOn = true;
    [SerializeField] bool constant = false;
    [SerializeField] float maxDistance = 64f;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if(statusOn)
        {
            if (WorldWideScripts.CalculateDistance(gameObject, player.gameObject) >= maxDistance)
            {
                statusOn = false;
                GetComponent<ParticleSystem>().Pause();
            }
        }
        else
        {
            if (WorldWideScripts.CalculateDistance(gameObject, player.gameObject) < maxDistance && !constant)
            {
                statusOn = true;
                GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
