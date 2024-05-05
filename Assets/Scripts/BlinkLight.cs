using System.Collections;
using UnityEngine;

public class BlinkLight : MonoBehaviour
{
    [SerializeField] AudioClip clickOn, clickOff;
    [SerializeField] ChromaticExplosion ce;

    private Light spotlight;
    private AudioSource audioSource;
    public Color color = Color.white;

    public Coroutine blinkRoutine = null;

    private void Start()
    {
        spotlight = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Blink()
    {
        blinkRoutine = StartCoroutine(RandomBlinkage());
    }

    public void Interrupt()
    {
        if (blinkRoutine != null)
        {
            spotlight.enabled = true;
            spotlight.color = Color.white;
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }
    }

    public void Explode(Color? playerCurrentColor)
    {
        if(playerCurrentColor == null || playerCurrentColor != color)
        {
            ce.Explode(null, true);
            Debug.Log("YOU DIED!");
        } else
        {
            ce.Explode(playerCurrentColor, false);
            Debug.Log("You are safe... for now!");
        }
    }

    IEnumerator RandomBlinkage()
    {
        yield return new WaitForSeconds(Random.Range(0.75f, 1.25f));
        spotlight.color = color;
        spotlight.enabled = !spotlight.enabled;
        if(spotlight.enabled)
        {
            audioSource.clip = clickOn;
        } else
        {
            audioSource.clip = clickOff;
        }
        audioSource.Play();

        blinkRoutine = StartCoroutine(RandomBlinkage());
    }
}
