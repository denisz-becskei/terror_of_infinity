using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeyDoorScript : MonoBehaviour
{
    public char openKey;

    void Start()
    {
        TMP_Text[] letters = GetComponentsInChildren<TMP_Text>();
        openKey = WorldWideScripts.Chance(30) == true ? '-' : GetRandomCharacter("bcefghijklmnopqrtuvxyz");
        foreach(TMP_Text text in letters)
        {
            text.text = openKey.ToString();
            if(openKey == '-')
            {
                text.color = Color.red;
            } else
            {
                text.color = Color.cyan;
            }
        }
    }

    private char GetRandomCharacter(string text)
    {
        return text[Random.Range(0, text.Length)];
    }
}
