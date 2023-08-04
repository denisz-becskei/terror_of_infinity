using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Spawning,
        Harass,
        Hide,
        Hunt
    }

    [SerializeField] Transform player;
    private NavMeshAgent nma;
    public EnemyState enemyState = EnemyState.Spawning;
    private bool huntTimerStarted = false;
    [SerializeField] bool isDebug = false;
    [SerializeField] TMP_Text modeText;
    private const float AGENT_SPEED = 4f;
    private const float AGENT_SPEED_BOOST = 7.5f;

    [SerializeField] AgentScriptableObject[] agents;

    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
        if(isDebug)
        {
            modeText.alpha = 1.0f;
        } else
        {
            modeText.alpha = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        switch (enemyState)
        {
            case EnemyState.Harass:
                Harass();
                modeText.text = "EnemyState: HARASS";
                break;
            case EnemyState.Hide:
                if (setHide) break;
                Hide();
                modeText.text = "EnemyState: HIDE";
                break;
        }
    }

    public void Move(Vector3 position)
    {
        nma.SetDestination(position);
    }

    private void Harass()
    {
        nma.speed = AGENT_SPEED;
        Move(player.position);
        Vector3 directionToPlayer = player.position - transform.position;
        RaycastHit hitInfo;
        bool raycastHit = Physics.Raycast(transform.position, directionToPlayer, out hitInfo);

        // Only increase speed if the ray reaches the player without hitting any obstacles
        if (raycastHit && hitInfo.collider.gameObject == player.gameObject)
        {
            bool beginHunt = WorldWideScripts.Chance(50);
            if (beginHunt)
            {
                enemyState = EnemyState.Hunt;
                if(!huntTimerStarted)
                {
                    huntTimerStarted = true;
                    StartCoroutine(HuntTimer());
                }
            } else
            {
                enemyState = EnemyState.Hide;
            }
        }

    }

    private void Hunt()
    {
        nma.speed = AGENT_SPEED_BOOST;
        Move(player.position);
    }

    private bool setHide = false;

    private void Hide()
    {
        nma.speed = AGENT_SPEED_BOOST * 1.1f;
        if(!setHide)
        {
            Move(WorldWideScripts.GetFurthestRoomFromGameObject(player.transform).transform.position);
            setHide = true;
        }

        float dist = nma.remainingDistance; 
        if (dist != Mathf.Infinity && nma.pathStatus == NavMeshPathStatus.PathComplete && nma.remainingDistance == 0)
        {
            Debug.Log("Destination Reached! Entering Harassment Mode!");
            setHide = false;
            enemyState = EnemyState.Harass;
        }

        if(nma.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            Debug.LogError("Something's wrong");
        }
    }

    private IEnumerator HuntTimer()
    {
        yield return new WaitForSeconds(Random.Range(5f, 30f));
        enemyState = EnemyState.Hide;
        huntTimerStarted = false;
    }

}
