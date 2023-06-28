using UnityEngine;

public class FadeObjectIn : MonoBehaviour
{
    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;

        Color color = material.color;
        color.a = 0f;
        material.color = color;

        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f);

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            Color color = material.color;
            color.a = alpha;
            material.color = color;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Color finalColor = material.color;
        finalColor.a = 1f;
        material.color = finalColor;
    }
}
