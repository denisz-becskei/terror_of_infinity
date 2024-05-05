using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlockProtocol : MonoBehaviour
{
    private List<GameObject> walls = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1.5f);
        Collider[] hitColliders = Physics.OverlapBox(new Vector3(this.transform.position.x, this.transform.position.y + 8f, this.transform.position.z), new Vector3(7f, 1f, 7f));
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.gameObject.name);
        }
    }
}
