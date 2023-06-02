using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public int avgFrameRate;
    public TMP_Text display_Text;

    public void Update()
    {
        float current = Time.frameCount / Time.time;
        avgFrameRate = (int)current;
        display_Text.text = avgFrameRate.ToString() + " FPS";
    }
}