using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FallenAngel : MonoBehaviour
{
    public bool isActive = false;
    private bool isBeingLookedAt = false;
    private NavMeshAgent nma;
    private GameObject player;
    private DeathHandler dh;
    private GenerationManager gm;

    private Camera fpsCamera;
    private Bounds bounds;
    private Plane[] cameraFrustrum;
    
    private Coroutine updatePathingRoutine;

    private float rotationY;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        gameObject.AddComponent<BoxCollider>();
        bounds = GetComponent<BoxCollider>().bounds;
        fpsCamera = player.GetComponentInChildren<Camera>();
        rotationY = transform.rotation.y;
        dh = GameObject.FindGameObjectWithTag("GameController").GetComponent<DeathHandler>();
        gm = GameObject.FindGameObjectWithTag("GenerationController").GetComponent<GenerationManager>();
        StartCoroutine(LateStart());
    }

    private void Update()
    {
        if (isActive)
        {
            LookedAtTest();
        }

        if (isActive && !isBeingLookedAt)
        {
            updatePathingRoutine = StartCoroutine(UpdatePathing());
        } else
        {
            if(updatePathingRoutine != null)
            {
                StopCoroutine(updatePathingRoutine);
                StopNavigation();
                updatePathingRoutine = null;
            }
        }
    }

    private void LookedAtTest()
    {
        cameraFrustrum = GeometryUtility.CalculateFrustumPlanes(fpsCamera);
        if(GeometryUtility.TestPlanesAABB(cameraFrustrum, bounds))
        {
            isBeingLookedAt = true;
            Debug.Log("Angel is being looked at.");
        } else
        {
            Debug.Log("Angel is no longer being looked at.");
            isBeingLookedAt = false;
        }
    }

    private void Move(Vector3 position)
    {
        nma.isStopped = false;
        Quaternion _lookRotation = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        transform.rotation = new Quaternion(_lookRotation.x, _lookRotation.y - rotationY, _lookRotation.z, _lookRotation.w);
        nma.SetDestination(position);
    }

    private void StopNavigation()
    {
        nma.velocity = Vector3.zero;
        nma.isStopped = true;
    }

    IEnumerator UpdatePathing()
    {
        Move(player.transform.position);
        yield return new WaitForSeconds(1f);
        if(!isBeingLookedAt)
        {
            updatePathingRoutine = StartCoroutine(UpdatePathing());
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(2f);
        NavMeshHit closestHit;
        gm.Rebake();

        if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, NavMesh.AllAreas))
        {
            gameObject.transform.position = closestHit.position;
            nma = gameObject.AddComponent<NavMeshAgent>();
            nma.baseOffset = 0;
            nma.speed = 16f;
            nma.acceleration = 16f;
        }
        else
        {
            Debug.LogError("Could not find position on NavMesh! Retrying.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isActive && !isBeingLookedAt && collision.gameObject.CompareTag("Player"))
        {
            dh.StartDeathSequence("ASCENDED.");
        }
    }

}
