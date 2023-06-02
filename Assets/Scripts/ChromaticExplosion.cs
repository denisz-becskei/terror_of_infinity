using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaticExplosion : MonoBehaviour
{
    GameObject[] killZones;
    public void Explode(Color? color, bool isPlayerDead)
    {
        if (isPlayerDead)
        {

        }
        else
        {
            killZones = KillZoneFinder().ToArray();
            foreach (GameObject kz in killZones)
            {
                Light? light = kz.transform.parent.gameObject.GetComponentInChildren<Light>();
                if (light != null)
                {
                    kz.GetComponent<Renderer>().enabled = !(light.color == color);
                }
                else
                {
                    kz.GetComponent<Renderer>().enabled = true;
                }
            }
            StartCoroutine(ResetExplode());
        }
    }

    List<GameObject> KillZoneFinder()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 7f);
        List<GameObject> killZones = new List<GameObject>();

        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.CompareTag("Ground"))
            {
                killZones.Add(collider.transform.parent.GetComponentInChildren<KillZone>().gameObject);
            }
        }
        return killZones;
    }

    IEnumerator ResetExplode()
    {
        yield return new WaitForSeconds(5f);
        foreach (GameObject kz in killZones)
        {
            kz.GetComponent<Renderer>().enabled = false;
        }
    }
}
