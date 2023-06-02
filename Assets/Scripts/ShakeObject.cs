using UnityEngine;
using System.Collections;

public class ShakeObject : MonoBehaviour
{

    public float shakeAmount = 0.005f;
    public float shakeDuration = 2f;
    public float noiseMagnitude = 0.0005f;


    private Vector3 originalPos;
    private AudioSource possessionSound;
    private Light assignedLight;

    void Start()
    {
        originalPos = transform.localPosition;
        assignedLight = GetComponentInChildren<Light>();
        possessionSound = GetComponentInChildren<AudioSource>();
        assignedLight.enabled = false;
        if (GetComponent<CreateBearVarient>().possessed)
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 90f));

            float timer = 0f;
            possessionSound.Play();
            while (timer < shakeDuration)
            {
                float x = Random.value * 2f - 1f;
                float y = Random.value * 2f - 1f;
                float noise = Mathf.PerlinNoise(Time.time * noiseMagnitude, 0f);
                transform.localPosition = originalPos + new Vector3(x, y, 0f) * shakeAmount * noise;
                timer += Time.deltaTime;

                if (WorldWideScripts.Chance(50))
                {
                    assignedLight.enabled = true;
                } else
                {
                    assignedLight.enabled = false;
                }

                yield return null;
            }
            possessionSound.Stop();
            assignedLight.enabled = false;
            transform.localPosition = originalPos;
        }
    }
}