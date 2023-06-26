using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FallenAngel : MonoBehaviour
{
    public bool isActive = false;
    private bool isBeingLookedAt = false;
    private bool setIntoAction = false;
    private NavMeshAgent nma;
    private GameObject player;
    private Renderer renderer;

    private Coroutine updatePathingRoutine;

    private void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    private void Update()
    {
        LookedAtTest();

        if(isActive && !setIntoAction && !isBeingLookedAt)
        {
            setIntoAction = true;
            updatePathingRoutine = StartCoroutine(UpdatePathing());
            Debug.Log("Angel was triggered");
        }

        if(updatePathingRoutine != null && isBeingLookedAt)
        {
            nma.isStopped = true;
            updatePathingRoutine = null;
            setIntoAction = false;
        }
    }

    private void LookedAtTest()
    {
        if(renderer.isVisible)
        {
            isBeingLookedAt = true;
        } else
        {
            isBeingLookedAt = false;
        }
    }

    public void Move(Vector3 position)
    {
        nma.SetDestination(position);
    }

    IEnumerator UpdatePathing()
    {
        yield return new WaitForSeconds(1f);
        Move(player.transform.position);
        updatePathingRoutine = StartCoroutine(UpdatePathing());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(2f);
        NavMeshHit closestHit;

        if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
        {
            gameObject.transform.position = closestHit.position;
            nma = gameObject.AddComponent<NavMeshAgent>();
        }
        else
        {
            Debug.LogError("Could not find position on NavMesh!");
        }
    }

}
