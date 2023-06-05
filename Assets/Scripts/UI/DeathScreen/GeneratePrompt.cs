using TMPro;
using UnityEngine;

public class GeneratePrompt : MonoBehaviour
{
    private void Awake()
    {
        var promptList = Resources.Load("DeathPrompts").ToString().Split('\n');
        GetComponent<TMP_Text>().text = promptList[Random.Range(0, promptList.Length)].Trim();
    }
}
