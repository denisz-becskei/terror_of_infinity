using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaticExplosion : MonoBehaviour
{
    GameObject[] killZones;
    [SerializeField] DeathHandler dh;

    [SerializeField] AudioClip laserClip;
    public void Explode(Color? color, bool isPlayerDead)
    {
        if (isPlayerDead)
        {
            dh.StartDeathSequence("EVAPORATED.");
        }
        else
        {
            killZones = KillZoneFinder().ToArray();
            AudioSource audioSource = killZones[WorldWideScripts.GetTotallyRandomNumberBetween(0, killZones.Length)].AddComponent<AudioSource>();
            audioSource.clip = laserClip;
            audioSource.Play();

            foreach (GameObject kz in killZones)
            {
                Light? light = kz.transform.parent.gameObject.GetComponentInChildren<Light>();
                if (light != null)
                {
                    kz.GetComponent<Renderer>().enabled = !(light.color == color);
                    if(light.color != color)
                    {
                        kz.GetComponent<KillZone>().isActive = true;
                    }
                }
                else
                {
                    kz.GetComponent<Renderer>().enabled = true;
                    kz.GetComponent<KillZone>().isActive = true;
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
            kz.GetComponent<KillZone>().isActive = false;
        }
    }
}
