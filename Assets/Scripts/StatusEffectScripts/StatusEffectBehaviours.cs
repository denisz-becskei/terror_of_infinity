using System.Collections;
using UnityEngine;

public class StatusEffectBehaviours : MonoBehaviour
{
    [SerializeField] DeathHandler dh;
    [SerializeField] private AudioSource behavioralAudio;
    [SerializeField] AudioClip[] coughingClips;

    // Toxication
    private Coroutine suffocationRoutine;
    private bool isToxicationActive = false;

    public void AddToxication()
    {
        isToxicationActive = true;
        suffocationRoutine = StartCoroutine(ToxicationTimer());
    }

    public void RemoveToxication()
    {
        if(isToxicationActive)
        {
            isToxicationActive = false;
            StopCoroutine(suffocationRoutine);
        }
    }

    private IEnumerator ToxicationTimer()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(60);
            behavioralAudio.clip = coughingClips[i];
            behavioralAudio.Play();
            Debug.Log("Phase " + i);
        }
        yield return new WaitForSeconds(2.5f);
        dh.StartDeathSequence("SUFFOCATED.");
    }
}
