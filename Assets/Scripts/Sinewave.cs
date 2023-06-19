using System.Collections;
using UnityEngine;

public class Sinewave : MonoBehaviour
{
    [SerializeField] Material[] materials;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip laserClip;
    [SerializeField] ChromaticColorPosition ccp;
    public BlinkLight flashlight;

    private LineRenderer lr;
    private int points = 45;
    private float frequency = 1;
    private float speed = 1;
    private float timer = 0;

    private bool isActive = true;

    int frameCount = 0;
    bool reset = false;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Draw()
    {
        float xStart = 0;
        float tau = 2 * Mathf.PI;
        float xFinish = 2;

        lr.positionCount = points;
        for(int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = 0.45f * Mathf.Sin((frequency * tau * x) + timer * speed);
            lr.SetPosition(currentPoint, new Vector3(x, y, 0));
        }
    }

    void Update()
    {
        if(isActive)
        {
            Draw();
        }
        DifferenceDeltaTime();
    }

    void FixedUpdate()
    {
        frameCount++;
        if(frameCount % 10 == 0 && isActive)
        {
            FrequencyIncreaser();
        }
        if(!isActive && !reset)
        {
            frameCount = 0;
            reset = true;
            Draw();
        }
    }

    void FrequencyIncreaser()
    {
        frequency += 0.01f;
        frequency = Mathf.Round(frequency * 100f) / 100f;

        if (frequency > 3 && frequency <= 3.5f)
        {
            lr.material = materials[1];
            if(flashlight.blinkRoutine == null)
            {
                flashlight.color = RandomColorSelector();
                flashlight.Blink();
                ccp.isCalled = true;
            }
        } else if(frequency > 3.5f)
        {
            audioSource.clip = laserClip;
            audioSource.Play();

            isActive = false;
            frequency = 0;
            speed = 1;
            lr.material = materials[0];
            flashlight.Interrupt();
            flashlight.Explode(ccp.currentColor);
            ccp.isCalled = false;
            StartCoroutine(Pause());
        }

        if(frequency > 1 && frequency <= 1.02f)
        {
            timer = 0;
            speed += 0.02f;
        }
        else if(frequency > 1)
        {
            speed += 0.02f;
        }
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(5f);
        isActive = true;
        reset = false;
    }

    float lastDeltaTime = 0;
    void DifferenceDeltaTime()
    {
        float currentDeltaTime = Time.timeSinceLevelLoad;
        timer += currentDeltaTime - lastDeltaTime;
        lastDeltaTime = currentDeltaTime;
    }

    Color RandomColorSelector()
    {
        Color[] colors = new Color[4] { Color.red, Color.green, Color.blue, Color.yellow};
        return colors[Random.Range(0, colors.Length)];
    }
}
