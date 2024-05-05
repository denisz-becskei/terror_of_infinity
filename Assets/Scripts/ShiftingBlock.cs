using System.Collections;
using UnityEngine;

public class ShiftingBlock : MonoBehaviour
{
    private Animator animator;
    private bool state;
    private GameObject player;
    private DeathHandler dh;
    private bool isMonitoring = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        state = WorldWideScripts.Chance(35);
        dh = GameObject.FindGameObjectWithTag("GameController").GetComponent<DeathHandler>();
        Shift();
    }

    private void FixedUpdate()
    {
        if(isMonitoring)
        {
            if(player.transform.position.y > 3f)
            {
                dh.StartDeathSequence("CRUSHED.");
            }
        }
    }

    public void Shift()
    {
        if(!state)
        {
            animator.Play("Shift");
            state = true;
        } else
        {
            animator.Play("ReverseShift");
            state = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            isMonitoring = true;
            StartCoroutine(MonitoringFalloff());
        }
    }

    IEnumerator MonitoringFalloff()
    {
        yield return new WaitForSeconds(5);
        isMonitoring = false;
    }
}
