using System;
using System.Collections;
using UnityEngine;

public class GuardingFlameController : MonoBehaviour
{
    private Vector3 target;
    private readonly float speed = 1f;
    private float lifeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        this.lifeTime = WorldWideScripts.GetTotallyRandomNumberBetween(10, 45);
        StartCoroutine(EndLife());
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
            rooms[WorldWideScripts.GetTotallyRandomNumberBetween(0, rooms.Length)].transform.position.x,
            2f,
            rooms[WorldWideScripts.GetTotallyRandomNumberBetween(0, rooms.Length)].transform.position.z
            );
    }

    IEnumerator EndLife()
    {
        yield return new WaitForSeconds(this.lifeTime);
        Destroy(this.gameObject);
    }
}
