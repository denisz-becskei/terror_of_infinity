using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogUIHandler : MonoBehaviour
{
    [SerializeField] private List<DialogScriptableObject> scripts = new List<DialogScriptableObject>();
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text speechText;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(DialogTest());
    }

    public void Play(int dialogToPlay)
    {
        DialogScriptableObject dialog;
        try
        {
            dialog = scripts[dialogToPlay];
        } catch (KeyNotFoundException)
        {
            return;
        }

        StartCoroutine(PlayDialog(dialog));
    }

    private IEnumerator PlayDialog(DialogScriptableObject dialog)
    {
        for(uint i = 0; i < dialog.dialogLines.Count; i++)
        {
            SetSpeech("");
            audioSource.clip = dialog.dialogLines[(int)i].audioClip;
            float audioClipLength = audioSource.clip.length;
            SetSpeaker(dialog.dialogLines[(int)i].sender);
            StartCoroutine(SetSpeech(dialog.dialogLines[(int)i].message, audioClipLength));
            audioSource.Play();
            yield return new WaitForSeconds(audioClipLength + 1.5f);
        }
        SetSpeaker("");
        SetSpeech("");
    }

    private void SetSpeaker(string speaker)
    {
        speakerText.text = speaker;
    }

    private void SetSpeech(string speech)
    {
        speechText.text = speech;
    }

    private IEnumerator SetSpeech(string speech, float audioLength)
    {
        float typewriteEffectDelay = audioLength / speech.Length;
        for(uint i = 0; i < speech.Length; i++)
        {
            speechText.text += speech[(int)i];
            yield return new WaitForSeconds(typewriteEffectDelay);
        }
    }

    private IEnumerator DialogTest()
    {
        yield return new WaitForSeconds(2f);
        Play(0);
    }

    


}
