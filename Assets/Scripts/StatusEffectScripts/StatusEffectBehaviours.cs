using System.Collections;
using UnityEngine;

public class StatusEffectBehaviours : MonoBehaviour
{
    [SerializeField] DeathHandler dh;
    [SerializeField] private AudioSource behavioralAudio;
    [SerializeField] AudioClip[] coughingClips;
    [SerializeField] private StatusEffectController controller;

    [Header("Toxication")]
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

    // Flashlight Power
    [Header("Flashlight Power")]
    [SerializeField] private Light flashlight;
    public void ModifyFlashlightPower(float power)
    {
        flashlight.innerSpotAngle = power;
        flashlight.spotAngle = power + 20f;
    }
    
    // Darkness Stacks
    private bool inDarkness = false;
    private int currentNumberOfStacks;
    private Coroutine coroutine;

    public void TriggerDarkness(bool active, StatusEffectScriptableObject statusEffect)
    {
        currentNumberOfStacks = StatusEffectController.activeStackableEffects[statusEffect];
        inDarkness = active;
        coroutine = StartCoroutine(ApplyDarkness(statusEffect));
    }

    IEnumerator ApplyDarkness(StatusEffectScriptableObject statusEffect)
    {
        yield return new WaitForSeconds(2f);
        if (inDarkness)
        {
            controller.AddStatusEffect("Darkness", true, true, 1);
            StatusEffectController.activeStackableEffects[statusEffect] += 1;
        }
        else
        {
            if (currentNumberOfStacks > 0)
            {
                controller.AddStatusEffect("Darkness", true, true, -1);
                StatusEffectController.activeStackableEffects[statusEffect] -= 1;
            }
            else
            {
                StopCoroutine(coroutine);
            }
        }
    }
}
