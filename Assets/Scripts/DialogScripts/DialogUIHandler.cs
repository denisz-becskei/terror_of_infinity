using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogUIHandler : MonoBehaviour, IDataPersistance
{
    [SerializeField] private List<string> scriptTitles = new List<string>();
    [SerializeField] private List<DialogScriptableObject> scripts = new List<DialogScriptableObject>();
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text speechText;

    [SerializeField] private AudioSource audioSource;

    [SerializeField] private DialogScriptableObject[] introReplace;

    private Coroutine dialogRoutine;
    private Coroutine typeRoutine;
    private bool isDialogRunning = false;

    private bool isInterrupted = false;

    private void Start()
    {
        if (Actor.CHARACTER_MAIN_NAME == "")
        {
            StartCoroutine(IntroStart());
        }
    }

    public bool Play(string scriptName, bool canInterrupt)
    {
        DialogScriptableObject dialog = GetDialogByName(scriptName);
        if (dialog == null) return false;

        if(isDialogRunning && canInterrupt)
        {
            StopCoroutine(dialogRoutine);
            isDialogRunning = false;
            SetSpeech("");
            SetSpeaker("");
        }
        if(!isDialogRunning)
        {
            dialogRoutine = StartCoroutine(PlayDialog(dialog));
            return true;
        }

        return false;
    }

    public bool Play(DialogScriptableObject dialog, bool canInterrupt)
    {
        if (isDialogRunning && canInterrupt)
        {
            StopCoroutine(dialogRoutine);
            isDialogRunning = false;
            SetSpeech("");
            SetSpeaker("");
        }

        if(!isDialogRunning)
        {
            dialogRoutine = StartCoroutine(PlayDialog(dialog));
            return true;
        }
        return false;
    }


    private (string, DialogScriptableObject) ModifyDialog(string? dialogToPlay, DialogScriptableObject dialogToModify, DialogScriptableObject dialogToRandomizeFrom, int index, int randomizedIndex, bool doesReturn)
    {
        DialogScriptableObject dialog;
        if (dialogToPlay != null)
        {
            dialog = GetDialogByName(dialogToPlay);
            if(dialog == null)
            {
                return ("", dialogToModify);
            }
        } else
        {
            dialog = dialogToModify;
        }

        dialog.dialogLines[index] = dialogToRandomizeFrom.dialogLines[randomizedIndex];

        if(doesReturn)
        {
            string optionalReturnValue = dialog.dialogLines[index].message;
            return (optionalReturnValue, dialog);
        } else
        {
            return ("", dialog);
        }
    }

    private IEnumerator PlayDialog(DialogScriptableObject dialog)
    {
        isDialogRunning = true;

        for(uint i = 0; i < dialog.dialogLines.Count; i++)
        {
            if (isInterrupted)
            {
                isInterrupted = false;
                break;
            }

            SetSpeech("");
            audioSource.clip = dialog.dialogLines[(int)i].audioClip;
            float audioClipLength = audioSource.clip.length;
            SetSpeaker(dialog.dialogLines[(int)i].sender);
            typeRoutine = StartCoroutine(SetSpeech(dialog.dialogLines[(int)i].message, audioClipLength));
            audioSource.Play();
            yield return new WaitForSeconds(audioClipLength + 1.5f);
        }
        SetSpeaker("");
        SetSpeech("");
        isDialogRunning = false;
    }

    public void InterruptAll()
    {
        if(typeRoutine != null)
        {
            StopCoroutine(typeRoutine);
        }
        
        SetSpeaker("");
        SetSpeech("");
        if (dialogRoutine == null) return;
        isDialogRunning = false;
        audioSource.Stop();
        isInterrupted = true;

        StopCoroutine(dialogRoutine);
    }

    private void SetSpeaker(string speaker)
    {
        if (speaker.Contains("CHARACTER.MAIN.NAME"))
        {
            speaker = speaker.Replace("CHARACTER.MAIN.NAME", Actor.CHARACTER_MAIN_NAME);
        }

        speakerText.text = speaker;
    }

    private void SetSpeech(string speech)
    {
        speechText.text = speech;
    }

    private IEnumerator SetSpeech(string speech, float audioLength)
    {
        if(speech.Contains("CHARACTER.MAIN.NAME"))
        {
            speech = speech.Replace("CHARACTER.MAIN.NAME", Actor.CHARACTER_MAIN_NAME);
        }

        float typewriteEffectDelay = audioLength / speech.Length;
        for(uint i = 0; i < speech.Length; i++)
        {
            speechText.text += speech[(int)i];
            yield return new WaitForSeconds(typewriteEffectDelay / 1.1f);
        }
    }

    private IEnumerator IntroStart()
    {
        yield return new WaitForSeconds(2f);

        int index = UnityEngine.Random.Range(0, introReplace.Length);

        var modified = ModifyDialog("Intro", null, introReplace[0], 3, index, true);
        Actor.CHARACTER_MAIN_NAME = modified.Item1;
        Actor.FixName();
        modified = ModifyDialog(null, modified.Item2, introReplace[1], 7, index, false);
        modified = ModifyDialog(null, modified.Item2, introReplace[2], 8, index, false);
        Play(modified.Item2, true);
    }

    public IEnumerator PlayDelayed(string scriptName, bool canInterrupt, float delay)
    {
        yield return new WaitForSeconds(delay);
        Play(scriptName, canInterrupt);
    }

    private DialogScriptableObject GetDialogByName(string name)
    {
        int dialogIndex = scriptTitles.IndexOf(name);
        if(dialogIndex == -1)
        {
            return null;
        }
        return scripts[dialogIndex];
    }


    public void LoadData(GameStates data)
    {
        Actor.CHARACTER_MAIN_NAME = data.PLAYER_NAME;
    }

    public void SaveData(ref GameStates data)
    {
        data.PLAYER_NAME = Actor.CHARACTER_MAIN_NAME;
    }
}
