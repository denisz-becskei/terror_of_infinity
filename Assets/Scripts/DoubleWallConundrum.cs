using System.Collections;
using UnityEngine;

public class DoubleWallConundrum : MonoBehaviour
{
    public string identifier;

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    void DetectSurroundingDoors()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Door"))
            {
                Debug.Log("Destroying walls");
                Destroy(gameObject);
            }
        }
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(2);
        switch(identifier)
        {
            case "Wall":
                DetectSurroundingDoors();
                break;
            case "Door":
                //DetectSurroundingWalls();
                break;
        }
    }
}
