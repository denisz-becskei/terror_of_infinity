using TMPro;
using UnityEngine;

public class GeneratePrompt : MonoBehaviour
{
    string[] promptList;

    private void Awake()
    {
        promptList = Resources.Load("DeathPrompts").ToString().Split('\n');
    }

    public void Generate()
    {
        Debug.Log("Generating...");
        GetComponent<TMP_Text>().text = promptList[Random.Range(0, promptList.Length)].Trim();
    }
}
