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
        GetComponent<TMP_Text>().text = promptList[WorldWideScripts.GetTotallyRandomNumberBetween(0, promptList.Length)].Trim();
    }
}
