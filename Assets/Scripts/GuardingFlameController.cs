using System;
using System.Collections;
using UnityEngine;

public class GuardingFlameController : MonoBehaviour
{
    private Vector3 target;
    private readonly float speed = 1f;
    private float lifeTime;
    private GameObject player;
    private StatusEffectController statusEffectController;

    private Coroutine checkPlayerDistance;
    
    void Start()
    {
        this.lifeTime = WorldWideScripts.GetPureRandomNumberBetween(10, 45);
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.statusEffectController = GameObject.FindGameObjectWithTag("StatusEffectController")
            .GetComponent<StatusEffectController>();
        StartCoroutine(EndLife());
        checkPlayerDistance = StartCoroutine(CheckPlayerDistance());
        SelectTarget();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (Math.Round(transform.position.x, 2) == Math.Round(target.x, 2) 
            && Math.Round(transform.position.z, 2) == Math.Round(target.z, 2))
        {
            SelectTarget();
        }
    }

    void SelectTarget()
    {
        Room[] rooms = GameObject.FindObjectsByType<Room>(FindObjectsSortMode.None);
        this.target = new Vector3(
            rooms[WorldWideScripts.GetPureRandomNumberBetween(0, rooms.Length)].transform.position.x,
            2f,
            rooms[WorldWideScripts.GetPureRandomNumberBetween(0, rooms.Length)].transform.position.z
            );
    }

    IEnumerator EndLife()
    {
        yield return new WaitForSeconds(this.lifeTime);
        StopCoroutine(checkPlayerDistance);
        Destroy(this.gameObject);
    }

    IEnumerator CheckPlayerDistance()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
        float distance = WorldWideScripts.CalculateDistance(this.gameObject, player);
        if (distance < 5f)
        {
            statusEffectController.AddStatusEffect("Darkness", true, true, -1);
        }

        checkPlayerDistance = StartCoroutine(CheckPlayerDistance());
    }
}
